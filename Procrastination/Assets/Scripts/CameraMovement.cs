using UnityEngine;
using System.Collections;

public class CameraMovement : Draggable {

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

#if UNITY_ANDROID || UNITY_IPHONE

    private Vector2 lastTouchPos = Vector2.zero;
    private Vector2 curTouchPos = Vector2.zero;
    private float speedFactor = 20.0f;
    public new void OnTouchDrag(Vector2 mousePos)
    {
        if (reset)
        {
            reset = false;
            curTouchPos = mousePos;
            return;
        }
        lastTouchPos = curTouchPos;
        curTouchPos = mousePos;

        Vector2 movement = (lastTouchPos - curTouchPos) * speedFactor * Time.deltaTime;
        transform.Translate(movement.x, movement.y, 0);
    }

    public new void endDrag()
    {
        reset = true;
    }

    public new bool isDragged()
    {
        return !reset;
    }
#endif
}
