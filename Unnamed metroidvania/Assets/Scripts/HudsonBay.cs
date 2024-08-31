using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudsonBay : MonoBehaviour
{
    public PlayerController player;
    public Slider healthSlider;
    public Slider energyMeter;
    public TextMeshProUGUI coinCount;
    private GameObject playerObj;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<PlayerController>();
        healthSlider.maxValue = player.maxHealth;
        energyMeter.maxValue = player.maxEnergy;
        coinCount.text = player.wealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = player.currentHealth;
        energyMeter.value = player.currentEnergy;
        coinCount.text = player.wealth.ToString();
    }
}
