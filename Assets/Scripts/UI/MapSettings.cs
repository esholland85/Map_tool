using UnityEngine;
using System.Text.RegularExpressions;

public class MapSettings : MonoBehaviour
{
    Rect menu;
    float menuWidth = 400f;
    float menuHeight = 300f;
    public SearchInterface searchBox;
    public string imagePath;
    public SelectorManager selectorManager;
    public GameObject mainMenu;
    private int windowId = WindowManager.GetWindowId();
    bool active = true;

    string mapWidthField = "30";
    string mapLengthField = "30";
    string squareSizeField = "5";

    private void Start()
    {
        menu = new Rect((Screen.width - menuWidth) / 2, (Screen.height - menuHeight) / 2, menuWidth, menuHeight);
    }

    private void OnGUI()
    {
        menu = GUILayout.Window(windowId, menu, MainMenuWindow,"Map Settings");
    }

    private void MainMenuWindow(int windowId)
    {
        GUILayout.BeginVertical();
        GUILayout.Space(50);

        GUILayout.BeginHorizontal();
        GUILayout.Space(menuWidth / 3);
        GUILayout.Label("Feet between height nodes: ");
        squareSizeField = GUILayout.TextField(squareSizeField, GUILayout.MaxWidth(menuWidth / 3));
        squareSizeField = Regex.Replace(squareSizeField, @"[^0-9.]", "");
        if (string.IsNullOrEmpty(squareSizeField))
        {
            squareSizeField = "0";
        }
        if (!string.IsNullOrEmpty(squareSizeField))
        {
            if (float.Parse(squareSizeField) > 255)
            {
                squareSizeField = "255";
            }
            if (float.Parse(squareSizeField) < 0)
            {
                squareSizeField = "0";
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(menuWidth / 3);
        GUILayout.Label("Map Width: ");
        mapWidthField = GUILayout.TextField(mapWidthField, GUILayout.MaxWidth(menuWidth/3));
        mapWidthField = Regex.Replace(mapWidthField, @"[^0-9]","");
        if (!string.IsNullOrEmpty(mapWidthField))
        {
            if (int.Parse(mapWidthField) > 255)
            {
                mapWidthField = "255";
            }
            if (int.Parse(mapWidthField) < 1)
            {
                mapWidthField = "1";
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(menuWidth / 3);
        GUILayout.Label("Map Length: ");
        mapLengthField = GUILayout.TextField(mapLengthField, GUILayout.MaxWidth(menuWidth / 3));
        mapLengthField = Regex.Replace(mapLengthField, @"[^0-9]", "");
        if (!string.IsNullOrEmpty(mapLengthField))
        {
            if (int.Parse(mapLengthField) > 255)
            {
                mapLengthField = "255";
            }
            if (int.Parse(mapLengthField) < 1)
            {
                mapLengthField = "1";
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Find Image",GUILayout.MaxWidth(menuWidth/3 - 5)))
        {
            searchBox.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        if (GUILayout.Button("Generate", GUILayout.MaxWidth(menuWidth / 3 - 5)))
        {
            GameController.meshWidth = int.Parse(mapWidthField);
            GameController.meshLength = int.Parse(mapLengthField);
            if (float.Parse(squareSizeField) == 0)
            {
                GameController.mapScale = 25;
            }
            else
            {
                GameController.mapScale = 5f / float.Parse(squareSizeField);
            }
            selectorManager.Generate(imagePath);
        }
        if (GUILayout.Button("Cancel", GUILayout.MaxWidth(menuWidth / 3 - 5)))
        {
            mainMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        GUILayout.EndHorizontal();
    }
}
