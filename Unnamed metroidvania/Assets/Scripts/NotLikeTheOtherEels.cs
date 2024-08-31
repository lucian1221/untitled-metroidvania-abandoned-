using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotLikeTheOtherEels : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public PlayerController player;
    public BaddieController self;
    public Transform location;
    public ParticleSystem particle;
    private float distance;
    public int shockDamage;
    public float shockback;
    public float range;
    public float ACPeriod;
    public bool on;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        location = player.gameObject.transform;
        particle.transform.SetParent(null);

        StartCoroutine("ShockyShocky", ACPeriod);
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(location.position, gameObject.transform.position);

        if(self.health <= 0)
        {
            particle.gameObject.SetActive(false);
        }
    }

    public IEnumerator ShockyShocky(float period)
    {
        while (true)
        {
            if (on)
            {
                if(player.wet && distance <= range)
                {
                    player.TakeDamage(shockDamage, new Vector2(0, shockback));
                }

                particle.gameObject.SetActive(true);
                Invoke("Wait", period);
                yield return new WaitForSeconds(period);
            }
            else
            {
                particle.gameObject.SetActive(false);
                Invoke("Wait", period);
                yield return new WaitForSeconds(period);
            }
        }
    }

    public void Wait()
    {
        if(on == true)
        {
            on = false;
        }
        else if(on == false)
        {
            on = true;
        }
    }
}
