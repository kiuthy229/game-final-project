using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    public string UserID { get; private set; }
    string Username;
    string UserPassword;
    string Level;
    string Coins;

    public void SetInfo(string username)
    {
        Username = username;
    }
    public void SetID(string id)
    {
        UserID = id;
        DBManager.id = id;
    }
}
