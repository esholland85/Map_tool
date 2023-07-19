using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadSelector : MonoBehaviour {
    private string _path;
    public SelectorManager settingsManager;
    public GameObject mainMenu;
    List<string> uiNames;
    List<string> saveNames;

    BinaryFormatter formatter;
    string saveFile;

    float scrollWidth = 200f;
    float scrollHeight = 300f;

    // The variable to control where the scrollview 'looks' into its child elements.
    Vector2 scrollPosition;

    private void Awake()
    {
        formatter = new BinaryFormatter();
        saveFile = GameController.saveFolderPath + "Current.saves";
        FileStream stream;

        if (File.Exists(saveFile))
        {
            stream = new FileStream(saveFile, FileMode.Open);
            SaveData recentSaves = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            uiNames = recentSaves.uiNames;
            saveNames = recentSaves.mapNames;
            //GameController.imageName = recentSaves.saveNames[recentSaves.saveNames.Count - 1];
        }
        else
        {
            uiNames = new List<string>();
            saveNames = new List<string>();
        }

        mainMenu.SetActive(false);
    }

    void OnGUI()
    {
        //BeginGroup to control relative position of content on screen, instead of using default upper left hand corner.
        GUI.BeginGroup(new Rect((Screen.width-scrollWidth)/2, 50, scrollWidth, (scrollHeight + 25)));
        // Begin a scroll view. All rects are calculated automatically -
        // it will use up any available screen space and make sure contents flow correctly.
        scrollPosition = GUILayout.BeginScrollView(
            scrollPosition, GUILayout.Width(scrollWidth), GUILayout.Height(scrollHeight));

        foreach (string uiName in uiNames)
        {
            if (GUILayout.Button(uiName))
            {
                string saveToLoad;
                saveToLoad = saveNames[uiNames.IndexOf(uiName)];
                GameController.imageName = saveToLoad;
                GameController.uiName = uiName;
                Load();
            }
        }

        // End the scrollview we began above.
        GUILayout.EndScrollView();

        // Now we add a button outside the scrollview - this will be shown below
        // the scrolling area.
        if (GUILayout.Button("Return"))
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
            
        GUI.EndGroup();
    }

    private void Load()
    {
        if (File.Exists(GameController.saveFolderPath + GameController.uiName + ".map"))
        {
            MapData data = SaveSystem.LoadMap(GameController.uiName);
            GameController.meshWidth = data.meshWidth;
            GameController.meshLength = data.meshLength;
            GameController.fileExt = data.fileExt;
            GameController.yValues = data.height;
            GameController.uiName = data.uiName;
            Color[] colorMap = new Color[data.rChannel.Length];
            for (int i = 0; i < colorMap.Length; i++)
            {
                colorMap[i] = new Color(data.rChannel[i], data.gChannel[i], data.bChannel[i], data.aChannel[i]);
            }
            GameController.colorMap = colorMap;
            
            if (GameController.imageName != data.imageName)
            {
                Debug.LogError("Saved file contains image name " + data.imageName + " which doesn't match load request: " + GameController.imageName);
            }

            for (int i = 0; i < data.charName.Length; i++)
            {
                GameObject temp = new GameObject();
                Character tempChar = temp.gameObject.AddComponent<Character>();
                tempChar.modelName = data.charName[i];
                GameController.loadedPositions.Add(new Vector3(data.charXPos[i], data.charYPos[i], data.charZPos[i]));
                GameController.characterCollection.Add(tempChar);
            }
        }
        GameController.SceneChange("NewMap");
    }
}
