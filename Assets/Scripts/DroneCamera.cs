using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCamera : MonoBehaviour {

    private Transform target;
    private Transform rotor;
    private Transform cam;
    private bool isActive = false;
    private bool isHumanActive = false;
    private DroneMovement drone;
    private Camera camera;

    private float currHor = 0;
    private float currVer = 0;
    public float rotSpeed = 1;


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

            if (Input.GetKeyDown(KeyCode.Escape)) {

            }
        }

        if (isHumanActive) {
            float mouseX = Input.GetAxis("mouseHor");
            float mouseY = Input.GetAxis("mouseVer");
            
            currHor += (mouseX * Time.deltaTime * rotSpeed);
            if (currHor > 360) {
                currHor -= 360;
            } else {
                if (currHor < 0) {
                    currHor += 360;
                }
            }

            currVer += (mouseY * Time.deltaTime * rotSpeed);
            if (currVer > 360) {
                currVer -= 360;
            } else {
                if (currVer < 0) {
                    currVer += 360;
                }
            }

            rotor.transform.localRotation = Quaternion.AngleAxis(currHor,Vector3.up);
            cam.transform.localRotation = Quaternion.AngleAxis(-currVer,Vector3.right);
        }
	}

    public void SetActive(bool activeState) {
        isActive = activeState;

        camera.enabled = activeState;

    }
    public void SetHumanActive(bool activeState) {
        isHumanActive = activeState;
    }
    public void hostCamera(GameObject hostedCamera) {
        camera.enabled = false;
        hostedCamera.transform.parent = cam.transform;
        hostedCamera.transform.localPosition = Vector3.zero;
        hostedCamera.transform.localRotation = Quaternion.identity;
    }

    public void resetCamera() {

    }
}
