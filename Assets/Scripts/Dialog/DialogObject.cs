using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogObject : MonoBehaviour
{
    [SerializeField] private Text myText;
    public void SetUp(string newDialog)
    {
        myText.text = newDialog;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
