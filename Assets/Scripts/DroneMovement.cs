using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour {

    //private LayerMask layerMask;
    public Transform target;
    public float camDistance = 2f;
    public float camHeight = 1.2f;
    public Vector3 newPosition;
    public float minDistance = 0.1f;
    private bool isActive = false;
    private bool isHumanActive = false;
    public float forceMultiplyer = 2000;
    public float minForce = 196f;

    public float rotateSpeedScalar = 1;

    public float idleHeight = 1;
    private Rigidbody rb;


    void Start() {
        //layerMask = (1 << LayerMask.NameToLayer("mapOnly"));

        if (target != null) {
            newPosition = target.position - target.forward * camDistance;
            newPosition.y = target.position.y + camHeight;
            transform.position = newPosition;
        }
        else {
            if(newPosition == null)
                newPosition = transform.position+ Vector3.up;
        }

        rb = GetComponent<Rigidbody>();
    }

    private Vector3 currforce;
    // Update is called once per frame

    private void FixedUpdate() {
        rb.AddForce(currforce * Time.fixedDeltaTime);
    }

    void Update() {
        if (isActive){
            if (target != null) {

                newPosition = target.position - (target.forward * camDistance);
                newPosition.y = target.position.y + camHeight;

                Vector3 lDir = target.position - transform.position;
                transform.rotation =Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(lDir.x, 0, lDir.z)),rotateSpeedScalar*Time.deltaTime);
            }
            else {
                
                RaycastHit hit;
                if(Physics.Raycast(transform.position,Vector3.down, out hit)) {
                    newPosition = hit.point + (Vector3.up* idleHeight);
                }
            }

            Vector3 vect = newPosition - transform.position;
            currforce = (vect * vect.magnitude * rb.mass * forceMultiplyer) + (Vector3.up * minForce);
                
        }

        if (isHumanActive) {
            
            float hor = Input.GetAxis("horAxis");
            float ver = Input.GetAxis("vertAxis");

            currforce += ((transform.forward * ver) + (transform.right * hor))* forceMultiplyer;
        }

    }

    public void SetActive(bool activeState) {
        isActive = activeState;
    }
    public void SetHumanActive(bool activeState) {
        isHumanActive = activeState;
    }
}
