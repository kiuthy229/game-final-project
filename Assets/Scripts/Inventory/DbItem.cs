using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class DbItem : ScriptableObject
{
    public string itemID;
    public string itemName;
    public string itemDescription;
    public string itemQuantity;
    public Sprite itemImage=null;
    public int itemPrice;
    public int numberHeld=1;
    public bool usable=true;
    public bool unique;
    public UnityEvent thisEvent;

    //có thể lấy quantity ở đây, viết 1 hàm nữa trong script Web

    public void Use()
    {
        //thisEvent.Invoke();
        Debug.Log("press use");
    }

    public void DecreaseAmount(int amountToDecrease)
    {
        numberHeld -= amountToDecrease;
        if (numberHeld < 0)
        {
            numberHeld = 0;
        }
    }
}
