using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance;

    public Web Web;
    public UserInfo UserInfo;
    public InventoryManager InventoryManager;

    void Start()
    {
        Instance = this;
        Web = GetComponent<Web>();
        UserInfo = GetComponent<UserInfo>();
        InventoryManager = GetComponent<InventoryManager>();
    }
}
