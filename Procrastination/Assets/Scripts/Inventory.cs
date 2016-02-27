using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class Inventory : MonoBehaviour{

    /// <summary>
    /// List of objects the user can spawn
    /// </summary>
    public GameObject[] prefabs;


    /// <summary>
    /// How much money do you have
    /// </summary>
    private int money = 2;

    /// <summary>
    /// Retreives how much money is available
    /// </summary>
    /// <returns>How much money you have</returns>
    public int getMoney()
    {
        return money;
    }

    /// <summary>
    /// Subtracts a designated amount of money from you
    /// </summary>
    /// <param name="amount">The amount of money to subtract</param>
    /// <returns>Whether or not "amout" was valid (more than you currently own)</returns>
    public bool subtractMoney(int amount)
    {
        if(amount > money)
        {
            return false;
        }
        else
        {
            money -= amount;
            return true;
        }
    }


    /// <summary>
    /// Spawn the generic object to the field
    /// </summary>
    /// <param name="data">Mouse Pointer data sent when called by the Unity Event Trigger</param>
    public void spawnGeneric(BaseEventData data)
    {
        //Make sure you have enough money
        if (subtractMoney(1))
        {
            PointerEventData pointerData = data as PointerEventData;

            //Create Generic game object and get its draggable component
            Draggable obj = (Instantiate(prefabs[0], transform.position, Quaternion.identity) as GameObject).GetComponent<Draggable>();

            //Touch_Handler doesn't handle mouse clicks
            #if UNITY_ANDROID || UNITY_IPHONE

            if (pointerData.pointerId >= 0)
            {
                //Notify the Touch_Handler about this new object and the touch controlling it
                Touch_Handler.handler.addObject(pointerData.pointerId, obj);
                return;
            }
            #endif
            //Snap spawned object to grid near mouse
            Vector2 position = pointerData.position;

            Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 10));
            touchPos.y = touchPos.z;

            touchPos.x -= touchPos.x % obj.getTileSize();
            touchPos.y -= touchPos.y % obj.getTileSize();

            transform.position = new Vector3(touchPos.x, 0, touchPos.y);

        }
        else
        {
            print("No Monies");
        }
    }

}
