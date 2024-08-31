using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SavePoint : MonoBehaviour
{
    public int bench_number;
    public Manager manager;
    public TMP_Text flavorText;
    public float textFadeSpeed;
    public Animator animator;

    public int sceneNumber;

    public bool activated;

    private void Awake()
    {
        sceneNumber = SceneManager.GetActiveScene().buildIndex;
    }

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<Manager>();
        flavorText.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(flavorText.alpha > 0)
        {
            flavorText.alpha -= textFadeSpeed * Time.deltaTime;
        }
        else if (flavorText.alpha <= 0)
        {
            flavorText.alpha = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerController>() != null)
        {
            //Debug.Log("can save");

            if (flavorText.alpha < 1)
            {
                flavorText.alpha += 2 * textFadeSpeed * Time.deltaTime;
            }
            else if(flavorText.alpha >= 1)
            {
                flavorText.alpha = 1;
            }

            if (Input.GetKeyDown(KeyCode.E) && collision.GetComponent<PlayerController>().isGrounded)
            {
                PlayerController player = FindObjectOfType<PlayerController>();
                SaveData.SaveAllData(player);
                player.UpdateSafePoint(player.transform.position);
                player.currentHealth = player.maxHealth;
                player.lastBench = gameObject.transform.position;
                Debug.Log("Data saved");
                animator.SetTrigger("player saves");
                manager.benchNumber = bench_number;
                manager.lastSavedScene = sceneNumber;
                manager.spawnPoint = transform.position;
                Debug.Log(transform.position);
                Debug.Log(sceneNumber);
                Debug.Log(manager.lastSavedScene);
            }

        }
    }
}
