using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldModule : MonoBehaviour {

    private int layer;
    private GameObject shieldPrefab;
    private bool isDeployd = false;
    public float castTime = 0.2f;

    private Cooldown castingTimer;
    private bool isActive = false;
    private CarSetup Setup;

    private HealthModule health;
    // Use this for initialization
    void Start () {
        health = GetComponent<HealthModule>();

        layer = LayerMask.NameToLayer("shield");
        Setup = GetComponent<CarSetup>();
        
        shieldPrefab = Instantiate( Resources.Load<GameObject>("shieldSphere"),transform);
        shieldPrefab.name = "shield";
        GameObject collider = CarSetup.FindGameObjectInChildWithName(shieldPrefab.transform,"collider")[0];
        collider.SetActive(false);
        MeshCollider coll = shieldPrefab.AddComponent<MeshCollider>();

        coll.sharedMesh = collider.GetComponent<MeshFilter>().sharedMesh;
        coll.convex = true;
        coll.sharedMaterial = Resources.Load<PhysicMaterial>("PhysicsMaterials/noFriction");
        shieldPrefab.layer = layer;
        shieldPrefab.transform.localScale = Vector3.zero;
         
        shieldPrefab.SetActive(false);

        castingTimer = new Cooldown();
        castingTimer.init(0, castTime);
        //shieldPrefab.transform.position = transform.position;
        //shieldPrefab.transform.parent = transform;
    }

    // Update is called once per frame
    void Update() {

        if (isActive) {
            if (Input.GetButtonDown(Setup.LShoulder)) {
                isDeployd = true;
                castingTimer.use();
                shieldPrefab.SetActive(true);
                health.invulnerable = true;
            }
            else {
                if (Input.GetButtonUp(Setup.LShoulder)) {
                    isDeployd = false;
                    castingTimer.use();
                    health.invulnerable = false;
                }
            }

            if (isDeployd) {
                if (castingTimer.onCooldown) {
                    float scl = castingTimer.CDPercent;
                    //Debug.Log(scl);
                    shieldPrefab.transform.localScale = new Vector3(scl, scl, scl);
                }
            }
            else {
                if (castingTimer.onCooldown) {
                    float scl = 1 - castingTimer.CDPercent;
                    //Debug.Log(scl);
                    shieldPrefab.transform.localScale = new Vector3(scl, scl, scl);
                }
                else {
                    shieldPrefab.SetActive(false);
                }
            }
            castingTimer.customUpdate();
        }
    }

    public void SetActive(bool activeState) {
        isActive = activeState;
    }
}
