using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterOptions : MonoBehaviour
{
    public List<Character> characters;
    Rect trayLocation;
    Rect contentRect;
    Vector2 scrollPosition;

    float trayHeight = 70f;
    public float xLocation;

    private void Start()
    {
        characters = Resources.FindObjectsOfTypeAll(typeof(Character)).Cast<Character>().Where(g => g.tag == "character").ToList();
        scrollPosition = new Vector2(0, 0); //scroll position does how far on x and y it's currently scrolled, NOT the starting space on the screen. maybe guilayout.box?
        trayLocation = new Rect(xLocation, (Screen.height - trayHeight) * .87f, 300f, 75f);
        contentRect = new Rect(xLocation, (Screen.height - trayHeight) * .87f, 500f, 50f);
    }

    /*void OnGUI()
    {
        
        scrollPosition = GUI.BeginScrollView(trayLocation,scrollPosition,contentRect,true,false);
        if (GUILayout.Button("test"))
        {
            Debug.Log("test button");
        }
        GUILayout.BeginHorizontal();
        foreach (Character character in characters)
        {
            if (character.buttonImage != null)
            {
                if (GUILayout.Button(character.buttonImage,GUILayout.Height(50),GUILayout.Width(50)))
                {
                    Debug.Log(character.modelName + " selected");
                }
            }
            else
            {
                if (GUILayout.Button(character.modelName, GUILayout.Height(50), GUILayout.Width(50)))
                {
                    Debug.Log(character.modelName + " selected");
                }
            }
        }
        if (GUILayout.Button("test"))
        {
            Debug.Log("test button");
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("test"))
        {
            Debug.Log("test button");
        }
        GUI.EndScrollView();
    }*/
}
