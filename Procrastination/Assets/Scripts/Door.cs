using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    /// <summary>
    /// The cost to buy this door
    /// </summary>
    private int price = 250;

    /// <summary>
    /// The cieling of the office attached to this door
    /// </summary>
    [SerializeField]
    private GameObject officeCieling = null;

    /// <summary>
    /// Is this door selected
    /// </summary>
    [SerializeField]
    private bool selected = false;
    
    /// <summary>
    /// Amount of time until the door stops being selected
    /// </summary>
    private float selectedTimeOut = 1.0f;

    /// <summary>
    /// Timer used to see time since the door was selected
    /// </summary>
    private float selectedTimeOutTimer = 0.0f;

    /// <summary>
    /// Handles movement when not on mobile
    /// </summary>
    #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
    void OnMouseDown()
    {
        selectDoor();
    }
    #endif


    public void setPrice(int price)
    {
        this.price = price;
    }

    public int getPrice()
    {
        return price;
    }

    public void selectDoor()
    {
        if (!selected)
        {
            selected = true;
            SelectedItemCanvas.can.select("Door", price, false, deselect, buyDoor);
        }

    }

    public void buyDoor()
    {
        if (Inventory.inv.subtractMoney(price))
        {
            if(officeCieling != null)
            {
                Destroy(officeCieling.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void deselect()
    {
        selected = false;
    }

}
