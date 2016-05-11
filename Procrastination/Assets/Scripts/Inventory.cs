using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class Inventory : MonoBehaviour{

    /// <summary>
    /// A static reference to this inventory
    /// </summary>
    public static Inventory inv;

    /// <summary>
    /// How much money do you have
    /// </summary>
    private int money = 500;

    /// <summary>
    /// List of objects the user can spawn
    /// </summary>
    public GameObject[] prefabs;

    /// <summary>
    /// The Text component displaying the current money
    /// </summary>
    [SerializeField]
    private Text moneyText;

    /// <summary>
    /// The Text component displaying the current hourly rate
    /// </summary>
    [SerializeField]
    private Text hourlyRateText;

    /// <summary>
    /// Money Generated per hour
    /// </summary>
    private int hourlyRate = 1;


    void Awake()
    {
        inv = this;
        updateMoneyUI();
        updateRateUI();
    }

    /// <summary>
    /// Retreives how much money is available
    /// </summary>
    /// <returns>How much money you have</returns>
    public int getMoney()
    {
        return money;
    }

    public void setMoney(int money)
    {
        this.money = money;
        updateMoneyUI();
    }

    public void addMoney(int money)
    {
        this.money += money;
        updateMoneyUI();
    }

    public void increasePay(int rate)
    {
        hourlyRate += rate;
        updateRateUI();
    }

    public void decreasePay(int rate)
    {
        hourlyRate -= rate;
        updateRateUI();
    }

    public void setPay(int pay)
    {
        hourlyRate = pay;
    }

    public int getPay()
    {
        return hourlyRate;
    }

    public void payDay()
    {
        money += hourlyRate;
        updateMoneyUI();
    }
    public bool penalty(float percent, int level)
    {

        return loseMoney((int)(50 * (1 / percent) * (level * 1.2)));

    }

    private void updateMoneyUI()
    {
        moneyText.text = "Total: $" + money;
    }

    private void updateRateUI()
    {
        hourlyRateText.text = "Hourly: $" + hourlyRate;
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
            updateMoneyUI();
            return true;
        }
    }

    public bool loseMoney(int amount)
    {
        bool answer = true;

        if (amount > money)
        {
            money = 0;
            answer = false;
        }
        else
        {
            money -= amount;
        }
        updateMoneyUI();

        return answer;
    }

    /// <summary>
    /// Spawns a long desk with a chair
    /// </summary>
    /// <param name="data">Pointer data sent by Unity</param>
    public void spawnLongWithChair(BaseEventData data)
    {
        spawnGeneric(data, 0, 50);
    }

    /// <summary>
    /// Spawns a long desk without a chair
    /// </summary>
    /// <param name="data">Pointer data sent by Unity</param>
    public void spawnLong(BaseEventData data)
    {
        spawnGeneric(data, 1, 50);
    }

    /// <summary>
    /// Spawns a small desk with a chair
    /// </summary>
    /// <param name="data">Pointer data sent by Unity</param>
    public void spawnSmallWithChair(BaseEventData data)
    {
        print("Spawn 1");
        spawnGeneric(data, 2, 25);
    }

    /// <summary>
    /// Spawns a small desk without a chair
    /// </summary>
    /// <param name="data">Pointer data sent by Unity</param>
    public void spawnSmall(BaseEventData data)
    {
        spawnGeneric(data, 3, 25);
    }

    public void spawnWatercooler(BaseEventData data)
    {
        spawnGeneric(data, 4, 50);
    }

    /// <summary>
    /// Spawn the generic object to the field
    /// </summary>
    /// <param name="data">Mouse Pointer data sent when called by the Unity Event Trigger</param>
    /// <param name="prefabIndex">Index of the prefab to spawn</param>
    /// <param name="price">Price to spawn this item</param>
    public void spawnGeneric(BaseEventData data, int prefabIndex, int price)
    {
        print("Spawn 2");
        //Make sure you have enough money
        if (subtractMoney(price))
        {
            PointerEventData pointerData = data as PointerEventData;

            //Create Generic game object and get its draggable component
            Draggable obj = (Instantiate(prefabs[prefabIndex], transform.position, Quaternion.identity) as GameObject).GetComponent<Draggable>();

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


    public GameObject makeItem(string itemName)
    {
        switch (itemName)
        {
            case "LONG_WITH_CHAIR":
                return Instantiate(prefabs[0], transform.position, Quaternion.identity) as GameObject;
            case "LONG":
                return Instantiate(prefabs[1], transform.position, Quaternion.identity) as GameObject;
            case "SMALL_WITH_CHAIR":
                return Instantiate(prefabs[2], transform.position, Quaternion.identity) as GameObject;
            case "SMALL":
                return Instantiate(prefabs[3], transform.position, Quaternion.identity) as GameObject;
            case "WATERCOOLER":
                return Instantiate(prefabs[4], transform.position, Quaternion.identity) as GameObject;
            case "DOOR":
                return Instantiate(prefabs[5], transform.position, Quaternion.identity) as GameObject;
            default:
                return Instantiate(prefabs[0], transform.position, Quaternion.identity) as GameObject;
        }
    }

}
