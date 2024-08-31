using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Treasure
{
    public GameObject prefab;
    public int amount;
}

[SelectionBase]
public class ChestOfTreasures : MonoBehaviour
{
    public Treasure[] treasureArray;
    public Stack<Treasure> treasures;
    public bool isVomiting;
    Treasure toVomit = null;
    public Transform[] emitters;
    public float vomitForce;

    //public TimeManager timeManager;

    // Start is called before the first frame update
    void Start()
    {
        treasures = new Stack<Treasure>();
        for (int oi = 0; oi < treasureArray.Length; oi++)
        {
            treasures.Push(treasureArray[oi]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isVomiting)
        {
            return;
        }

        for(int i = 0; i < emitters.Length; i++)
        {
            if(toVomit == null || toVomit.amount <= 0)
            {
                Debug.Log(treasures);
                if(treasures.Count == 0)
                {
                    return;
                }
                toVomit = treasures.Pop();
                /*if(toVomit == null)
                {
                    return;
                }*/
            }

            GameObject vomited = Instantiate(toVomit.prefab, emitters[i].position, emitters[i].rotation);
            Rigidbody2D vrb = vomited.GetComponent<Rigidbody2D>();

            if(vrb != null)
            {
                vrb.transform.Rotate(0, 0, UnityEngine.Random.Range(-15, 15));
                vrb.velocity = vomited.transform.up * (vomitForce + UnityEngine.Random.Range(-2, 2));
            }

            toVomit.amount--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "playerOuchie" || collision.tag == "playerProjectile")
        {
            isVomiting = true;
            //timeManager.Slowmo();
        }
    }
}
