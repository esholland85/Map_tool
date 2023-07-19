using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveWarning : MonoBehaviour
{
    float windowWidth = 200f;
    Rect windowRect;
    int windowId1 = WindowManager.GetWindowId();
    public Texture2D image;
    string userInput;

    private void Awake()
    {
        userInput = GameController.uiName;
    }

    void OnGUI()
    {
        windowRect = new Rect((Screen.width - windowWidth) / 2, 100, windowWidth, 50);
        windowRect = GUILayout.Window(windowId1, windowRect, DoMyWindow, "Overwrite Warning");
    }

    void DoMyWindow(int windowID)
    {
        GUI.skin.window.normal.background = image;
        GUI.skin.window.onNormal.background = image;

        GUILayout.Space(20);

        GUILayout.Label("A previous save file exists with the same name. Saving will delete it. Do you want to continue?");

        GUILayout.Space(20);

        if (GUILayout.Button("Overwrite"))
        {
            SaveSystem.OverwriteSave();
            gameObject.SetActive(false);
        }
        if (GUILayout.Button("No"))
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }
}
