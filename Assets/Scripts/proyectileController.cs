using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProyectileMode { bullet, missile }
public class proyectileController : MonoBehaviour {
    public int layer;
    
    public float speed;
    public Vector3 aditionalSpeed = Vector3.zero;
    private RaycastHit hit;
    public GameObject decal;
    public ProyectileMode mode = 0;
    public float damage = 0;
    private Vector3 finalMoveVector;

    public Transform target;
    private Vector3 lastPosition;
    public float propultion;
    private Rigidbody missileRB;
    private bool propellerActive = false;

    public float explotionForce = 1000;
    public float explotionRadius = 1;

    void Start () {
		
	}

    private bool collitionActive = false;
    private LayerMask layerMask;
    private void Awake() {
        if (mode == ProyectileMode.bullet)
            finalMoveVector = (transform.forward * speed) + aditionalSpeed;

        layerMask = ~((1<<layer)| Physics.IgnoreRaycastLayer);


        if (mode == ProyectileMode.missile) {
            lastPosition = transform.position;
            missileRB = transform.GetComponent<Rigidbody>();
            missileRB.AddForce(aditionalSpeed);
            missileRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            collitionActive = true;
        }

    }
    // Update is called once per frame
    void Update () {
        if (mode == ProyectileMode.bullet) {
            if (Physics.Raycast(transform.position, finalMoveVector, out hit, finalMoveVector.magnitude * Time.deltaTime, layerMask)) {

                if (hit.rigidbody) {
                    hit.rigidbody.AddForceAtPosition(finalMoveVector,hit.point,ForceMode.Impulse);
                }

                if (hit.transform.root.tag.Contains("Player")) { hit.rigidbody.gameObject.SendMessage("damage", damage); }
                else {
                    GameObject dec = Instantiate(decal, hit.point + (hit.normal * CarSetup.minSurfaceDistance), Quaternion.LookRotation(hit.normal),hit.transform);
                    dec.AddComponent<LifeTime>().lifeTime = 30;
                    dec.SetActive(true);
                }

                Destroy(gameObject);
            }
            else {
                transform.position += finalMoveVector * Time.deltaTime;
            }
        }
        else {
            if (mode == ProyectileMode.missile) {
                
                if(!propellerActive) {
                    if (transform.position.y < lastPosition.y) {
                        propellerActive = true;
                        missileRB.useGravity = false;
                    }
                    lastPosition = transform.position;
                    //Debug.LogWarning("missile propeller activated");
                }
                else {
                    transform.LookAt(target);
                    missileRB.AddRelativeForce(Vector3.forward * propultion * Time.deltaTime,ForceMode.Force);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
         
        if (collitionActive) {
            ContactPoint hit = collision.contacts[0];
            explosiveController.addExplotion(hit.point,explotionForce,explotionRadius,damage,layer);

            if (!collision.transform.root.tag.Contains("Player")) {
                GameObject dec = Instantiate(decal, hit.point + (hit.normal * CarSetup.minSurfaceDistance), Quaternion.LookRotation(hit.normal), collision.transform);
                dec.AddComponent<LifeTime>().lifeTime = 30;
                dec.SetActive(true);
            }
            Destroy(gameObject);
        }
    }
}
