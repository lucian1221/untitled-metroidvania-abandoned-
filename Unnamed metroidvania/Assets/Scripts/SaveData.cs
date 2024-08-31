using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SaveData
{
    public int currentHealth;
    public int maxHealth;
    public float maxEnergy;
    public bool canDash;
    public bool canWallJump;
    public bool canParaglide;
    public int jumpMax;
    public List<SpellData> eruditeSpells;
    public int wealth;

    public int bench;
    public int level;
    public int scene;

    public Vector3 position;


    private static SaveData singleton = new SaveData();
    public bool saveExists;
    public Manager manager;

    private SaveData()
    {
        singleton = this;
    }

    public static void LoadAllData()
    {
        if(!System.IO.File.Exists(Application.persistentDataPath + "/SaveData.json"))
        {
            return;
        }

        string jason = System.IO.File.ReadAllText(Application.persistentDataPath + "/SaveData.json");
        singleton = JsonUtility.FromJson<SaveData>(jason);
        singleton.saveExists = true;
    }

    public static void ReadPlayerData(PlayerController player, Manager manager)
    {
        if (!singleton.saveExists)
        {
            player.currentHealth = player.maxHealth;
            player.currentEnergy = 0;
            Debug.Log("New game");
            return;
        }
        else
        {
            Debug.Log("Loading player data");
        }

        player.currentHealth = singleton.currentHealth;
        player.maxHealth = singleton.maxHealth;
        player.maxEnergy = singleton.maxEnergy;
        PlayerController.canDash = singleton.canDash;
        PlayerController.canWallJump = singleton.canWallJump;
        PlayerController.canParaglide = singleton.canParaglide;
        PlayerController.jumpMax = singleton.jumpMax;
        //player.transform.position = singleton.position;
        manager.spawnPoint = singleton.position;
        manager.lastSavedScene = singleton.scene;
        PlayerController.eruditeSpells = singleton.eruditeSpells;
        player.wealth = singleton.wealth;
        //SceneManager.LoadScene(singleton.scene);
    }

    private void SavePlayerData(PlayerController player)
    {
        currentHealth = player.currentHealth;
        maxHealth = player.maxHealth;
        maxEnergy = player.maxEnergy;
        canDash = PlayerController.canDash;
        canWallJump = PlayerController.canWallJump;
        canParaglide = PlayerController.canParaglide;
        jumpMax = PlayerController.jumpMax;
        position = player.transform.position;
        eruditeSpells = PlayerController.eruditeSpells;
        wealth = player.wealth;
        scene = player.level_number;
    }

    public static void SaveAllData(PlayerController player)
    {
        singleton.SavePlayerData(player);
        string jason = JsonUtility.ToJson(singleton, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/SaveData.json", jason);
    }

    public static void OffloadDataToManager(Manager manager)
    {
        manager.scene = singleton.scene;
        manager.health = singleton.currentHealth;
        manager.maxHealth = singleton.maxHealth;
        manager.maxEnergy = singleton.maxEnergy;
        manager.spawnPoint = singleton.position;
    }
}
