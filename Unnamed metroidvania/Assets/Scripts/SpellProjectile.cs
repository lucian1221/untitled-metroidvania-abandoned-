using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public SpellData spellData;

    private void Start()
    {
        Destroy(gameObject, spellData.lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            Destroy(gameObject);
        }
    }
}
