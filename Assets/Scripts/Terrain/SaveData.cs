using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<string> mapNames;
    public List<string> uiNames;



    public SaveData (string newSaveName, string newUIName)
    {
        mapNames = new List<string>();
        uiNames = new List<string>();

        mapNames.Add(newSaveName);
        uiNames.Add(newUIName);
    }
}
