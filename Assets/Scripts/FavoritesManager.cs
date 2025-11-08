using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

[System.Serializable]
public class FavoriteData
{
    public int key;
    public string value;
}

[System.Serializable]
public class FavoriteListWrapper
{
    public List<FavoriteData> favorites = new();
}

public class FavoritesManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform favoritesContainer; // contenitore dei prefab
    public GameObject favoritePrefab;    // prefab con input e 2 bottoni
    public RunWithAffinity runWithAffinity;

    private Dictionary<int, string> favorites = new();
    private string savePath;

    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "favorites.json");
    }

    void Start()
    {
        LoadFavorites();
        RefreshUI();
    }

    // 🔹 Aggiunge un nuovo preferito
    public void AddFavorite()
    {
        int newKey = favorites.Count > 0 ? favorites.Keys.Max() + 1 : 1;
        favorites[newKey] = "";

        SaveFavorites();
        RefreshUI();
    }

    // 🔹 Rimuove un preferito
    public void RemoveFavorite(int key)
    {
        if (favorites.ContainsKey(key))
        {
            favorites.Remove(key);
            SaveFavorites();
            RefreshUI();
        }
    }

    // 🔹 Salvataggio su file
    public void SaveFavorites()
    {
        var wrapper = new FavoriteListWrapper();
        foreach (var kvp in favorites)
            wrapper.favorites.Add(new FavoriteData { key = kvp.Key, value = kvp.Value });

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    // 🔹 Caricamento da file
    private void LoadFavorites()
    {
        if (!File.Exists(savePath))
            return;

        string json = File.ReadAllText(savePath);
        var wrapper = JsonUtility.FromJson<FavoriteListWrapper>(json);

        favorites.Clear();
        foreach (var f in wrapper.favorites)
            favorites[f.key] = f.value;
    }

    // 🔹 Rigenera la UI
    private void RefreshUI()
    {
        // pulisce vecchi elementi
        foreach (Transform child in favoritesContainer)
            Destroy(child.gameObject);

        // ricrea la lista
        foreach (var kvp in favorites)
        {
            GameObject obj = Instantiate(favoritePrefab, favoritesContainer);

            var input = obj.GetComponentInChildren<TMP_InputField>();
            var buttons = obj.GetComponentsInChildren<Button>();

            Button startButton = null;
            Button deleteButton = null;

            // trova i bottoni per nome
            foreach (var btn in buttons)
            {
                if (btn.name.ToLower().Contains("start"))
                    startButton = btn;
                else if (btn.name.ToLower().Contains("delete"))
                    deleteButton = btn;
            }

            input.text = kvp.Value;

            // 🔸 Aggiorna il valore salvato quando cambia testo
            input.onEndEdit.AddListener(value =>
            {
                favorites[kvp.Key] = value;
                SaveFavorites();
            });

            // 🔸 Bottone "Start"
            if (startButton != null)
            {
                startButton.onClick.AddListener(() =>
                {
                    OnStartFavorite(input.text);
                });
            }

            // 🔸 Bottone "Delete"
            if (deleteButton != null)
            {
                deleteButton.onClick.AddListener(() =>
                {
                    RemoveFavorite(kvp.Key);
                });
            }
        }
    }

    // 🔹 Azione quando si preme Start su un preferito
    private void OnStartFavorite(string value)
    {
        Debug.Log($"[FavoritesManager] Avvio processo con valore: {value}");

        if(value != "" || value != null) runWithAffinity.RunFavoriteWithAffinityLauncher(value);
    }
}
