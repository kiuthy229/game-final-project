using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : Interactable
{
     public GameObject dialogBox;
    public Text dialogText;
    public string dialog;

	void Start () {
		
	}
	
	void Update () {
        if(Input.GetKeyDown(KeyCode.X) && playerInRange)
        {
            if(dialogBox.activeInHierarchy)
            {
                dialogBox.SetActive(false);
            }else{
                dialogBox.SetActive(true);
                dialogText.text = dialog;
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            Debug.Log("player in range");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            context.Raise();
            playerInRange = false;
            Debug.Log("player out of range");
            dialogBox.SetActive(false);
        }
    }
}
