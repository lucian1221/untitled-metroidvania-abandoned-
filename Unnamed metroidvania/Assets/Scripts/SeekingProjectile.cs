using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingProjectile : MonoBehaviour
{
    public Transform target;
    public Rigidbody2D rb;
    public float speed;
    public int damage;
    public float lifetime;
    public float knockback;
    public float detectionRadius;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Invoke("DeleteProjectile", lifetime);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, target.transform.position);
        if(distance <= detectionRadius)
        {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            transform.position += direction * speed * Time.fixedDeltaTime;
        }
    }

    private void DeleteProjectile()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController isPlayer = collision.collider.GetComponent<PlayerController>();

        if (isPlayer != null)
        {
            Vector2 distance = isPlayer.transform.position - transform.position;
            isPlayer.TakeDamage(damage, distance.normalized * knockback);
        }

        Debug.Log("slfdrkigjuhbask");

        Destroy(gameObject);
    }
}
