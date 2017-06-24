using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public GameObject player;
    private PlayerController playerController;
    Slider healthBar;

    public int currentHealth;
    public int maxHealth;
    public float currentPercentHealth;

	// Use this for initialization
	void Start () {
        playerController = player.GetComponent<PlayerController>();
        healthBar = gameObject.GetComponent<Slider>();
        SetHealth();

        healthBar.value = CalculateHealth();
	}
	

	// Update is called once per frame
	public void UpdateHealth()
    {
        SetHealth();
        healthBar.value = CalculateHealth();
    }

    void SetHealth()
    {
        currentHealth = playerController.Hp;
        maxHealth = playerController.MaxHp;
    }

    float CalculateHealth()
    {
        currentPercentHealth = (float)currentHealth / (float)maxHealth;
        return currentPercentHealth;
    }
}
