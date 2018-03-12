using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosiveController : MonoBehaviour {
    public int layer;
    public bool useSensor = true;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          

    public float explotionForce = 1000;
    public float explotionRadius = 2;
    public float damage;
    public GameObject decal;
    
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void explote() {
        GameObject newDecal = Instantiate(decal, transform.position + (transform.up * CarSetup.minSurfaceDistance), Quaternion.LookRotation(transform.up), transform.parent);
        newDecal.AddComponent<LifeTime>().lifeTime = CarSetup.globalLifetime;
        newDecal.SetActive(true);
        addExplotion(transform.position, explotionForce, explotionRadius, damage, layer);
        Destroy(gameObject);
    }
    
    public static void addExplotion(Vector3 position,float force, float radius, float damage, int layer) {
        Collider[] targets = Physics.OverlapSphere(position, radius);
        List<Transform> rootList = new List<Transform>();
        List<GameObject> objectList = new List<GameObject>();

        foreach (Collider hit in targets) {
            if (!rootList.Contains(hit.transform.root)) {
                rootList.Add(hit.transform.root);
                if (hit.transform.root.tag.Contains("Player")) {
                    if (hit.gameObject.layer != layer) {
                        float finalDamage = (radius - Vector3.Distance(position, hit.ClosestPoint(position))) / radius * damage;
                        hit.transform.root.SendMessage("damage", finalDamage);
                    }

                    if (hit.transform.root.GetComponent<HealthModule>().isDead) {
                        Rigidbody[] rbs = hit.transform.GetComponentsInChildren<Rigidbody>();

                        foreach(Rigidbody rb in rbs) {
                            rb.AddExplosionForce(force, position, radius);
                        }
                    }
                    else {
                        hit.attachedRigidbody.AddExplosionForce(force, position, radius);
                    }
                }
                else {
                    Rigidbody rb = hit.attachedRigidbody;
                    if (rb != null) {
                        rb.AddExplosionForce(force, position, radius);
                    }
                }
            }

            if (!objectList.Contains(hit.gameObject)) {
                objectList.Add(hit.gameObject);

                Rigidbody rb = hit.attachedRigidbody;
                if (rb != null) {
                    rb.AddExplosionForce(force, position, radius);
                }
            }
        }

        List<Transform> targetList = new List<Transform>();


    }

    private void OnTriggerEnter(Collider other) {
        if (useSensor) {
            explote();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.root.tag.Contains("Player") && transform.parent == null) {
            explote();
        }
    }

    private void OnCollisionStay(Collision collision) {
        
        if (transform.parent == null) {

            Rigidbody rb = transform.GetComponent<Rigidbody>();

            if(rb.velocity == Vector3.zero) {
                transform.parent = collision.transform;
                Destroy(rb);
            }
        }
    }
}
