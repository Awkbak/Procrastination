using UnityEngine;
using System.Collections;

public class Touch_Handler : MonoBehaviour {

    //#if UNITY_ANDROID || UNITY_IPHONE
    // Use this for initialization
    void Start () {
	
	}

	
	// Update is called once per frame
	void Update () {
        RaycastHit hit = new RaycastHit();
	    for(int e = 0; e < Input.touchCount; ++e)
        {
            DebugScript.d.println("Touch");
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(e).position);
            if (Physics.Raycast(ray, out hit, 10.1f))
            {
                DebugScript.d.println("Hit");
                Touch touch = Input.GetTouch(e);
                if(touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                {

                    if (hit.collider.CompareTag("Draggable"))
                    {
                        DebugScript.d.println("Draggable");
                        Vector2 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                        hit.collider.GetComponent<Draggable>().OnTouchDrag(touchPos);
                    }

                }
            }
        }
	}
    //#endif
}
