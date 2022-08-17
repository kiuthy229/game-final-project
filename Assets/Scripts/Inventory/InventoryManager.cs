using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using SimpleJSON;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Information")]
    public PlayerInventory playerInventory;
    [SerializeField] private GameObject blankInventorySlot;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Image itemImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text quantityText;
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject sellButton;
    public DbItem currentItem;
    public Text coinDisplay;

    Action<string> _createItemsCallback;

    void Start()
    {
        ClearInventorySlots();
        _createItemsCallback = (jsonArrayString) =>
        {
            StartCoroutine(CreateItemsRoutine(jsonArrayString));
        };
        CreateItems();
    }

    public void SetTextAndButton(string name, string description, string quantity, bool buttonActive)
    {
        nameText.text = name;
        descriptionText.text = description;
        quantityText.text = quantity;
        if (buttonActive)
        {
            useButton.SetActive(true);
            sellButton.SetActive(true);
        }
        else
        {
            useButton.SetActive(false);
            sellButton.SetActive(false);
        }
    }

    /*void MakeInventorySlots()
    {
        if (playerInventory)
        {
            for (int i = 0; i < playerInventory.myInventory.Count; i++)
            {
                if (playerInventory.myInventory[i].numberHeld > 0 ||
                    playerInventory.myInventory[i].itemName == "Bottle")
                {
                    GameObject temp =
                        Instantiate(blankInventorySlot,
                        inventoryPanel.transform.position, Quaternion.identity);
                    temp.transform.SetParent(inventoryPanel.transform);
                    InventorySlot newSlot = temp.GetComponent<InventorySlot>();
                    if (newSlot)
                    {
                        newSlot.Setup(playerInventory.myInventory[i], this);
                    }
                }
            }
        }
    }*/

    void OnEnable()
    {
        ClearInventorySlots();
        //MakeInventorySlots();
        SetTextAndButton("Choose an item to see description", "", "", false);
    }


    public void SetupDescriptionAndButton(string newNameString, string newDescriptionString, string newQuantityString, bool isButtonUsable, DbItem newItem)
    {
        currentItem = newItem;
        nameText.text = newNameString;
        descriptionText.text = newDescriptionString;
        quantityText.text = newQuantityString;
        useButton.SetActive(isButtonUsable);
        sellButton.SetActive(true);
    }

    public void ClearInventorySlots()
    {
        for (int i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            Destroy(inventoryPanel.transform.GetChild(i).gameObject);
        }
    }

    public void UseButtonPressed()
    {
        if (currentItem)
        {
            currentItem.Use();

            ClearInventorySlots();
            _createItemsCallback = (jsonArrayString) =>
            {
                StartCoroutine(CreateItemsRoutine(jsonArrayString));
            };
            CreateItems();
            //MakeInventorySlots();
            StartCoroutine(Main.Instance.Web.UseItem(currentItem.itemID, DBManager.id));
            SetTextAndButton("Choose an item to see description", "", "", false);
            
            //thêm function như sell item bên dưới
        }
    }

    public void SellButtonPressed()
    {
        if (currentItem)
        {
            //Debug.Log("sell item");
            //Debug.Log(currentItem.itemID);
            ClearInventorySlots();
            _createItemsCallback = (jsonArrayString) =>
            {
                StartCoroutine(CreateItemsRoutine(jsonArrayString));
            };
            CreateItems();
            DBManager.coins += currentItem.itemPrice;
            StartCoroutine(Main.Instance.Web.SellItem(currentItem.itemID, DBManager.id));
            coinDisplay.text = "" + DBManager.coins;
        }
    }



    public void CreateItems()
    {
        string userId = DBManager.id;
        StartCoroutine(Main.Instance.Web.GetItemIDs(DBManager.id, _createItemsCallback));
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
            string itemQuantity = jsonArray[i].AsObject["quantity"];
            JSONObject itemInfoJson = new JSONObject();


            //GET EACH ITEM INFO
            Action<string> getItemInfoCallback = (itemInfo) =>
            {
                isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJson = tempArray[0].AsObject;
                //Debug.Log(itemInfoJson);
            };

            StartCoroutine(Main.Instance.Web.GetItems(itemId, getItemInfoCallback));

            yield return new WaitUntil(() => isDone == true);

            GameObject item = Instantiate(blankInventorySlot,
                        inventoryPanel.transform.position, Quaternion.identity);
            item.transform.SetParent(inventoryPanel.transform);

            InventorySlot newSlot = item.GetComponent<InventorySlot>();
            DbItem newItem = new DbItem();
            newItem.itemName = itemInfoJson["name"];
            newItem.itemDescription = itemInfoJson["description"];
            newItem.itemQuantity = itemQuantity;
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

    //thêm một hàm add item routine
}