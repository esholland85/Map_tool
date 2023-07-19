using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    public static void SaveMap ()
    {
        TerrainGenerator map = GameController.map;
        MapData data = new MapData(map);
        BinaryFormatter formatter = new BinaryFormatter();
        // simplify path to troubleshoot if problems arise. tutorial used persistentDataPath + "/player.fun";
        string savePath = GameController.saveFolderPath;
        FileStream stream;

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        if (!File.Exists(savePath + GameController.uiName + ".map"))
        {
            SaveData currentSave;
            if (!File.Exists(savePath + "Current.saves"))
            {
                currentSave = new SaveData(GameController.imageName, GameController.uiName);

                stream = new FileStream(savePath + "Current.saves", FileMode.Create);
                formatter.Serialize(stream, currentSave);
                stream.Close();

                stream = new FileStream(savePath + GameController.uiName + ".map", FileMode.Create);
                formatter.Serialize(stream, data);
                stream.Close();
            }
            else
            {
                currentSave = LoadSaveData();
                if (currentSave.uiNames.IndexOf(GameController.uiName) >= 0)
                {
                    UI.saveWarning.gameObject.SetActive(true);
                }
                else
                {
                    currentSave.mapNames.Add(GameController.imageName);
                    currentSave.uiNames.Add(GameController.uiName);

                    stream = new FileStream(savePath + "Current.saves", FileMode.Create);
                    formatter.Serialize(stream, currentSave);
                    stream.Close();

                    stream = new FileStream(savePath + GameController.uiName + ".map", FileMode.Create);
                    formatter.Serialize(stream, data);
                    stream.Close();
                }  
            }   
        }
        else
        {
            UI.saveWarning.gameObject.SetActive(true);
        }
    }

    public static MapData LoadMap (string saveName)
    {
        string saveFile = GameController.saveFolderPath + saveName + ".map";
        if (File.Exists(saveFile))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveFile, FileMode.Open);
            MapData data = formatter.Deserialize(stream) as MapData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + saveFile);
            return null;
        }
    }

    private static SaveData LoadSaveData()
    {
        string loadPath = GameController.saveFolderPath + "Current.saves";
        if (File.Exists(loadPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(loadPath, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + loadPath);
            return null;
        }
    }

    public static void OverwriteSave()
    {
        //fix up so .maps receive UI name so multiple can exist of the same base image.
        string savePath = GameController.saveFolderPath;
        if (File.Exists(savePath + "Current.saves"))
        {
            SaveData saveFiles = LoadSaveData();
            int saveIndex = saveFiles.uiNames.IndexOf(GameController.uiName);
            while (saveIndex >= 0)
            {
                if (File.Exists(savePath + saveFiles.uiNames[saveIndex] + ".map"))
                {
                    File.Delete(savePath + saveFiles.uiNames[saveIndex] + ".map");
                }
                saveFiles.uiNames.RemoveAt(saveIndex);
                saveFiles.mapNames.RemoveAt(saveIndex);
                saveIndex = saveFiles.uiNames.IndexOf(GameController.uiName);
            }
            FileStream stream = new FileStream(savePath + "Current.saves", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, saveFiles);
            stream.Close();
        }
        if (File.Exists(savePath + GameController.uiName + ".map"))
        {
            File.Delete(savePath + GameController.uiName + ".map");
        }
        SaveMap();
    }
}