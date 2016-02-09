using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Touch_Handler : MonoBehaviour {


    //Touches only exist on mobile
    #if UNITY_ANDROID || UNITY_IPHONE

    /// <summary>
    /// Holds a dictionary of actively used touches and what they are interacting with
    /// </summary>
    Dictionary<int, Draggable> used = new Dictionary<int, Draggable>();

    /// <summary>
    /// Holds the Event System in order to check if a touch is over the UI
    /// </summary>
    EventSystem eventSystem;

    void Awake()
    {
        //Make sure we are in landscape mode
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        //Fetch the current event system
        eventSystem = EventSystem.current;
    }

	// Update is called once per frame
	void Update () {
        //If on android, make the back button quit the game (doesn't exist on iphone)
        #if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        #endif
        //Object touch is dragging
        Draggable drag;
        //Find what the touch is touching
        RaycastHit hit = new RaycastHit();

        //Go through every touch
	    for(int e = 0; e < Input.touchCount; ++e)
        {
            //Get the touch and see if it is already being used
            Touch touch = Input.GetTouch(e);
            if (used.TryGetValue(touch.fingerId, out drag))
            {
                //If it's used and has ended
                if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    //Try and get the CameraMovement component
                    CameraMovement cam = drag.GetComponent<CameraMovement>();
                    //If it exists, it is a camera
                    if(cam != null)
                    {
                        cam.endDrag();
                    }
                    else
                    {
                        drag.endDrag();
                    }
                    ///Remove touch from the dictionary
                    used.Remove(touch.fingerId);
                }//If the used touch has moved
                else if(touch.phase == TouchPhase.Moved)
                {
                    //Get the new touch position
                    Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                    touchPos.y = touchPos.z;

                    //Try and get the CameraMovement component
                    CameraMovement cam = drag.GetComponent<CameraMovement>();
                    //If it exists, it isn't a camera
                    if (cam == null)
                    {
                         drag.OnTouchDrag(touchPos);
                    }
                    else
                    {
                        cam.OnTouchDrag(touchPos);
                    }

                }
            }//If the touch isn't being used and isn't over a UI object
            else if(!eventSystem.IsPointerOverGameObject(touch.fingerId))
            {
                //Get a ray based on touch position sand see what it hits
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit, 10.1f))
                {
                    //If the touch is moving/beginning
                    if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                    {
                        //See if it touched a draggable object
                        if (hit.collider.CompareTag("Draggable"))
                        {
                            //Get the touched objects Draggable component and make sure it isn't already being dragged
                            drag = hit.collider.GetComponent<Draggable>();
                            if (drag.isDragged())
                            {
                                continue;
                            }
                            //Get the world coordinates of the touch
                            Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                            touchPos.y = touchPos.z;
                            //Tell the object the touches position and add it to the dictionary
                            drag.OnTouchDrag((Vector2)touchPos);
                            used.Add(touch.fingerId, drag);
                        }//If it touched the ground
                        else if (hit.collider.CompareTag("Ground"))
                        {
                            //Get the cameras Draggable component and CameraMovement component
                            drag = Camera.main.GetComponent<Draggable>();
                            CameraMovement cam = drag.GetComponent<CameraMovement>();
                            //Make sure the camera isn't already being dragged
                            if (cam.isDragged())
                            {
                                continue;
                            }
                            //Get the world coordinates of the touch
                            Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                            touchPos.y = touchPos.z;
                            //Tell the camera the touches position and add the object to the dictionary
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
