using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectedItemCanvas : MonoBehaviour {

    /// <summary>
    /// A static reference to this script
    /// </summary>
    public static SelectedItemCanvas can;

    /// <summary>
    /// Holds the rest of the UI for easy toggling
    /// </summary>
    [SerializeField]
    private GameObject holder;

    /// <summary>
    /// Text component displaying the selected item's name
    /// </summary>
    [SerializeField]
    private Text itemNameText;

    /// <summary>
    /// Text component displaying the selected item's price
    /// </summary>
    [SerializeField]
    private Text itemPriceText;


    /// <summary>
    /// Holds the button used to sell an item
    /// </summary>
    [SerializeField]
    private GameObject sellButton;

    /// <summary>
    /// The price of the selected item
    /// </summary>
    private int price = 0;

    /// <summary>
    /// Usedf in order to be able to use functions as parameters
    /// </summary>
    public delegate void callback();

    /// <summary>
    /// Callback function for when the selection is cancelled
    /// </summary>
    private callback cancelCallback = null;

    /// <summary>
    /// Callback function for when the item wants to be bought
    /// </summary>
    private callback buyCallback = null;

    /// <summary>
    /// Callback function for when the item wants to be sold
    /// </summary>
    private callback sellCallback = null;

    // Use this for initialization
    void Awake () {
        can = this;
        holder.SetActive(false);

	}

	
	public void select(string itemName, int price, bool canSell, callback cancelCallback = null, callback buyCallback = null, callback sellCallback = null)
    {
        this.price = price;
        itemNameText.text = itemName;
        itemPriceText.text = "$" + price;
        if (canSell)
        {
            sellButton.SetActive(true);
        }
        else
        {
            sellButton.SetActive(false);
        }

        this.cancelCallback = cancelCallback;
        this.buyCallback = buyCallback;
        this.sellCallback = sellCallback;

        holder.SetActive(true);
    }

    public void deselect()
    {
        holder.SetActive(false);
        if (cancelCallback != null)
        {
            cancelCallback();
        }
    }

    public void buy()
    {
        if (Inventory.inv.getMoney() >= price)
        {
            if (buyCallback != null)
            {
                buyCallback();
            }
            clearCallback();
            deselect();
        }
    }

    public void sell()
    {
        if (sellCallback != null)
        {
            sellCallback();
        }
        clearCallback();
        deselect();
    }

    private void clearCallback()
    {
        cancelCallback = null;
        buyCallback = null;
        sellCallback = null;
        price = 0;
    }
}
