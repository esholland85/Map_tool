using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public Character model;
    public HoloController holoModel;
    public Sprite buttonImage;
    public string buttonText;

    private void Start()
    {
        holoModel = model.holoFrame;
        if (buttonImage != null)
        {
            Image currentImage = GetComponent<Image>();
            currentImage.sprite = buttonImage;
        }
        else
        {
            Text currentText = GetComponentInChildren<Text>();
            currentText.text = buttonText;
        }
    }

    public void MyMethod()
    {
        HoloController temp = Instantiate(holoModel);
        temp.model = model;
    }
}
