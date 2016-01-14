using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {

    void OnMouseDrag()
    {
        Vector3 newPos = Input.mousePosition;
        newPos.z = 10;
        newPos = Camera.main.ScreenToWorldPoint(newPos);
        newPos.y = 0;
        transform.position = newPos;
    }
}
