using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using hDandD;

public class ToolsController : MonoBehaviour
{
    // Draws a window you can resize between 80px and 200px height
    // Just click the box inside the window and move your mouse
    float trayWidth = 260f;
    float trayHeight = 50f;
    bool scaling = false;
    public TerraformingOptions terrainBar;
    public CharBar charBar;

    public Texture2D tex1;
    public Texture2D tex2;
    public Texture2D tex3;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect((Screen.width-trayWidth)/2,(Screen.height)*.9f,trayWidth,trayHeight));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(tex1, GUILayout.MaxHeight(50), GUILayout.MaxHeight(50)))
        {
            ModeSwitch(MapMode.terrainMode);
        }
        if (GUILayout.Button(tex2, GUILayout.MaxHeight(50), GUILayout.MaxHeight(50)))
        {
            ModeSwitch(MapMode.charMode);
        }
        GUILayout.Button(tex3, GUILayout.MaxHeight(50), GUILayout.MaxHeight(50));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void ModeSwitch(MapMode newMode)
    {
        CloseAllTools();
        if (GameController.currentMode == newMode)
        {
            GameController.currentMode = MapMode.defaultMode;
            return;
        }
        GameController.currentMode = newMode;
        if (newMode == MapMode.terrainMode)
        {
            terrainBar.gameObject.SetActive(true);
            terrainBar.xLocation = (Screen.width - trayWidth) / 2 - 25;
        }
        else if (newMode == MapMode.charMode)
        {
            charBar.gameObject.SetActive(true);
            charBar.barPosition = new Vector2(Screen.width/2, Screen.height *.13f);
        }
    }

    private void CloseAllTools()
    {
        terrainBar.gameObject.SetActive(false);
        charBar.gameObject.SetActive(false);
    }
}
