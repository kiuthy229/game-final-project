using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{
    public InputField nameField;
    public InputField passwordField;

    public Button submitButton;
    
    public void CallLogin()
    {
        StartCoroutine(Signin());
    }

    IEnumerator Signin()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);
        WWW www = new WWW("http://localhost/sqlconnect/login.php", form);
        yield return www;
        
        using (UnityWebRequest www1 = UnityWebRequest.Post("http://localhost/sqlconnect/getLoginInfo.php", form))
        {
            yield return www1.SendWebRequest();

            if (www1.isNetworkError || www1.isHttpError)
            {
                Debug.Log(www1.error);
            }
            else
            {
                Main.Instance.UserInfo.SetID(www1.downloadHandler.text);
                Debug.Log(www1.downloadHandler.text);
            }
        }
        if (www.text[0] == '0')
        {
            DBManager.username = nameField.text;
/*            DBManager.level = int.Parse(www.text.Split('\t')[1]);*/
            DBManager.coins = int.Parse(www.text.Split('\t')[1]);

            UnityEngine.SceneManagement.SceneManager.LoadScene(3);


        }
        else
        {
            Debug.Log("User login failed" + www.text);
        }
    }


    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 4 && passwordField.text.Length >= 4);
    }
}
