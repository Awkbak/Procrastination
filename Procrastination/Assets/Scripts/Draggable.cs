using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {


    protected int tileSize = 2;

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
        mousePos.x -= mousePos.x % tileSize;
        mousePos.y -= mousePos.y % tileSize;
        transform.position = new Vector3(mousePos.x, 0, mousePos.y);
        DebugScript.d.println("Called wrong draggable");
    }

}
