using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Touch_Handler : MonoBehaviour {

    Dictionary<int, Draggable> used = new Dictionary<int, Draggable>();

    #if UNITY_ANDROID || UNITY_IPHONE
	void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

	// Update is called once per frame
	void Update () {
        #if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
#endif
        Draggable drag;
        RaycastHit hit = new RaycastHit();
	    for(int e = 0; e < Input.touchCount; ++e)
        {
            Touch touch = Input.GetTouch(e);
            if (used.TryGetValue(touch.fingerId, out drag))
            {
                if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    CameraMovement cam = drag.GetComponent<CameraMovement>();
                    if(cam != null)
                    {
                        cam.endDrag();
                    }
                    used.Remove(touch.fingerId);
                }
                else if(touch.phase == TouchPhase.Moved)
                {
                    Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                    touchPos.y = touchPos.z;

                    CameraMovement cam = drag.GetComponent<CameraMovement>();
                    if (cam == null)
                    {
                         drag.OnTouchDrag(touchPos);
                    }
                    else
                    {
                        cam.OnTouchDrag(touchPos);
                    }

                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit, 10.1f))
                {

                    if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                    {

                        if (hit.collider.CompareTag("Draggable"))
                        {
                            Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                            touchPos.y = touchPos.z;
                            drag = hit.collider.GetComponent<Draggable>();

                            drag.OnTouchDrag((Vector2)touchPos);
                            used.Add(touch.fingerId, drag);
                        }
                        else if (hit.collider.CompareTag("Ground"))
                        {
                            Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                            touchPos.y = touchPos.z;
                            drag = Camera.main.GetComponent<Draggable>();
                            CameraMovement cam = drag.GetComponent<CameraMovement>();
                            cam.OnTouchDrag(touchPos);
                            used.Add(touch.fingerId, drag);
                        }
                        

                    }
                }
            }
        }
	}
    #endif
}
