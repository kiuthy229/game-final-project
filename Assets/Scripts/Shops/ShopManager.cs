using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using SimpleJSON;

public class ShopManager : MonoBehaviour
{
    [Header("Inventory Information")]
    public PlayerInventory playerInventory;
    [SerializeField] private GameObject blankInventorySlot;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Image itemImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private GameObject buyButton;
    public DbItem currentItem;
    public Text coinDisplay;

    Action<string> _createItemsCallback;

    void Start()
    {
        _createItemsCallback = (jsonArrayString) =>
        {
            StartCoroutine(CreateItemsRoutine(jsonArrayString));
        };
        CreateItems();
    }

    public void SetTextAndButton(string name, string description, string price, bool buttonActive)
    {
        nameText.text = name;
        descriptionText.text = description;
        priceText.text = price;
        if (buttonActive)
        {
            buyButton.SetActive(true);
        }
        else
        {
            buyButton.SetActive(false);
        }
    }


    void OnEnable()
    {
        SetTextAndButton("Choose an item to see description", "", "", false);
    }


    public void SetupDescriptionAndButton(string newNameString, string newDescriptionString, string newPriceString, DbItem newItem)
    {
        currentItem = newItem;
        nameText.text = newNameString;
        descriptionText.text = newDescriptionString;
        priceText.text = newPriceString;
        buyButton.SetActive(true);
    }




    public void BuyButtonPressed()
    {
        if (currentItem)
        {
            //Debug.Log("sell item");
            //Debug.Log(currentItem.itemID);
            DBManager.coins -= currentItem.itemPrice;
            StartCoroutine(Main.Instance.Web.BuyItem(currentItem.itemID, DBManager.id));
            coinDisplay.text = "" + DBManager.coins;
        }
    }

    public void CreateItems()
    {
        StartCoroutine(Main.Instance.Web.GetShopItemIDs(_createItemsCallback));
    }


    public IEnumerator CreateItemsRoutine(string jsonArrayString)
    {
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
        Debug.Log(jsonArray);

        for (int i = 0; i < jsonArray.Count; i++)
        {
            Debug.Log(jsonArray[i]);
            bool isDone = false;
            string itemId = jsonArray[i].AsObject["itemID"];
            JSONObject itemInfoJson = new JSONObject();


            //GET EACH ITEM INFO
            Action<string> getItemInfoCallback = (itemInfo) =>
            {
                isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJson = tempArray[0].AsObject;
                Debug.Log(itemInfoJson);
            };

            StartCoroutine(Main.Instance.Web.GetShopItems(itemId, getItemInfoCallback));

            yield return new WaitUntil(() => isDone == true);

            GameObject item = Instantiate(blankInventorySlot,
                        inventoryPanel.transform.position, Quaternion.identity);
            item.transform.SetParent(inventoryPanel.transform);

            ShopSlot newSlot = item.GetComponent<ShopSlot>();
            DbItem newItem = new DbItem();
            newItem.itemName = itemInfoJson["name"];
            newItem.itemDescription = itemInfoJson["description"];
            newItem.itemID = itemId;
            newItem.itemPrice = itemInfoJson["price"];
            newItem.usable = true;
            newItem.unique = true;
            //Debug.Log("item quantity"+itemQuantity);

            //nameText.text = itemInfoJson["name"];
            //descriptionText.text = itemInfoJson["description"];
            //Debug.Log("Instantiated");

            if (newSlot)
            {
                newSlot.Setup(newItem, this);
                Debug.Log("setup");
                //nameText.text = itemInfoJson["name"];
                //descriptionText.text = itemInfoJson["description"];
            }

            Action<Sprite> getItemIconCallback = (downloadedSprite) =>
            {
                item.transform.Find("image").GetComponent<Image>().sprite = downloadedSprite;
                Debug.Log("loaded");
            };
            StartCoroutine(Main.Instance.Web.GetItemIcon(itemId, getItemIconCallback));

        }

    }
}
