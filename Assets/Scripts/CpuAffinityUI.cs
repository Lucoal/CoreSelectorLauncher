using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;
using System.Runtime.InteropServices;
using Microsoft.Win32;

public class CpuAffinityUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform coresParent;       // Container con GridLayoutGroup
    public GameObject coreTogglePrefab; // Prefab con Toggle + Label
    public TMP_Text bitmaskText;            // Mostra il valore della bitmask
    public TMP_InputField maskInputField;   // Campo di input manuale (opzionale)
    public Button allButton;
    public Button noneButton;

    private List<Toggle> coreToggles = new();
    private int coreCount;

    private bool updatingFromInput = false;

    string processorName;

    void Start()
    {
        string cpuName = GetCpuName();

        // Ottieni numero di core logici
        coreCount = Environment.ProcessorCount;
        processorName = $"CPU: {cpuName} ({coreCount} cores)";
        GenerateCoreToggles();

        allButton?.onClick.AddListener(SelectAllCores);
        noneButton?.onClick.AddListener(DeselectAllCores);

        if (maskInputField != null)
            maskInputField.onEndEdit.AddListener(OnMaskInputChanged);

        UpdateBitmaskDisplay();
    }

    void GenerateCoreToggles()
    {
        for (int i = 0; i < coreCount; i++)
        {
            var obj = Instantiate(coreTogglePrefab, coresParent);
            obj.name = $"Core {i}";
            var toggle = obj.GetComponentInChildren<Toggle>();
            var label = obj.GetComponentInChildren<TMP_Text>();

            label.text = $"Core {i}";
            toggle.isOn = true; // di default tutti attivi
            toggle.onValueChanged.AddListener(_ => OnCoreToggleChanged());
            coreToggles.Add(toggle);
        }
    }

    void OnCoreToggleChanged()
    {
        if (updatingFromInput) return;
        UpdateBitmaskDisplay();
    }

    void UpdateBitmaskDisplay()
    {
        int mask = 0;
        for (int i = 0; i < coreToggles.Count; i++)
        {
            if (coreToggles[i].isOn)
                mask |= (1 << i);
        }

        string hex = $"0x{mask:X}";
        string binary = Convert.ToString(mask, 2).PadLeft(coreCount, '0');

        bitmaskText.text = $"Processor: {processorName}\nBitmask: {hex}\nBinary: {binary}";
        if (!updatingFromInput && maskInputField != null)
            maskInputField.text = hex;
    }

    void OnMaskInputChanged(string input)
    {
        try
        {
            updatingFromInput = true;
            if (string.IsNullOrWhiteSpace(input))
                return;

            int mask = 0;
            if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                mask = Convert.ToInt32(input, 16);
            else
                mask = Convert.ToInt32(input);

            for (int i = 0; i < coreToggles.Count; i++)
            {
                bool active = (mask & (1 << i)) != 0;
                coreToggles[i].isOn = active;
            }

            UpdateBitmaskDisplay();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Invalid mask input: {input} ({e.Message})");
        }
        finally
        {
            updatingFromInput = false;
        }
    }

    void SelectAllCores()
    {
        foreach (var t in coreToggles) t.isOn = true;
        UpdateBitmaskDisplay();
    }

    void DeselectAllCores()
    {
        foreach (var t in coreToggles) t.isOn = false;
        UpdateBitmaskDisplay();
    }

    public int GetCurrentMask()
    {
        int mask = 0;
        for (int i = 0; i < coreToggles.Count; i++)
        {
            if (coreToggles[i].isOn)
                mask |= (1 << i);
        }
        return mask;
    }

    string GetCpuName()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        try
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0"))
            {
                if (key != null)
                {
                    object value = key.GetValue("ProcessorNameString");
                    if (value != null)
                        return value.ToString().Trim();
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Unable to read CPU name from registry: {e.Message}");
        }
#endif

#if UNITY_ANDROID
        try
        {
            using (var build = new AndroidJavaClass("android.os.Build"))
            {
                string hw = build.GetStatic<string>("HARDWARE");
                string model = build.GetStatic<string>("MODEL");
                return $"{model} ({hw})";
            }
        }
        catch { }
#endif

        // Fallback universale
        return SystemInfo.processorType;
    }
}
