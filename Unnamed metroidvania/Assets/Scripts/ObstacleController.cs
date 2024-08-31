using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public GameObject player;

    public Direction direction;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        player = FindObjectOfType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(direction == Direction.Left && player.transform.position.x > transform.position.x)
        {
            return;
        }
        else if(direction == Direction.Right && player.transform.position.x < transform.position.x)
        {
            return;
        }
        else if (direction == Direction.Top && player.transform.position.y < transform.position.y)
        {
            return;
        }
        else if (direction == Direction.Bottom && player.transform.position.y > transform.position.y)
        {
            return;
        }

        if (collision.tag == "playerOuchie")
        {
            health -= player.GetComponent<PlayerController>().meleeDamage;
        }

        if (collision.tag == "playerProjectile")
        {
            health -= collision.attachedRigidbody.GetComponent<SpellProjectile>().spellData.damage;
        }
    }
}
