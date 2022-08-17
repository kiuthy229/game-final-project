using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponseObject : MonoBehaviour
{
    [SerializeField] private Text myText;

    private int choiceValue;
    public void SetUp(string newDialog, int myChoice)
    {
        myText.text = newDialog;
        choiceValue = myChoice;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
