using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]

public class Mechanisms : MonoBehaviour
{
    public bool enemyCanActivate;
    public UnityEvent toRun;
    public bool ran;

    // Start is called before the first frame update
    void Start()
    {
        ran = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((collision.collider.tag == "Player" ||
           collision.collider.tag == "playerOuchie" ||
           collision.collider.tag == "playerProjectile" ||
           (collision.collider.tag == "badBoy" && enemyCanActivate))
           && !ran
          )
        {
            transform.localScale = new Vector2(transform.localScale.x / 2, transform.localScale.y);
            toRun.Invoke();
            ran = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.tag == "Player" ||
           collision.tag == "playerOuchie" ||
           collision.tag == "playerProjectile" ||
           (collision.tag == "badBoy" && enemyCanActivate))
           && !ran
          )
        {
            transform.localScale = new Vector2(transform.localScale.x / 2, transform.localScale.y);
            toRun.Invoke();
            ran = true;
        }
    }

}
