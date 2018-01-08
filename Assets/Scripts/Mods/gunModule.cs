using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunModule : MonoBehaviour {
    private int layer;
    private GameObject bulletPrefab;
    private GameObject bulletDecal;

    private GameObject[] guns;

    private Material material;
    public float bulletSpeed = 10;
    public Vector3 outputOffset = Vector3.zero;

    public bool singleFire = false;
    public float spreading = 1;
    public float rateOfFire = 10;

    public float bulletDamage = 10;
    private float bulletCD;

    private Cooldown bulletTimer;

    private bool isActive = false;
    private CarSetup Setup;
    // Use this for initialization
    void Start () {
        layer = gameObject.layer;

        Setup = GetComponent<CarSetup>();

        material = Setup.intenceRed;

        guns = CarSetup.FindGameObjectInChildWithName(transform,"gun");

        bulletCD = 1 / rateOfFire;
        bulletPrefab = new GameObject("bullet");
        bulletPrefab.SetActive(false);

        bulletDecal = new GameObject("bulletDecal");
        bulletDecal.SetActive(false);

        bulletDecal = Resources.Load<GameObject>("bulletHit");

        CarSetup.initMesh(bulletPrefab,Resources.Load<Mesh>("bullet"), material);
        proyectileController bullet = bulletPrefab.AddComponent<proyectileController>();
        bullet.mode = ProyectileMode.bullet;
        bullet.speed = bulletSpeed;
        bullet.damage = bulletDamage;
        bullet.layer = layer;

        bulletTimer = new Cooldown();
        bulletTimer.init(0,bulletCD);
    }
	
	// Update is called once per frame
	void Update () {

        if (isActive) {
            bulletTimer.customUpdate();
            if (singleFire) {
                if (Input.GetButtonDown(Setup.circle)) {

                    shootSingle();

                }

            }
            else {
                if (Input.GetButton(Setup.circle)) {

                    shootAuto();

                }

            }
        }
    }

    private void shoot(Transform cannon) {
        Vector3 offSet = (cannon.right * outputOffset.x) + (cannon.up * outputOffset.y) + (cannon.forward * outputOffset.z);

        GameObject bullet = Instantiate(bulletPrefab, cannon.position + offSet, cannon.rotation * Quaternion.Euler(Random.Range(-spreading, spreading), Random.Range(-spreading, spreading), 0));
        bullet.GetComponent<proyectileController>().aditionalSpeed = BasicMovement.velocity;
        bullet.GetComponent<proyectileController>().decal = bulletDecal;

        bullet.SetActive(true);
    }
    private void shootSingle() {
        foreach (GameObject gun in guns) {
            shoot(gun.transform);
        }
    }
    private void shootAuto() {
        if (!bulletTimer.onCooldown) {
            shootSingle();
        }
        bulletTimer.use();
    }

    public void SetActive(bool activeState) {
        isActive = activeState;
    }
}
