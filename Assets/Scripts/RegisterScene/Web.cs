using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using SimpleJSON;

public class Web : MonoBehaviour
{
    public string[] items;
    [SerializeField] private GameObject blankInventorySlot;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject shop;
    [SerializeField] private InventoryManager thisInventoryManager;
    Action<string> _createItemsCallback;
    [SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text quantityText;
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject sellButton;

    void Start()
    {
        /*        StartCoroutine(GetUsers());*/
       /* _createItemsCallback = (jsonArrayString) =>
        {
            StartCoroutine(CreateItemsRoutine(jsonArrayString));
        };
        CreateItems();*/
    }
    public void ShowUserItems()
    {
        //StartCoroutine(GetItem(DBManager.id));
        ClearInventorySlots();
        inventory.SetActive(true);
        _createItemsCallback = (jsonArrayString) =>
        {
            StartCoroutine(CreateItemsRoutine(jsonArrayString));
        };
        CreateItems();
    }
    public void CloseInventory()
    {
        //StartCoroutine(GetItem(DBManager.id));
        inventory.SetActive(false);
    }
    public void CloseShop()
    {
        //StartCoroutine(GetItem(DBManager.id));
        shop.SetActive(false);
    }
    IEnumerator GetUsers()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/sqlconnect/getuser.php"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                byte[] results = www.downloadHandler.data;
            }
        }
    }
    //get userId from UserInfo.cs
    public IEnumerator GetItemIDs(string userID, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/getItemId.php", form))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;

                callback(jsonArray);
            }
        }
    }

/*    public IEnumerator GetItem(string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/getitem.php", form))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string itemsDataString = www.downloadHandler.text;
                items = itemsDataString.Split(';');
                Debug.Log(GetDataValue(items[0], "description:"));
                //DBManager.items
                //callback(jsonArray);
                

            }
        }
    }*/

    public IEnumerator GetItems(string id, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/getitem.php", form))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;
                callback(jsonArray);
            }
        }
    }

    public IEnumerator GetShopItems(string id, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/getShopItem.php", form))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;
                callback(jsonArray);
            }
        }
    }

    public IEnumerator GetShopItemIDs(System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/sqlconnect/getShopItemId.php"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;

                callback(jsonArray);
            }
        }
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index)+index.Length);
        if(value.Contains("|"))value = value.Remove(value.IndexOf("|"));
        return value;
    }

    public IEnumerator SellItem(string id, string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("userID", userID);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/sellitem.php", form))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;
            }
        }
    }

    public IEnumerator GetItemIcon(string id, System.Action<Sprite> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/getItemIcon.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                byte[] bytes = www.downloadHandler.data;

                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);

                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                callback(sprite);

            }
        }
    }

    //function to add item when users earn it in game
    public IEnumerator AddItems(string id, string itemID)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("itemID", itemID);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/addItemId.php", form))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator ItemQuantity(string itemID, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/ItemQuantity.php", form))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string jsonArray = www.downloadHandler.text;
                callback(jsonArray);
            }
        }
    }


    public IEnumerator UseItem(string id, string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("userID", userID);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/useitem.php", form))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;
            }
        }
    }

    public IEnumerator BuyItem(string id, string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("userID", userID);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/buyitem.php", form))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;
            }
        }
    }


    public void ClearInventorySlots()
    {
        for (int i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            Destroy(inventoryPanel.transform.GetChild(i).gameObject);
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
        //Debug.Log(jsonArray);

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
                Debug.Log(itemInfoJson);
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
                newSlot.Setup(newItem, thisInventoryManager);
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
