using UnityEngine;
using System.Diagnostics;
using System;
using UnityEngine.UI;
using TMPro;

public class RunWithAffinity : MonoBehaviour
{
    public TMP_InputField InputField;
    public TMP_InputField inputBitmaskField;
    string appPathField;
    string bitmaskField;
    public TMP_Text debuginfo;

    [Header("Favorites")]
    public TMP_InputField InputField1;
    public TMP_InputField InputField2;
    public TMP_InputField InputField3;


    //Main Run
    public void RunWithAffinityLauncher()
    {
        appPathField = InputField.text;
        bitmaskField = inputBitmaskField.text;

        // Percorso all'applicazione che vuoi avviare
        //string appPath = @"E:\Games\Game.exe";
        string appPath = appPathField;
        string cmdPath = @"C:\Windows\System32\cmd.exe";

        // Affinità CPU desiderata (CPU 3 e 4 - 0x18)
        string affinityMask = bitmaskField;

        // Comando da eseguire
        string command = $"start \"\" /AFFINITY {affinityMask} \"{appPath}\"";

        // Configurazione del processo
        ProcessStartInfo processInfo = new ProcessStartInfo(cmdPath, "/c " + command);
        processInfo.CreateNoWindow = true; // Non mostrare la finestra del prompt dei comandi
        processInfo.UseShellExecute = false;

        try
        {

            UnityEngine.Debug.Log("Starting process...");
            debuginfo.text += "\nStarting process...";

            Process process = Process.Start(processInfo); //start

            UnityEngine.Debug.Log("Process started");
            debuginfo.text += "\nProcess started...";

            process.WaitForExit();

            UnityEngine.Debug.Log("Process exited with code: " + process.ExitCode);
            debuginfo.text += "\nProcess exited with code: " + process.ExitCode;

        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Error while starting the process: " + e.Message);
            debuginfo.text += "\nError while starting the process: " + e.Message;
        }
    }

    //Run Favorites
    public void RunFavoriteWithAffinityLauncher(string appPathValue)
    {
        bitmaskField = inputBitmaskField.text;

        // Percorso all'applicazione che vuoi avviare
        //string appPath = @"E:\Games\Game.exe";
        string appPath = appPathValue;
        string cmdPath = @"C:\Windows\System32\cmd.exe";

        // Affinità CPU desiderata
        string affinityMask = bitmaskField;

        // Comando da eseguire
        string command = $"start \"\" /AFFINITY {affinityMask} \"{appPath}\"";

        // Configurazione del processo
        ProcessStartInfo processInfo = new ProcessStartInfo(cmdPath, "/c " + command);
        processInfo.CreateNoWindow = true; // Non mostrare la finestra del prompt dei comandi
        processInfo.UseShellExecute = false;

        try
        {

            UnityEngine.Debug.Log("Starting process...");
            debuginfo.text += "\nStarting process...";

            Process process = Process.Start(processInfo); //start

            UnityEngine.Debug.Log("Process started");
            debuginfo.text += "\nProcess started...";

            process.WaitForExit();

            UnityEngine.Debug.Log("Process exited with code: " + process.ExitCode);
            debuginfo.text += "\nProcess exited with code: " + process.ExitCode;

        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Error while starting the process: " + e.Message);
            debuginfo.text += "\nError while starting the process: " + e.Message;
        }
    }
}
