using UnityEngine.EventSystems;
using UnityEngine;

public class TerraformingOptions : MonoBehaviour
{
    Rect windowRect1;
    Rect windowRect2;
    int windowId1 = WindowManager.GetWindowId();
    int windowId2 = WindowManager.GetWindowId();
    string userInput1;
    string userInput2;

    float trayHeight = 70f;
    public float xLocation;

    private void Start()
    {
        windowRect1 = new Rect(xLocation, (Screen.height - trayHeight) * .87f, 75, 50);
        windowRect2 = new Rect(xLocation + 80, (Screen.height - trayHeight) * .87f, 75, 50);
    }

    void OnGUI()
    {
        windowRect1 = GUILayout.Window(windowId1, windowRect1, DoWindow1, "Width");
        windowRect2 = GUILayout.Window(windowId2, windowRect2, DoWindow2, "Length");
    }

    void DoWindow1(int windowID)
    {
        GUILayout.BeginVertical();
        if (Event.current.type == EventType.ScrollWheel)
        {
            if (Event.current.delta.y > 0 && GameController.brushWidth > 1)
            {
                GameController.brushWidth--;
            }
            if (Event.current.delta.y < 0 && GameController.brushWidth <= GameController.meshWidth)
            {
                GameController.brushWidth++;
            }
        }
        GameController.brushWidth = Mathf.CeilToInt(GUILayout.HorizontalSlider(GameController.brushWidth, 1, GameController.meshWidth + 1));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(GameController.brushWidth.ToString());
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
    void DoWindow2(int windowID)
    {
        GUILayout.BeginVertical();
        if (Event.current.type == EventType.ScrollWheel)
        {
            if (Event.current.delta.y > 0 && GameController.brushLength > 1)
            {
                GameController.brushLength--;
            }
            if (Event.current.delta.y < 0 && GameController.brushLength <= GameController.meshLength)
            {
                GameController.brushLength++;
            }
        }
        GameController.brushLength = Mathf.CeilToInt(GUILayout.HorizontalSlider(GameController.brushLength, 1, GameController.meshLength + 1));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(GameController.brushLength.ToString());
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
    
}
