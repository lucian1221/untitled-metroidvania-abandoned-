using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaTrigger : MonoBehaviour
{
    public GameObject barriers;
    public SpawnCage arena;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (arena.done)
        {
            barriers.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !arena.running)
        {
            arena.BeginArena();
            barriers.SetActive(true);
        }
    }
}
