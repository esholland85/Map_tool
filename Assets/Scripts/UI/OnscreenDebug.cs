using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnscreenDebug : MonoBehaviour
{
    private TextMeshProUGUI textBox;

    private void Awake()
    {
        textBox = GetComponent<TextMeshProUGUI>();
    }

    public void Log(string message)
    {
        //textBox.text += System.Environment.NewLine + message;
        textBox.text += message + ", ";
    }
}
