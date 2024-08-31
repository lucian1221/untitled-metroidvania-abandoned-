using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegraphAnything : MonoBehaviour
{
    public GameObject attack;
    public float attackDelay;

    // Start is called before the first frame update
    void Awake()
    {
        Invoke("Attack", attackDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Attack()
    {
        Instantiate(attack, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
