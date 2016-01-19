using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
    public Vector2 screenSize;
    public float panFactor = 0.9f;
    public Vector4 panEdge;
    public float panSpeed = 5.0f;

    void Awake()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
        panEdge = new Vector4((1 - panFactor) * screenSize.x, panFactor * screenSize.x, (1 - panFactor) * screenSize.y, panFactor * screenSize.y);
    }

    void Update () {
        Vector2 mousePos = Input.mousePosition;
        if(mousePos.x < panEdge[0])
        {
            transform.Translate(-panSpeed * Time.deltaTime, 0, 0);
        }
        else if(mousePos.x > panEdge[1])
        {
            transform.Translate(panSpeed * Time.deltaTime, 0, 0);
        }
        if(mousePos.y < panEdge[2])
        {
            transform.Translate(0, -panSpeed * Time.deltaTime, 0);
        }
        else if(mousePos.y > panEdge[3])
        {
            transform.Translate(0, panSpeed * Time.deltaTime, 0);
        }
	}
    #endif
}
