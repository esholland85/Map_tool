using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject view;
    [SerializeField] private GameObject heightButton;
    [SerializeField] private GameObject heightPanel;
    public static SaveWarning saveWarning;
    public static SaveDialogue saveDialogue;

    private void Start()
    {
        saveWarning = FindObjectOfType<SaveWarning>();
        saveWarning.gameObject.SetActive(false);
        saveDialogue = FindObjectOfType<SaveDialogue>();
        saveDialogue.gameObject.SetActive(false);

        GameController.userInterface = this;
    }

    public void CharacterSelected()
    {
        view.SetActive(true);
    }

    public void NodeSelected()
    {
        //extra processing to simplify code. For efficiency, figure out a better way to tell the input field to update than turning it off and on again.
        heightPanel.SetActive(false);
        heightPanel.SetActive(true);
    }

    public void DeselectNode()
    {
        heightPanel.SetActive(false);
    }

    public void SaveMap()
    {
        //maybe merge save and load screens so players can load a new map in the middle of one.
        saveDialogue.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            heightPanel.SetActive(false);
            view.SetActive(false);
        }
    }
}
