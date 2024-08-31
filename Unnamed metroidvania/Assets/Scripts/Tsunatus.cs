using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Tsunatus : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public PlayerController player;

    public int health;
    public int maxHealth;

    public GameObject head;
    public TsunatusHead tsunatusHead;

    public int contactDamage;

    public int currentState;
    //private Coroutine[] attacks;
    public float attackDelay;
    public float phase2attackDelay;

    public Transform projectileSpawnCenter;
    public int shockBallNumber;
    public GameObject shockball;
    public float shockBallXoffset;
    public float shockballYoffset;

    public Transform[] lightningSpawns;
    public GameObject lightning;
    public float lightningSpawnOffset;

    public Transform[] uumuuSpawns1;
    public Transform[] uumuuSpawns2;
    public Transform[] uumuuSpawns3;
    public float uumuuOffsetTime;
    public GameObject uumuuBall;

    public GameObject deadlyLazer;
    public GameObject actualDeadlyLazer;
    public float deadlyLazerChargeTime;

    public SpriteShapeRenderer sprite;
    public Color originalColor;
    public Color targetColor;

    // Start is called before the first frame update

    private void Awake()
    {
        originalColor = sprite.color;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        deadlyLazer.SetActive(false);
        StartCoroutine("StateMachine");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StateMachine()
    {
        while (true)
        {
            currentState = Random.Range(0, 4);

            Debug.Log(currentState);

            if(currentState == 0)
            {
                StartCoroutine("BallLightning");
            }
            if(currentState == 1)
            {
                StartCoroutine("LightningPillars");
            }
            if(currentState == 2)
            {
                int index = Random.Range(1, 4);

                if(index == 1)
                {
                    StartCoroutine("UumuuAttack", uumuuSpawns1);
                }
                if (index == 2)
                {
                    StartCoroutine("UumuuAttack", uumuuSpawns2);
                }
                if (index == 3)
                {
                    StartCoroutine("UumuuAttack", uumuuSpawns3);
                }
            }
            if(currentState == 3)
            {
                deadlyLazer.SetActive(true);
                Invoke("DeadlyLazer", 2);
            }

            if(tsunatusHead.health > 0.5 * tsunatusHead.maxHealth)
            {
                //Debug.Log("a");
                yield return new WaitForSeconds(attackDelay);
            }
            else if (tsunatusHead.health <= 0.5 * tsunatusHead.maxHealth)
            {
                //Debug.Log("b");
                yield return new WaitForSeconds(phase2attackDelay);
            }
            else
            {
                Debug.Log("bro something has gone extremely wrong and you need to fix your math");
            }
        }
    }

    IEnumerator BallLightning()
    {
        //Debug.Log("A");
        for(int i = 0; i < shockBallNumber; i++)
        {
            //Debug.Log("B");
            shockBallXoffset = Random.Range(-5, 5);
            shockballYoffset = Random.Range(-2, 2);
            Vector3 offset = new Vector2(shockBallXoffset, shockballYoffset);
            Instantiate(shockball, projectileSpawnCenter.position + offset, Quaternion.identity);
            //Debug.Log("C");
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator LightningPillars()
    {
        for(int i = 0; i < lightningSpawns.Length; i++)
        {
            Instantiate(lightning, new Vector2(lightningSpawns[i].position.x + Random.Range(-lightningSpawnOffset, lightningSpawnOffset), lightningSpawns[i].position.y), Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator UumuuAttack(Transform[] uumuuSpawns)
    {
        for(int i = 0; i < uumuuSpawns.Length; i++)
        {
            Instantiate(uumuuBall, uumuuSpawns[i].position, Quaternion.identity);
            yield return new WaitForSeconds(uumuuOffsetTime);
        }
    }

    void DeadlyLazer()
    {
        Instantiate(actualDeadlyLazer, deadlyLazer.transform.position, deadlyLazer.transform.rotation);
        deadlyLazer.SetActive(false);
    }

    public void ChangeColor()
    {
        sprite.color = targetColor;
        Invoke("UnchangeColor", 0.2f);
    }

    void UnchangeColor()
    {
        sprite.color = originalColor;
    }
}
