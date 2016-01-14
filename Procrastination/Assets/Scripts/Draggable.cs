using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {

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
        mousePos.x -= mousePos.x % 2;
        mousePos.y -= mousePos.y % 2;
        transform.position = new Vector3(mousePos.x, 0, mousePos.y);
    }

}
