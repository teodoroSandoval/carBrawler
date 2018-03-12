using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class picker : MonoBehaviour {

    private Transform camera;

    private bool hasPicked;
    private Transform pickedObject;
    private Vector3 targetPosition;
    public float frontScaler = 1f;
    public float dragSpeed = 0.7f;
    public float rotSpeed = 0.7f;
    public float grabDistance = 0.3f;


    private bool grabbed = false;

    // Use this for initialization
    void Start () {
        camera = transform.Find("Camera_01");
        hasPicked = false;

    }

    private RaycastHit hit;
	// Update is called once per frame
	void Update () {

        targetPosition = camera.transform.position + (camera.transform.forward * frontScaler);

        if (!grabbed) {
            if (Input.GetMouseButtonDown(0)) {
                if (Physics.Raycast(camera.position, camera.forward, out hit, 1000)) {
                    if (hit.transform.tag == "pickeable") {
                        hasPicked = true;
                        pickedObject = hit.transform;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0)) {
                hasPicked = false;
                pickedObject = null;
            }

            if (hasPicked) {
                pickedObject.position += (targetPosition - pickedObject.position) * dragSpeed * Time.deltaTime;
                pickedObject.rotation = Quaternion.Lerp(Quaternion.LookRotation(pickedObject.forward), Quaternion.LookRotation(camera.forward), rotSpeed * Time.deltaTime);

                Debug.Log(Vector3.Distance(pickedObject.transform.position, targetPosition));
                if (Vector3.Distance(pickedObject.transform.position, targetPosition) < grabDistance) {
                    grabObject();
                }
            }
        } else {
            pickedObject.position = targetPosition;
            pickedObject.rotation = Quaternion.LookRotation(camera.forward);
        }
    }

    private void grabObject() {
        grabbed = true;

        
    }
}
