using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public Text playerDisplay;
    public Text scoreDisplay;

    private void Awake()
    {
        if (DBManager.username == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        scoreDisplay.text = "Level: " + DBManager.level;
    }

    /*    public void CallSaveData()
        {
            StartCoroutine(SavePlayerData());
        }

        IEnumerator SavePlayerData()
        {
            WWWForm form = new WWWForm();
            form.AddField("name", DBManager.username);
            form.AddField("level", DBManager.level);
            form.AddField("coins", DBManager.coins);

            WWW www = new WWW("http://localhost/sqlconnect/savedata.php", form);
            yield return www;
            if (www.text == "0")
            {
                Debug.Log("Game Saved.");
            }
            else
            {
                Debug.Log("SAVE FAILED." + www.text);
            }
            DBManager.LogOut();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }*/

    public void IncreaseScore()
    {
            DBManager.level++;
            scoreDisplay.text = "Level: " + DBManager.level;
    }
}
