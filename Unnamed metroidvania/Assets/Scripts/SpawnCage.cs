using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCage : MonoBehaviour
{
    public Transform[] spawnpoints;
    public GameObject[] enemies;

    private int i;
    //[SerializeField] private int enemyNumber;
    public bool canSpawn = false;
    public float spawnTime;

    public bool running;
    public bool done;

    private void Awake()
    {
        //StartCoroutine("SpawnEnemies");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BeginArena()
    {
        canSpawn = true;
        StartCoroutine("SpawnEnemies");
    }

    public IEnumerator SpawnEnemies()
    {
        for(int enemy = 0; enemy < enemies.Length; enemy++)
        {
            running = true;
            i = Random.Range(0, spawnpoints.Length);
            Instantiate(enemies[enemy], spawnpoints[i].position, Quaternion.identity);
            if(enemy == enemies.Length - 1)
            {
                done = true;
                canSpawn = false;
                StopCoroutine("SpawnEnemies");
            }
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
