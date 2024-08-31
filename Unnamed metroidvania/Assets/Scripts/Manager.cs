using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public int scene;
    public PlayerController player;
    public int health;
    public float energy;
    public Vector3 coords = new Vector3 (0, 0, 0);
    public int maxHealth;
    public float maxEnergy;
    public int flasks;
    public int maxFlasks;

    public int benchNumber;
    public int lastSavedScene;

    public Vector3 spawnPoint;

    public static Manager instance;

    public SavePoint[] savePoints;

    public bool transitionedByDying;

    private void Awake()
    {
        

        player = FindObjectOfType<PlayerController>();
        if (instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(FindObjectsOfType<Manager>().Length > 1 && FindObjectsOfType<Manager>()[0] != this)
        {
            Destroy(gameObject);
        }

        SaveData.LoadAllData();
        SaveData.ReadPlayerData(player, this);
        SaveData.OffloadDataToManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            player = FindObjectOfType<PlayerController>();
            scene = player.level_number;

            if (maxHealth <= 0)
            {
                maxHealth = 100;
            }

            if (maxEnergy <= 0)
            {
                maxEnergy = 100;
            }

            if (maxFlasks <= 0)
            {
                maxFlasks = 1;
            }
        }
        catch
        {
            
        }
    }

    public void Spawn(int sceneNumber, Vector3 coordinates)
    {
        //Debug.Log("asdofigujao");
        //Debug.Log(sceneNumber);
        //Debug.Log(coordinates);
        //Debug.Log("a");
        coords = coordinates;
        SceneManager.LoadScene(sceneNumber);
        //Debug.Log("b");
        player = FindObjectOfType<PlayerController>();
        //Debug.Log("c");
        //player.gameObject.transform.position = coords;
        //Debug.Log("d");
    }

    public void PlayerDeath()
    {
        Debug.Log("skkkkkkkkkkkkkk");
        SceneManager.LoadScene(lastSavedScene);
        Debug.Log(lastSavedScene);
        SaveData.LoadAllData();
        savePoints = FindObjectsOfType<SavePoint>();
        Debug.Log(savePoints);
        Debug.Log(savePoints.Length);
        foreach(SavePoint savePoint in savePoints)
        {
            Debug.Log("viiaiibkkk");
            if(savePoint.bench_number == benchNumber)
            {
                Debug.Log("kiwfiivi");
                player.transform.position = savePoint.transform.position;
                Debug.Log(player.transform.position);
                Debug.Log(savePoint.transform.position);
                //spawnPoint = savePoint.transform.position;
                //break;
            }
        }
    }

    public void FirstLoad()
    {
        SceneManager.LoadScene(scene);
        Debug.Log(scene);
        player = FindObjectOfType<PlayerController>();
        SaveData.LoadAllData();
    }
}
