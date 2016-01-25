using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {


    private int tileSize = 1;
    protected CheckGen child;
    protected bool reset = true;

    #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
    void OnMouseDrag()
    {
        Vector3 newPos = Input.mousePosition;
        newPos.z = 10;
        newPos = Camera.main.ScreenToWorldPoint(newPos);
        newPos.y = newPos.z;
        OnTouchDrag(newPos);
    }
    #endif
    public void OnTouchDrag(Vector2 mousePos)
    {
        if (reset)
        {
            transform.Translate(0, 1.0f, 0);
            reset = false;
            child.appear();
        }
        mousePos.x -= mousePos.x % tileSize;
        mousePos.y -= mousePos.y % tileSize;
        if (!mousePos.Equals(transform.position))
        {
            child.recheck();
            transform.position = new Vector3(mousePos.x, transform.position.y, mousePos.y);
        }
    }

    public void endDrag()
    {
        reset = true;
        transform.Translate(0, -1.0f, 0);
        child.disappear();
    }

    public int getSize()
    {
        return tileSize;
    }

    public bool isDragged()
    {
        return !reset;
    }


}
