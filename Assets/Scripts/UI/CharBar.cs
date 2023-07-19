using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class CharBar : MonoBehaviour
{
    public Vector2 barPosition;
    public List<Character> characters;
    private List<CharacterButton> buttons;
    public CharacterButton buttonPrefab;

    private void Start()
    {
        transform.position = barPosition;
        if (buttons == null)
        {
            buttons = new List<CharacterButton>();
            //characters = Resources.FindObjectsOfTypeAll(typeof(Character)).Cast<Character>().Where(g => g.tag == "character").ToList(); //this wasn't working to find prefabs. Currently manually selected in editor.
            foreach (Character model in characters)
            {
                CharacterButton temp = Instantiate(buttonPrefab);
                temp.model = model;
                temp.buttonImage = model.buttonImage;
                temp.buttonText = model.modelName;
                temp.transform.SetParent(transform);
                Texture2D tex = AssetPreview.GetAssetPreview(model.gameObject);
                if (tex != null)
                {
                    temp.buttonImage = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f), 10);
                }
                buttons.Add(temp);
            }
        }
    }

    public void Repopulate()
    {
        List<Character> newBodies = new List<Character>(GameController.characterCollection);
        GameController.characterCollection.Clear();
        for (int i = 0; i < newBodies.Count; i++)
        {
            string searchTerm = newBodies[i].modelName;
            Character bodyCheck = characters.Find(c => c.modelName == searchTerm);
            if (bodyCheck != null)
            {
                Character nextSpawn = Instantiate(bodyCheck, GameController.loadedPositions[i], Quaternion.identity); // record y rotation at least?
            }
        }
    }
}
