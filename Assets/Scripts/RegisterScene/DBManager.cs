using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DBManager
{
    public static string username;
    public static string id;
    public static int level;
    public static int coins;
    public static string[] items;

    public static bool LoggedIn
    {
        get
        {
            return username != null;
        }
    }

    public static void LogOut()
    {
        username = null;
    }
}
