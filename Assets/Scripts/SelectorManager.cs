using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public class SelectorManager : MonoBehaviour
{
    public GameObject mapSettings;
    //public TMP_InputField xInput;
    //public TMP_InputField zInput;
    public LoadSelector options;
    public GameObject mainMenu;

    public void NewMapMenu()
    {
        mapSettings.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void Generate(string path)
    {
        OnscreenDebug debugger = FindObjectOfType<OnscreenDebug>();
        GameController.newMap = true;
        //GameController.meshWidth = int.Parse(xInput.text);
        //GameController.meshLength = int.Parse(zInput.text);
        if (!string.IsNullOrEmpty(path))
        {
            GameController.loadPath = path.Replace("\\", "/");
            ParsePath(GameController.loadPath);

            string imagePath = GameController.imageFolderPath;
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }
            if (!File.Exists(imagePath + GameController.imageName + GameController.fileExt))
            {
                File.Copy(GameController.loadPath, imagePath + GameController.imageName + GameController.fileExt);
            }
        }
        GameController.SceneChange("NewMap");
    }

    public void Load()
    {
        options.gameObject.SetActive(true);
    }

    // check to see if inputfield that triggers the action can be referenced directly instead of checking both
    /*public void NumberCap()
    {
        if (xInput.text == "-" || zInput.text == "-")
        {
            return;
        }
        if (!string.IsNullOrEmpty(xInput.text) & !string.IsNullOrEmpty(zInput.text))
        {
            if (int.Parse(xInput.text) > 255)
            {
                xInput.text = 255.ToString();
            }
            if (int.Parse(zInput.text) > 255)
            {
                zInput.text = 255.ToString();
            }if (int.Parse(xInput.text) < 1)
            {
                xInput.text = 1.ToString();
            }
            if (int.Parse(zInput.text) < 1)
            {
                zInput.text = 1.ToString();
            }
        }
    }*/

    //trace workflow to figure out why having this in Load2d makes the game crash.
    private void ParsePath(string path)
    {
        string[] pathPieces = path.Split('/');
        string fileName = pathPieces[pathPieces.Length-1];
        string[] fileAndExt = fileName.Split('.');

        GameController.imageName = fileAndExt[0];
        GameController.fileExt = '.' + fileAndExt[1];
        Debug.Log(GameController.imageName);
        Debug.Log(GameController.fileExt);
    }
}
