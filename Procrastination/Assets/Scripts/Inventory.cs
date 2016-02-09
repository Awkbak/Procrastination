using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class Inventory : MonoBehaviour{

    /// <summary>
    /// List of objects the user can spawn
    /// </summary>
    public GameObject[] prefabs;



    public void spawnGeneric(BaseEventData data)
    {
        

        //Create Generic game object and get its draggable component
        Draggable obj = (Instantiate(prefabs[0], transform.position, Quaternion.identity) as GameObject).GetComponent<Draggable>();
        #if UNITY_ANDROID || UNITY_IPHONE
        PointerEventData pointerData = data as PointerEventData;
        //Notify the Touch_Handler about this new object and the touch controlling it
        Touch_Handler.handler.addObject(pointerData.pointerId, obj);
        #endif
    }
}
