using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : Interactable
{
    [SerializeField] private GameObject shopPanel;

    void Start()
    {

    }

    void Update()
    {
        if (playerInRange)
        {
            if (Input.GetButtonDown("Check"))
            {
                shopPanel.SetActive(true);
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
