using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class explosiveController : MonoBehaviour {
    public LayerMask layer;
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
		
        if (useSensor) {

        }
	}

    private void explote() {
        GameObject newDecal = Instantiate(decal, transform.position + (transform.up * CarSetup.minSurfaceDistance), Quaternion.LookRotation(transform.up), transform.parent);
        newDecal.AddComponent<LifeTime>().lifeTime = 30;
        newDecal.SetActive(true);
        addExplotion(transform.position, explotionForce, explotionRadius, damage, layer);
        Destroy(gameObject);
    }
    
    public static void addExplotion(Vector3 position,float force, float radius, float damage, LayerMask layer) {
        Collider[] targets = Physics.OverlapSphere(position, radius);
        List<GameObject> targetList = new List<GameObject>();

        foreach(Collider hit in targets) {
            if (!targetList.Contains(hit.gameObject)) {
                targetList.Add(hit.gameObject);
                if (hit.tag.Contains("Player") && ((1 << hit.gameObject.layer) & layer) != 0) {
                    float finalDamage = (radius - Vector3.Distance(position, hit.ClosestPoint(position))) / radius * damage;

                    if (hit.attachedRigidbody != null)
                        hit.attachedRigidbody.gameObject.SendMessage("damage", finalDamage);
                    else
                        Debug.Log("not rigid body attached to this target");
                }

                Rigidbody rb = hit.attachedRigidbody;
                if (rb != null) {
                    rb.AddExplosionForce(force, position, radius);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (useSensor) {
            explote();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Contains("Player") && transform.parent == null) {
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
