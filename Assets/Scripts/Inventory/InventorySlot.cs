using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Text itemNumberText;
    [SerializeField] private Image itemImage;

    public DbItem thisItem;
    public InventoryManager thisManager;


    public void Setup(DbItem newItem, InventoryManager newManager)
    {
        thisItem = newItem;
        thisManager = newManager;
        if (thisItem)
        {
            //itemImage.sprite = thisItem.itemImage
            //itemNumberText.text = "" + thisItem.numberHeld;
            itemImage.sprite = null;
            itemNumberText.text = "";
            //Debug.Log("Setup xong");
        }
    }
    public void ClickedOn()
    {
        if (thisItem)
        {
            //Debug.Log("ClickedOn");
            thisManager.SetupDescriptionAndButton(thisItem.itemName, thisItem.itemDescription, "Quantity: "+thisItem.itemQuantity, thisItem.usable, thisItem);
        }
    }

}
