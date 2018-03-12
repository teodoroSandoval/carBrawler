using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineModule : MonoBehaviour {
    private int layer;
    private GameObject minePrefab;
    private GameObject mineDecal;
    private Material material;
    
    public int stock = 100;
    private int used = 0;
    public float explotionDamage = 300;
    public float sensorSize = 0.1f;
    public float sensorHeight=0.015f;
    public int sensorDefinition = 16;
    public float explotionForce = 3500;
    public float explotionRadius = 0.5f;

    private bool isActive = false;

    private CarSetup Setup;

    // Use this for initialization

    void Start () {
        layer = gameObject.layer;
        Setup = GetComponent<CarSetup>();

        material = Setup.intenceRed;

        minePrefab = new GameObject("mine");
        minePrefab.SetActive(false);
        mineDecal = new GameObject("mineDecal");
        mineDecal.SetActive(false);
        mineDecal = Resources.Load<GameObject>("explotionDecal");

        GameObject mineObject = Resources.Load<GameObject>("mine");

        CarSetup.initMesh(minePrefab, mineObject.transform.Find("mine_mesh").GetComponent<MeshFilter>().sharedMesh, material);
        MeshCollider sensor = minePrefab.AddComponent<MeshCollider>();

        sensor.sharedMesh = mineObject.transform.Find("mine_sensor").GetComponent<MeshFilter>().sharedMesh;
        sensor.convex = true;
        sensor.isTrigger = true;


        MeshCollider collider = minePrefab.AddComponent<MeshCollider>();
        collider.sharedMesh = mineObject.transform.Find("mine_collider").GetComponent<MeshFilter>().sharedMesh;
        collider.convex = true;
        Rigidbody rb = minePrefab.AddComponent<Rigidbody>();
        rb.mass = 5;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        minePrefab.layer = layer;

        ExplosiveController explosive = minePrefab.AddComponent<ExplosiveController>();
        explosive.useSensor = true;
        explosive.damage = explotionDamage;
        explosive.layer = layer;
        explosive.explotionForce = explotionForce;
        explosive.explotionRadius = explotionRadius;
        explosive.decal = mineDecal;
    }
	
	// Update is called once per frame
	void Update () {
        if (isActive) {
            if (Input.GetButtonDown(Setup.triangle) && used < stock) {

                GameObject mineObject = Instantiate(minePrefab, transform.position, Quaternion.FromToRotation(Vector3.up, transform.up));

                mineObject.SetActive(true);
                mineObject.transform.parent = null;
                used += 1;
            }
        }
    }

    public void SetActive(bool activeState) {
        isActive = activeState;
    }
}
