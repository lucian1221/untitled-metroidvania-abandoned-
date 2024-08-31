using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardController : MonoBehaviour
{
    public int damage;
    public float knockbackForce;
    public bool backToSafeGround;
    public bool oneTime;
    public float duration;

    private void Awake()
    {
        if (oneTime)
        {
            Invoke("DestroySelf", duration);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            FindObjectOfType<PlayerController>().TakeDamage(damage, Vector2.up * knockbackForce);

            if (backToSafeGround)
            {
                FindObjectOfType<PlayerController>().EngageReturnToSafeGround();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("sodifjghsolaidfujhg");

        if (collision.tag == "Player")
        {
            FindObjectOfType<PlayerController>().TakeDamage(damage, Vector2.up * knockbackForce);

            if (backToSafeGround)
            {
                FindObjectOfType<PlayerController>().EngageReturnToSafeGround();
            }
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}

//
