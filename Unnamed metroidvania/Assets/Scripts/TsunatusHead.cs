using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TsunatusHead : MonoBehaviour
{
    public Tsunatus realBoss;
    public GameObject player;
    [SerializeField] private SpriteRenderer sprite;
    public int health;
    public int maxHealth;
    public Color targetColor;
    public Color originalColor;
    public int contactDamage;
    public float knockbackForce;

    private void Awake()
    {
        originalColor = sprite.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            Vector2 distance = player.transform.position + Vector3.up - transform.position;
            Vector2 direction = distance.normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + 90));
        }

        if(health <= 0)
        {
            Destroy(realBoss.gameObject);
            Destroy(gameObject);
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "playerOuchie")
        {
            realBoss.health -= player.GetComponent<PlayerController>().meleeDamage;
        }

        if (collision.tag == "playerProjectile")
        {
            realBoss.health -= collision.attachedRigidbody.GetComponent<SpellProjectile>().spellData.damage;
        }
        
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "playerOuchie")
        {
            health -= player.GetComponent<PlayerController>().meleeDamage;

            ChangeColor();
            realBoss.ChangeColor();
        }

        if (collision.tag == "playerProjectile")
        {
            health -= collision.attachedRigidbody.GetComponent<SpellProjectile>().spellData.damage;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController isPlayer = collision.collider.GetComponent<PlayerController>();

        if (isPlayer != null)
        {
            Vector2 distance = isPlayer.transform.position - transform.position;
            isPlayer.TakeDamage(contactDamage, distance.normalized * knockbackForce);
        }
    }

    void ChangeColor()
    {
        sprite.color = targetColor;
        Invoke("UnchangeColor", 0.2f);
    }

    void UnchangeColor()
    {
        sprite.color = originalColor;
    }
}
