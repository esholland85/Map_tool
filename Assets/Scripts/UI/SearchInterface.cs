using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchInterface : MonoBehaviour
{
    [SerializeField] private MapSettings settingsPanel;
    Rect explorer;
    Rect selectCorner;
    Vector2 scrollPosition;
    float menuWidth;
    float menuHeight;
    private int windowId = WindowManager.GetWindowId();
    private string directoryPath;
    private string[] currentDirectoryContents;
    private string[] currentDirectoryFiles;
    private string[] currentDirectoryFolders;
    private string[] fileTypes = new string[]
    {
        "*.jpg",
        "*.png",
        "*.psd",
        "*.tiff",
        "*.tga",
        "*.gif",
        "*.bmp",
        "*.iff",
        "*.pict"
    };
    [SerializeField] private Texture2D folderIcon;
    [SerializeField] private Texture2D fileIcon;
    [SerializeField] private Texture2D upIcon;
    private string selectedFile;

    private void Start()
    {
        directoryPath = Application.persistentDataPath + "/testFolder/";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        menuWidth = Screen.width - 200f;
        menuHeight = Screen.height - 100f;
        explorer = new Rect((Screen.width - menuWidth) / 2, (Screen.height - menuHeight) / 2, menuWidth, menuHeight);
        selectCorner = new Rect(menuWidth / 2, menuHeight -75f, menuWidth / 2, 50f);
        ProcessDirectory(directoryPath, fileTypes);
    }

    private void OnGUI()
    {
        GUIStyle clippedStyle = new GUIStyle(GUI.skin.label);

        explorer = GUILayout.Window(windowId, explorer, ExplorerWindow, "File Explorer");
    }

    private void ExplorerWindow(int windowId)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(upIcon, GUILayout.MaxWidth(25)))
        {
            directoryPath = directoryPath.Replace('\\', '/');
            string[] pathArray = directoryPath.Split('/');
            string upDirectory = "";
            if (pathArray[pathArray.Length-1] != "")
            {
                for (int i = 0; i < pathArray.Length - 1; i++)
                {
                    upDirectory += pathArray[i] + "/";
                }
                if (pathArray.Length < 2)
                {
                    upDirectory += pathArray[0] + "/";
                }
            }
            else
            {
                for (int i = 0; i < pathArray.Length - 2; i++)
                {
                    upDirectory += pathArray[i] + "/";
                }
                if (pathArray.Length < 3)
                {
                    upDirectory += pathArray[0] + "/";
                }
            }
            ProcessDirectory(upDirectory, fileTypes);
        }
        GUILayout.Label(directoryPath);
        GUILayout.EndHorizontal();
        
        if (currentDirectoryFiles.Length > 0 || currentDirectoryFolders.Length > 0)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
            int currentRow = 0;
            int maxRow = Mathf.FloorToInt((menuWidth - 25)/85);
            foreach (string filePath in currentDirectoryFolders)
            {
                if (currentRow == 0)
                {
                    GUILayout.BeginHorizontal();
                }
                currentRow++;
                GUILayout.BeginVertical();
                if (GUILayout.Button(folderIcon, GUILayout.MaxHeight(75), GUILayout.MaxWidth(75)))
                {
                    ProcessDirectory(filePath, fileTypes);
                }
                GUILayout.Label(GetFileName(filePath), GUILayout.MaxWidth(75));
                GUILayout.EndVertical();
                if (currentRow == maxRow)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginVertical();
                    GUILayout.Space(10);
                    GUILayout.EndVertical();
                    currentRow = 0;
                }
            }
            foreach (string filePath in currentDirectoryFiles)
            {
                if (currentRow == 0)
                {
                    GUILayout.BeginHorizontal();
                }
                currentRow++;
                GUILayout.BeginVertical();
                if (GUILayout.Button(fileIcon, GUILayout.MaxHeight(75), GUILayout.MaxWidth(75)))
                {
                    selectedFile = filePath;
                    //selectedFile = GetFileName(filePath);
                }
                GUILayout.Label(GetFileName(filePath),GUILayout.MaxWidth(75));
                GUILayout.EndVertical();
                if (currentRow == maxRow)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10);
                    currentRow = 0;
                }
            }
            if (currentRow != 0 && currentDirectoryFiles.Length + currentDirectoryFolders.Length > 0)
            {
                GUILayout.Space((maxRow - currentRow) * 60);
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }
        
        GUILayout.BeginHorizontal();
        GUILayout.Space(menuWidth / 2);
        GUILayout.BeginVertical();
        if (!string.IsNullOrEmpty(selectedFile))
        {
            GUILayout.Label(GetFileName(selectedFile));
        }
        else
        {
            GUILayout.Label("Select an image to use for your map");
        }
        if (GUILayout.Button("Select File"))
        {
            settingsPanel.gameObject.SetActive(true);
            settingsPanel.imagePath = selectedFile;
            gameObject.SetActive(false);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    private string GetFileName(string currentDirectoryPath)
    {
        string[] pathArray = currentDirectoryPath.Split('/');
        pathArray = pathArray[pathArray.Length -1].Split('\\');
        return pathArray[pathArray.Length-1];
    }
    private void ProcessDirectory(string currentDirectoryPath)
    {
        directoryPath = currentDirectoryPath;
        currentDirectoryFolders = Directory.GetDirectories(currentDirectoryPath);
        currentDirectoryFiles = Directory.GetFiles(currentDirectoryPath);
        Array.Sort(currentDirectoryFolders);
        Array.Sort(currentDirectoryFiles);
    }
    private void ProcessDirectory(string currentDirectoryPath, string[] fileTypes)
    {
        directoryPath = currentDirectoryPath;
        List<string> compilationList = new List<string>();
        foreach (string fileType in fileTypes)
        {
            foreach (string file in Directory.GetFiles(currentDirectoryPath,fileType))
            {
                compilationList.Add(file);
            }
        }
        currentDirectoryFolders = Directory.GetDirectories(currentDirectoryPath);
        currentDirectoryFiles = compilationList.ToArray();
        Array.Sort(currentDirectoryFolders);
        Array.Sort(currentDirectoryFiles);
    }
}
