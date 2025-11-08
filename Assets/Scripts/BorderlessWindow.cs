using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BorderlessWindow : MonoBehaviour
{
    // Import delle funzioni API di Windows
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", SetLastError = true)]
    static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    // Costanti necessarie per le API di Windows
    const int GWL_STYLE = -16; // Indice per la modifica dello stile della finestra
    const int WS_OVERLAPPEDWINDOW = 0x00CF0000; // Stile finestra normale (con bordi)
    const int WS_POPUP = unchecked((int)0x80000000); // Stile finestra senza bordi

    const uint SWP_NOMOVE = 0x0002; // Non muovere la finestra
    const uint SWP_NOSIZE = 0x0001; // Non ridimensionare la finestra
    const uint SWP_NOZORDER = 0x0004; // Non modificare l'ordine di Z
    const uint SWP_FRAMECHANGED = 0x0020; // Applicare i cambiamenti del frame

    void Start()
    {
        // Ottieni l'handle della finestra attiva
        IntPtr hWnd = GetActiveWindow();

        // Cambia lo stile della finestra per rimuovere i bordi
        int style = GetWindowLong(hWnd, GWL_STYLE);
        SetWindowLong(hWnd, GWL_STYLE, (style & ~WS_OVERLAPPEDWINDOW) | WS_POPUP);

        // Aggiorna la finestra per applicare le modifiche
        SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
    }
}
