using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPoint : MonoBehaviour {
    private Renderer objectRenderer;
    private Vector3 screenPosition;
    public string text;

    void Start() {
        objectRenderer = GetComponent<Renderer>();
    }

    void OnGUI() {
        if (objectRenderer.isVisible) {
            screenPosition = Manager.instance.mainCam.WorldToScreenPoint(transform.position);
            Vector2 textSize = GUI.skin.label.CalcSize(new GUIContent(text));
            GUI.Label(new Rect(screenPosition.x, Screen.height - screenPosition.y, textSize.x, textSize.y), text);
        }
    }
}