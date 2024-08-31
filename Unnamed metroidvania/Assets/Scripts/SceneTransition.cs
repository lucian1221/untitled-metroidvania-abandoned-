using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public int sceneNumber;
    public Vector3 coords;
    public BlackRectangle blackRectangle;
    public PlayerController player;
    public Manager manager;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        manager = FindObjectOfType<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        manager = FindObjectOfType<Manager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            manager.transitionedByDying = false;
            blackRectangle.Fade();
            manager.health = player.currentHealth;
            manager.energy = player.currentEnergy;
            manager.Spawn(sceneNumber, coords);
            /*SceneManager.LoadScene(sceneNumber); 
            player = FindObjectOfType<PlayerController>();
            player.transform.position = coords;
            
            blackRectangle.Unfade();*/
        }
    }
}
