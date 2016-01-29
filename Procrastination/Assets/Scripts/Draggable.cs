using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {

    /// <summary>
    /// Snapping size (in Unity units)
    /// </summary>
    private int tileSize = 1;
    /// <summary>
    /// Holds checkGen that handles the obstruction panels
    /// </summary>
    protected CheckGen child;
    /// <summary>
    /// Makes sure the object correctly resets at each new touch
    /// </summary>
    protected bool reset = true;

    /// <summary>
    /// Handles movement when not on mobile
    /// </summary>
    #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
    void OnMouseDrag()
    {
        //Get the mouse position in world coordinates and send it to OnTOuchDrag
        Vector3 newPos = Input.mousePosition;
        newPos.z = 10;
        newPos = Camera.main.ScreenToWorldPoint(newPos);
        newPos.y = newPos.z;
        OnTouchDrag(newPos);
    }
    #endif

    /// <summary>
    /// Handles translation of the object
    /// </summary>
    /// <param name="mousePos">The position in World Coordinates of the touch or thing dragging the object</param>
    public void OnTouchDrag(Vector2 mousePos)
    {
        //If the drag just started
        if (reset)
        {
            //Raise the object up and make obstruction panels appear
            transform.Translate(0, 1.0f, 0);
            reset = false;
            child.appear();
        }
        //Snap mousePos to the nearing tile
        mousePos.x -= mousePos.x % tileSize;
        mousePos.y -= mousePos.y % tileSize;
        //If the mousePos changed this time
        if (!mousePos.Equals(transform.position))
        {
            //Update the obstruction panels and move to the new position
            child.recheck();
            transform.position = new Vector3(mousePos.x, transform.position.y, mousePos.y);
        }
    }

    /// <summary>
    /// Called when object stops being dragged to help reset settings
    /// </summary>
    public void endDrag()
    {
        reset = true;
        //Move back down to normal height and make panels dissapear
        transform.Translate(0, -1.0f, 0);
        child.disappear();
    }

    /// <summary>
    /// Gets the snapping size
    /// </summary>
    /// <returns></returns>
    public int getSize()
    {
        return tileSize;
    }


    /// <summary>
    /// Whether or not this object is currently being dragged
    /// </summary>
    /// <returns>Is this object being dragged?</returns>
    public bool isDragged()
    {
        return !reset;
    }


}
