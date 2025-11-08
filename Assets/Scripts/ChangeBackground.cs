using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Windows.Forms;
using System.IO;

public class ChangeBackground : MonoBehaviour
{
    public Image backgroundImage;  // L'immagine di sfondo sul Canvas
    private string filePath;

    private void Start()
    {
        string imagePath = PlayerPrefs.GetString("backgroundImagePath");
        if (imagePath != null || imagePath != "")
        {
            filePath = imagePath;
            StartCoroutine(LoadImage());
        }
    }

    // Metodo per aprire il file browser
    public void OpenFileDialog()
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            filePath = ofd.FileName;
            Debug.Log(filePath);
            if (!string.IsNullOrEmpty(filePath))
            {
                StartCoroutine(LoadImage());
            }
            else
            {
                Debug.LogError("filePath non è valido.");
                Debug.LogError(filePath);
            }
        }
    }

    // Coroutine per caricare l'immagine selezionata
    private IEnumerator LoadImage()
    {
        // Verifica se il file esiste
        if (!File.Exists(filePath))
        {
            Debug.LogError("File non trovato: " + filePath);
            yield break;
        }

        // Carica l'immagine come byte array
        byte[] imageData = File.ReadAllBytes(filePath);
        Debug.Log(imageData.Length);
        

        // Crea una nuova texture
        Texture2D texture = new Texture2D(2,2);
        texture.LoadImage(imageData);
        if (!texture.LoadImage(imageData))
        {
            Debug.Log("Errore nel caricamento della texture.");
            yield break;
        }
        Debug.Log(texture.width+"x"+texture.height);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        Debug.Log(sprite.name);

        // Imposta la texture come sfondo
        backgroundImage.sprite = sprite;

        PlayerPrefs.SetString("backgroundImagePath", filePath);
    }
}

