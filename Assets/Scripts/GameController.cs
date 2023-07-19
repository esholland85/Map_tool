using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using hDandD;

public class GameController : MonoBehaviour
{
    public static bool newMap = false;
    public static bool terrainMode = false;
    public static MapMode currentMode = MapMode.defaultMode;
    public static List<Character> characterCollection = new List<Character>();
    public static List<Vector3> loadedPositions = new List<Vector3>();

    public static GameController Instance { get; private set; }
    public static TerrainGenerator map;
    public static NodeController activeNode;
    public static UI userInterface;

    public static int imagePort = 10009;
    public static int meshWidth = 10;
    public static int meshLength = 10;
    public static int brushWidth = 3;
    public static int brushLength = 3;
    public static float mapScale = 1;

    public static float[] yValues;
    public static Color[] colorMap;
    public static NodeController[] currentNodes;

    public static string uiName = "New Map";
    public static string imageName = "Map8";
    //public static string imageName;
    public static string fileExt = ".jpg";
    public static string loadPath;
    public static string imageFolderPath;
    public static string saveFolderPath;
    public static string terrainBrush = "rect";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        imageFolderPath = Application.persistentDataPath + "/Map Images/";
        saveFolderPath = Application.persistentDataPath + "/Saved Maps/";
    }

    public static void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
