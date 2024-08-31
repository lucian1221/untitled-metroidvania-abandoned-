using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class BaddieController : MonoBehaviour
{
    public float speed;
    public int health;
    public int maxHealth;
    public float knockback;
    public float acceleration;
    public float invincibilityTimer;
    public float invincibilityDuration;
    public float los;

    public int headbangStrength;
    public float knockbackForce;

    public Rigidbody2D rigidbody;
    public GameObject player;

    public Vector3 phonyForce;
    protected Vector3 forceReduction;
    public float knockback_y_correction_constant;

    public AIType AI;

    public Vector3 maxie;
    public Vector3 minnie;
    public bool isTravellingRight;

    public float shootyTimer;
    public GameObject projectile;
    public float shootyInterval;
    public float bulletV;
    public float bulletLifetime;
    public Vector2 bulletSpawnPos;
    public Transform shootPoint;

    public Transform target;
    public GameObject[] movePoints;
    private int movePointIndex;
    public float movePointWaitTime;

    public Color targetColor;
    public Color originalColor;
    [SerializeField] private SpriteRenderer sprite;

    private void Awake()
    {
        //sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;

        if(AI == AIType.FlyingTurret)
        {
            StartCoroutine("Navigate");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        if(AI == AIType.Patrol)
        {
            maxie = transform.Find("Maxie").position;
            Destroy(transform.Find("Maxie").gameObject);

            minnie = transform.Find("Minnie").position;
            Destroy(transform.Find("Minnie").gameObject);
        }

        if(AI == AIType.FlyingTurret)
        {
            foreach (GameObject movePoint in movePoints)
            {
                movePoint.transform.SetParent(null);
            }
        }

        player = FindObjectOfType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(AI == AIType.Chase)
        {
            Vector2 distance = player.transform.position + Vector3.up - transform.position;
            Vector2 direction = distance.normalized;

            if(distance.magnitude < los)
            {
                rigidbody.AddForce(direction * acceleration);
                rigidbody.velocity = new Vector3(Mathf.Clamp(rigidbody.velocity.x, -speed, speed), rigidbody.velocity.y) + phonyForce;
            }
            //rigidbody.velocity = direction * speed + Vector2.up * rigidbody.velocity.y;

            if (player.transform.position.x > transform.position.x)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        else if (AI == AIType.Patrol)
        {
            //Debug.Log(rigidbody.velocity);

            if (isTravellingRight)
            {
                transform.localScale = new Vector3(-1, 1, 1);

                rigidbody.AddForce(Vector3.right * acceleration);
                rigidbody.velocity = new Vector3(Mathf.Clamp(rigidbody.velocity.x, -speed, speed), rigidbody.velocity.y) + phonyForce;

                if(transform.position.x > maxie.x)
                {
                    /*rigidbody.AddForce(Vector3.left * acceleration);
                    rigidbody.velocity = new Vector3(Mathf.Clamp(rigidbody.velocity.x, -speed, speed), rigidbody.velocity.y) + phonyForce;*/
                    isTravellingRight = false;
                }
            }

            else
            {
                transform.localScale = Vector3.one;

                rigidbody.AddForce(Vector3.left * acceleration);
                rigidbody.velocity = new Vector3(Mathf.Clamp(rigidbody.velocity.x, -speed, speed), rigidbody.velocity.y) + phonyForce;

                if(transform.position.x < minnie.x)
                {
                    /*rigidbody.AddForce(Vector3.right * acceleration);
                    rigidbody.velocity = new Vector3(Mathf.Clamp(rigidbody.velocity.x, -speed, speed), rigidbody.velocity.y) + phonyForce;*/
                    isTravellingRight = true;
                }
            }
        }

        else if (AI == AIType.Turret)
        {
            if (Time.realtimeSinceStartup >= shootyTimer)
            {
                FireProjectile();
                shootyTimer = Time.realtimeSinceStartup + shootyInterval;
            }
        }

        else if(AI == AIType.Bullet)
        {
            if(invincibilityTimer >= bulletLifetime)
            {
                Destroy(gameObject);
            }
        }

        else if(AI == AIType.FlyingTurret)
        {
            Vector2 distance = player.transform.position + Vector3.up - transform.position;
            Vector2 direction = distance.normalized;

            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            //Debug.Log(transform.position);

            if (Time.realtimeSinceStartup >= shootyTimer && distance.magnitude < los)
            {
                FireProjectile();
                shootyTimer = Time.realtimeSinceStartup + shootyInterval;
            }
        }

        if(health <= 0)
        {
            Destroy(gameObject);

            if(AI != AIType.Bullet)
            {
                player.GetComponent<PlayerController>().killstreak += 1;
            }
        }

        phonyForce = Vector3.SmoothDamp(phonyForce, Vector3.zero, ref forceReduction, 0f);

        invincibilityTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if(AI == AIType.FlyingTurret)
        {
            Vector2 aimDirection = new Vector2(player.transform.position.x, player.transform.position.y) - rigidbody.position;
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
            rigidbody.rotation = aimAngle;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(invincibilityTimer >= invincibilityDuration && AI != AIType.Bullet)
        {
            if (collision.tag == "playerOuchie")
            {
                health -= player.GetComponent<PlayerController>().meleeDamage;

                //Debug.Log($"Player: {player.transform.position}\nEnemy: {transform.position}");

                Vector2 distance = player.transform.position - transform.position;
                Vector2 direction = distance.normalized;

                //rigidbody.AddForce(direction * knockback, ForceMode2D.Impulse);
                phonyForce = new Vector2(direction.x * -knockback, direction.y * -knockback / knockback_y_correction_constant);
                invincibilityTimer = 0;
                //Debug.Log(phonyForce);

                if(AI != AIType.Bullet)
                {
                    ChangeColor();
                }
            }

            if (collision.tag == "playerProjectile")
            {
                health -= collision.attachedRigidbody.GetComponent<SpellProjectile>().spellData.damage;

                Vector2 distance = collision.transform.position - transform.position;
                Vector2 direction = distance.normalized;

                phonyForce = direction * -knockback;
                invincibilityTimer = 0;

                //Destroy(collision.gameObject);
                if (AI != AIType.Bullet)
                {
                    ChangeColor();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController isPlayer = collision.collider.GetComponent<PlayerController>();

        if(isPlayer != null)
        {
            Vector2 distance = isPlayer.transform.position - transform.position;
            isPlayer.TakeDamage(headbangStrength, distance.normalized * knockbackForce);
        }

        if(AI == AIType.Bullet)
        {
            Destroy(gameObject);
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

    void FireProjectile()
    {
        GameObject P = Instantiate(projectile, shootPoint.position, shootPoint.rotation);
        //Debug.Log(shootPoint.position);
        //P.transform.right = transform.up;
        //P.transform.position = new Vector3(transform.localPosition.x + bulletSpawnPos.x, transform.localPosition.y + bulletSpawnPos.y, 0);
        P.GetComponent<Rigidbody2D>().AddForce(-shootPoint.up * bulletV, ForceMode2D.Impulse);
    }

    IEnumerator Navigate()
    {
        while (true)
        {
            movePointIndex = Random.Range(0, movePoints.Length - 1);
            target = movePoints[movePointIndex].transform;
            yield return new WaitForSeconds(movePointWaitTime);
        }
    }
}
