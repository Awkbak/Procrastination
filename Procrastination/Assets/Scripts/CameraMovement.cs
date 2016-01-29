using UnityEngine;
using System.Collections;

/// <summary>
/// Used in order to control camera panning and zooming
/// </summary>
public class CameraMovement : Draggable {

    /// <summary>
    /// Variables Specific to non-mobile because of a lack of touch
    /// </summary>
    #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
    /// <summary>
    /// Size of the screen in pixels
    /// </summary>
    public Vector2 screenSize;
    /// <summary>
    /// Pan if the mouse is within (1 - panFactor)%  from the edge of the screen
    /// </summary>
    public float panFactor = 0.9f;
    /// <summary>
    /// Holds the pixel locations where each pan begins (left, rightm up, down)
    /// </summary>
    public Vector4 panEdge;
    /// <summary>
    /// How fast to pan in Units/Second
    /// </summary>
    public float panSpeed = 5.0f;

    void Awake()
    {
        //Get the screen size and set the coordinates in which to pan
        screenSize = new Vector2(Screen.width, Screen.height);
        panEdge = new Vector4((1 - panFactor) * screenSize.x, panFactor * screenSize.x, (1 - panFactor) * screenSize.y, panFactor * screenSize.y);
    }

    void Update () {
        Vector2 mousePos = Input.mousePosition;

        //If mouse on left or right pan portion, pan in given direction
        if(mousePos.x < panEdge[0])
        {
            transform.Translate(-panSpeed * Time.deltaTime, 0, 0);
        }
        else if(mousePos.x > panEdge[1])
        {
            transform.Translate(panSpeed * Time.deltaTime, 0, 0);
        }
        //If mouse on top or bottom pan portion, pan in given direction
        if (mousePos.y < panEdge[2])
        {
            transform.Translate(0, -panSpeed * Time.deltaTime, 0);
        }
        else if(mousePos.y > panEdge[3])
        {
            transform.Translate(0, panSpeed * Time.deltaTime, 0);
        }
	}
    #endif


    /// <summary>
    /// If On mobile
    /// </summary>
    #if UNITY_ANDROID || UNITY_IPHONE

    /// <summary>
    /// The previous position of the touch in Unity units
    /// </summary>
    private Vector2 lastTouchPos = Vector2.zero;
    /// <summary>
    /// The current position of the touch in Unity units
    /// </summary>
    private Vector2 curTouchPos = Vector2.zero;
    /// <summary>
    /// Coefficient of panning speed in order to make panning feel normal
    /// </summary>
    private float speedFactor = 20.0f;

    /// <summary>
    /// Pans the screen based on how the current touch is changing
    /// </summary>
    /// <param name="mousePos">Location of the current touch</param>
    public new void OnTouchDrag(Vector2 mousePos)
    {
        //If this is the first call for the current touch, then there is no previous touch yet
        if (reset)
        {
            reset = false;
            curTouchPos = mousePos;
            return;
        }
        //Set last and current touch
        lastTouchPos = curTouchPos;
        curTouchPos = mousePos;

        //Move based on how much the touch has moved and the speedFactor
        Vector2 movement = (lastTouchPos - curTouchPos) * speedFactor * Time.deltaTime;
        transform.Translate(movement.x, movement.y, 0);
    }

    /// <summary>
    /// Called when a touch stops and resets the dragging
    /// </summary>
    public new void endDrag()
    {
        reset = true;
    }

    /// <summary>
    /// Is the object currently being dragged?
    /// </summary>
    /// <returns>Whether or not the object is being dragged</returns>
    public new bool isDragged()
    {
        return !reset;
    }
#endif
}
