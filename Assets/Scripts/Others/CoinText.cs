using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinText : MonoBehaviour
{
/*    public Inventory playerInventory;*/
    public Text coinDisplay;
    private void Start()
    {
        coinDisplay.text = "" + DBManager.coins;
    }
    
    public void UpdateCoinCount()
    {
/*        playerInventory.coins++;*/
/*        DBManager.coins++;*/
        coinDisplay.text = "" + DBManager.coins;
    }
}
