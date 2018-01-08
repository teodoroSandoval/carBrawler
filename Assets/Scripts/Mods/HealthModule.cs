using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthModule : MonoBehaviour {

    private float currentHealth;
    public float maxHealth = 1000;
    private Slider healthSlider;

    public bool isDead { get; private set; }
    private bool isActive = false;
    // Use this for initialization
    void Start () {
         
        healthSlider = GetComponent<CarSetup>().canvas.transform.Find("Slider1").GetComponent<Slider>();
        currentHealth = maxHealth;
        isDead = false;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void damage(float damageRecived) {

        Debug.LogWarning("Damage Called");

        if (isActive) {
            currentHealth -= damageRecived;

            if (currentHealth <= 0) {
                currentHealth = 0;
                isDead = true;
                GetComponent<CarSetup>().destroyCar();
            }
            else {
                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;

            }
            healthSlider.value = currentHealth;
        }
    }

    public void SetActive(bool activeState) {
        isActive = activeState;
    }
}
