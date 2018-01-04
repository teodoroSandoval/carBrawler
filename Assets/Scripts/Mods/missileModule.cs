using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileModule : MonoBehaviour {

    public LayerMask layer;
    private GameObject pointerPrefab;
    private GameObject missilePrefab;
    private GameObject missileDecal;
    public Material material;
    public float launchForce = 1000;
    public float propultion=10000;
    public float missileDamage = 200;
    private Transform target;
    public float explotionForce = 3000;
    public float explotionRadius = 1;
    public Vector3 outputOffset = Vector3.zero;

    private bool isActive = false;
    private CarSetup Setup;

    private GameObject[] cannons;
    // Use this for initialization
    void Start () {
        cannons = CarSetup.FindGameObjectInChildWithName(transform,"cannon");

        Setup = GetComponent<CarSetup>();

        missilePrefab = new GameObject("missile");
        missilePrefab.SetActive(false);
        missileDecal = new GameObject("missileDecal");
        missileDecal.SetActive(false);
        missileDecal = Resources.Load<GameObject>("explotionDecal");

        pointerPrefab = new GameObject("pointer");
        pointerPrefab.SetActive(false);
        CarSetup.initMesh(pointerPrefab, Resources.Load<Mesh>("pointer"), material);

        CarSetup.initMesh(missilePrefab, Resources.Load<Mesh>("missile"), material);

        MeshCollider collider = missilePrefab.AddComponent<MeshCollider>();
        collider.sharedMesh = Resources.Load<Mesh>("missile");
        collider.convex = true;
        missilePrefab.AddComponent<Rigidbody>().mass = 5;

        proyectileController missileController = missilePrefab.AddComponent<proyectileController>();
        missileController.mode = ProyectileMode.missile;
        missileController.propultion = propultion;
        missileController.damage = missileDamage;
        missileController.layer = layer;
        missileController.explotionForce = explotionForce;
        missileController.explotionRadius = explotionRadius;
        missileController.decal = missileDecal;

    }

    // Update is called once per frame
	void Update () {
        if (isActive) {
            if (Input.GetButtonDown(Setup.square)) {
                if (pointerPrefab.activeSelf) {
                    pointerPrefab.SetActive(false);
                }
                else {
                    pointerPrefab.SetActive(true);
                }
            }
            else {
                if (pointerPrefab.activeSelf) {

                    float hor = Input.GetAxis(Setup.RSHor);
                    float ver = -Input.GetAxis(Setup.RSVer);
                    float size = 2.5f;
                    RaycastHit hit;
                    Vector3 newPosition = transform.position + (transform.forward * size) + (transform.forward * ver * size) + (transform.right * hor * size);
                    newPosition.y = 100.0f;
                    if (Physics.Raycast(newPosition, Vector3.down, out hit)) { newPosition.y = hit.point.y; } else { newPosition.y = 0.0f; }
                    pointerPrefab.transform.position = newPosition;

                    if (Input.GetButtonDown(Setup.RShoulder)) {

                        if (hit.transform.tag.Contains("Player")) {
                            launchMissile(hit.transform);
                        }
                        else {
                            GameObject decoi = new GameObject("decoi");
                            decoi.transform.position = newPosition;
                            launchMissile(decoi.transform);
                        }
                    }
                }
            }
        }
    }

    private void launchMissile(Transform target) {

        foreach (GameObject cannon in cannons) {
            Vector3 offSet = (cannon.transform.right * outputOffset.x) + (cannon.transform.up * outputOffset.y) + (cannon.transform.forward * outputOffset.z);

            GameObject miss = Instantiate(missilePrefab, cannon.transform.position + offSet + (BasicMovement.velocity * Time.deltaTime), Quaternion.LookRotation(cannon.transform.forward));
            miss.layer = LayerMask.NameToLayer("weapons");
            miss.GetComponent<proyectileController>().target = target;
            miss.GetComponent<proyectileController>().aditionalSpeed = BasicMovement.velocity;
            miss.SetActive(true);
            miss.GetComponent<Rigidbody>().AddForce(cannon.transform.forward * launchForce, ForceMode.Force);
        }
    }
    public void SetActive(bool activeState) {
        isActive = activeState;
    }
}
