using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogNPC : Interactable
{
    [SerializeField] private TextAssetValue dialogValue;

    [SerializeField] private TextAsset myDialog;

    [SerializeField] private Notification branchingDialogNotification;

    void Start()
    {

    }

    void Update()
    {
        if (playerInRange)
        {
            if (Input.GetButtonDown("Check"))
            {
                dialogValue.value = myDialog;
                branchingDialogNotification.Raise();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            context.Raise();
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            context.Raise();
            playerInRange = false;
            Debug.Log("player out of range");
        }
    }
}
