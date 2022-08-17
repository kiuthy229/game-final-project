using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;

    public DbItem thisItem;
    public ShopManager thisManager;


    public void Setup(DbItem newItem, ShopManager newManager)
    {
        thisItem = newItem;
        thisManager = newManager;
        if (thisItem)
        {
            //itemImage.sprite = thisItem.itemImage
            //itemNumberText.text = "" + thisItem.numberHeld;
            itemImage.sprite = null;
            //Debug.Log("Setup xong");
        }
    }
    public void ClickedOn()
    {
        if (thisItem)
        {
            //Debug.Log("ClickedOn");
            thisManager.SetupDescriptionAndButton(thisItem.itemName, thisItem.itemDescription, "Price:" + thisItem.itemPrice.ToString(), thisItem);
        }
    }
}
