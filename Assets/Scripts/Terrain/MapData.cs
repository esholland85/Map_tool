using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData 
{
    public float[] height;
    public float[] rChannel;
    public float[] gChannel;
    public float[] bChannel;
    public float[] aChannel;

    public int meshWidth;
    public int meshLength;
    public string imageName;
    public string fileExt;
    public string uiName;
    public float mapScale;

    public float[] charXPos;
    public float[] charYPos;
    public float[] charZPos;
    public string[] charName;


    public MapData (TerrainGenerator map)
    {
        Color[] colorMap = map.mesh.colors;
        height = new float[map.vertices.Length];
        for (int i = 0; i < map.vertices.Length; i++)
        {
            height[i] = map.vertices[i].y;
        }

        rChannel = new float[colorMap.Length];
        gChannel = new float[colorMap.Length];
        bChannel = new float[colorMap.Length];
        aChannel = new float[colorMap.Length];
        for (int i = 0; i < colorMap.Length; i++)
        {
            rChannel[i] = colorMap[i].r;
            gChannel[i] = colorMap[i].g;
            bChannel[i] = colorMap[i].b;
            aChannel[i] = colorMap[i].a;
        }

        meshWidth = map.meshWidth;
        meshLength = map.meshLength;
        imageName = GameController.imageName;
        fileExt = GameController.fileExt;
        uiName = GameController.uiName;
        mapScale = GameController.mapScale;

        charXPos = new float[GameController.characterCollection.Count];
        charYPos = new float[GameController.characterCollection.Count];
        charZPos = new float[GameController.characterCollection.Count];
        charName = new string[GameController.characterCollection.Count];
        for (int i = 0; i < GameController.characterCollection.Count; i++)
        {
            charXPos[i] = GameController.characterCollection[i].transform.position.x;
            charYPos[i] = GameController.characterCollection[i].transform.position.y;
            charZPos[i] = GameController.characterCollection[i].transform.position.z;
            charName[i] = GameController.characterCollection[i].modelName;
        }
    }
}
