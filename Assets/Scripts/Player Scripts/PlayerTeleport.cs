using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public GameObject teleportPosition;
    void Start()
    {
        
    }



    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            other.transform.position = new Vector3(teleportPosition.transform.position.x, teleportPosition.transform.position.y, 0);
        }
    }

}
