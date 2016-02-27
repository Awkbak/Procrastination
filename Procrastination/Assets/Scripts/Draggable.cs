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
    /// The position of the object when the drag started
    /// </summary>
    private Vector3 oldPosition;

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

    void OnMouseUp()
    {
        endDrag();
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
            //Save old location
            oldPosition = transform.position;

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

        //Make panels dissapear and check if on a valid location
        if (child.disappear())
        {
            //Move back down to normal height
            transform.Translate(0, -1.0f, 0);
        }
        else//If on invalid location, move back to old position
        {
            transform.position = oldPosition;
        }
    }

    /// <summary>
    /// Gets the snapping size
    /// </summary>
    /// <returns>The tile size (snapping size in units)</returns>
    public int getTileSize()
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
