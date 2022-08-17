using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour {

    public float thrust;
    public float knockTime;
    public float damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Pot>().Smash();
        }
        if (other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if(hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);
                if (other.gameObject.CompareTag("enemy") && other.isTrigger)
                {
                    if (hit.GetComponent<Enemy>().enemyHealth <=1 && hit.GetComponent<Enemy>().enemyHealth >0){
                        hit.GetComponent<Enemy>().enemyState = EnemyState.stagger;
                    }
                    other.GetComponent<Enemy>().Knock(hit, knockTime, damage);
                }
                if(other.gameObject.CompareTag("Player"))
                {
                    if (other.GetComponent<PlayerMovement>().currentState != PlayerState.stagger)
                    {
                        hit.GetComponent<PlayerMovement>().currentState = PlayerState.stagger;
                        other.GetComponent<PlayerMovement>().Knock(knockTime, damage);
                    }
                }
            }
        }
    }
    // private void OnCollisionStay2D(Collision other)
    //     {
    //         if (other.gameObject.tag == "Player")
    //         {
                
    //         }
    //     }

//      public float force = 5;
//     public ForceMode2D forceMode = ForceMode2D.Impulse;
//     void OnCollisionEnter2D(Collision2D collision)
//  {
//      if (collision.gameObject.CompareTag("enemy"))
//     {
//         Debug.Log("Collision");
        
//             // Calculate Angle Between the collision point and the player
//             ContactPoint2D contactPoint = collision.GetContact(0);
//             Vector2 playerPosition = transform.position;
//             Vector2 dir = contactPoint.point - playerPosition;
//             Debug.Log(dir);

//             dir = -dir.normalized;

//             GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
//             GetComponent<Rigidbody2D>().inertia = 0;
//             GetComponent<Rigidbody2D>().AddForce(dir * force, forceMode);
        
//     }
//  }

}