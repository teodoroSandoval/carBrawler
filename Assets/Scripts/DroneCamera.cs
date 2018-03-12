using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCamera : MonoBehaviour {

    private Transform target;
    private Transform rotor;
    private Transform cam;
    private bool isActive = false;

    private DroneMovement drone;
    private Camera camera;
    // Use this for initialization
    void Start () {
        rotor = transform.Find("rotor");
        if (rotor != null)
            cam = rotor.Find("cam");

        drone = GetComponent<DroneMovement>();
        camera = cam.GetComponentInChildren<Camera>();
        camera.enabled = false;
        
	}
	
	// Update is called once per frame
	void Update () {
        if (isActive) {
            if(drone.target != null){

                //
                Vector3 dir = drone.target.transform.position - transform.position ;

                Vector3 angles = Quaternion.LookRotation(dir).eulerAngles;

                rotor.transform.localRotation = Quaternion.AngleAxis(angles.y - transform.rotation.eulerAngles.y, Vector3.up);
                //Debug.Log(rotor.transform.rotation);
                cam.transform.localRotation = Quaternion.AngleAxis(angles.x-transform.rotation.eulerAngles.x, Vector3.right);
                //cam.transform.LookAt(drone.target);
            }
        }
	}

    public void SetActive(bool activeState) {
        isActive = activeState;

        camera.enabled = isActive?true:false;

    }
}
