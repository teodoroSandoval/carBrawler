using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown {

    private float baseTime, CDTime, elapsedTime, length, deathBaseTime, deathTime, deathTimeCD = 0;

    public float currentLoad { get; private set; }
    public float CDPercent { get; private set; }

    public bool onCooldown { get; private set; }
    private bool isRechargeable = false;

    private bool depleted = false;

    public void init(float gaugeLength, float cooldownTime) {
        length = currentLoad = gaugeLength;
        CDTime = cooldownTime;
        CDPercent = 0;
        onCooldown = false;
    }

    public void init(float gaugeLength, float cooldownTime,float deathTimeCooldown) {
        isRechargeable = true;
        deathBaseTime = Time.time;
        deathTime = deathTimeCooldown;
        length = currentLoad = gaugeLength;
        CDTime = cooldownTime;
        CDPercent = 0;
        onCooldown = false;
    }

    public bool use() {

        if (!depleted && isRechargeable) {
            currentLoad -= Time.deltaTime;
            onCooldown = false;
            elapsedTime = (currentLoad / length) * CDTime;
            deathBaseTime = Time.time;
        }
        else {
            if (!onCooldown) {
                currentLoad -= Time.deltaTime;
                elapsedTime = (currentLoad / length) * CDTime;
            }
        }

        if (currentLoad < 0) {
            currentLoad = 0;
            CDPercent = 0;
            onCooldown = true;
            baseTime = Time.time;
            depleted = true;
            elapsedTime = 0;
            return false;
        }
        return true;
    }

    public void customUpdate() {
        gaugeAndCooldown();
    }
    private void gaugeAndCooldown() {
        if (onCooldown) {
            
            elapsedTime += Time.deltaTime;

            CDPercent = elapsedTime / CDTime;
            currentLoad = CDPercent * length;
            if (elapsedTime > CDTime) {
                CDPercent = 1;
                onCooldown = false;
                currentLoad = length;
                elapsedTime = 0;
                depleted = false;
            }
        }
        else {

            if (isRechargeable && !depleted && deathBaseTime + deathTime < Time.time && currentLoad < length) {
                elapsedTime = (currentLoad / length) * CDTime;
                onCooldown = true;
            }
                
        }
    }
}


public class LifeTime : MonoBehaviour {

    public float lifeTime = 0;
    private float baseTime;

    private void Awake() {
        baseTime = Time.time;
    }
    private void Update() {
        if (baseTime + lifeTime < Time.time)
            Destroy(gameObject);
    }
}
