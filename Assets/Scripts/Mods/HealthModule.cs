using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthModule : MonoBehaviour {

    public float currentHealth;
    public float maxHealth = 1000;
    private Slider healthSlider;
    public bool invulnerable = false;
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

    private void damage(float damageRecived) {

        Debug.LogWarning("Damage Called");

        if (isActive) {

            if (!invulnerable) {
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
    }

    public void SetActive(bool activeState) {
        isActive = activeState;
    }
}
