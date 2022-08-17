using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBubble : MonoBehaviour
{
    // public Image background;
    // public Image icon;
    public Text dialogText;
    public string dialog;


    void Start(){
        // background=transform.Find("Background").GetComponent<Image>();
        // icon = transform.Find("Icon").GetComponent<Image>();
        dialogText = transform.Find("Text").GetComponent<Text>();
        // SetUp(dialog);
    }
    void Update(){
        dialogText.text = dialog;
    }

    // private void SetUp (string text){
    //     dialogText.text= dialog;
        
    //     Vector2 textSize = dialogText.ge
    // }
}
