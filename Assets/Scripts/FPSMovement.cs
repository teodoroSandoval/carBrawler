using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMovement : MonoBehaviour {

    public float speed = 10;
    public float rotSpeed = 1;

    private float currHor = 0;
    private float currVer = 0;

    private CharacterController charCont;
    public Transform camera;

    private bool isActive = true;

    // Use this for initialization
    void Start () {
        charCont = GetComponent<CharacterController>();
        //camera = transform.Find("Camera_01");
	}
	
	// Update is called once per frame
	void Update () {
        if (isActive) {
            float hor = Input.GetAxis("horAxis");
            float ver = Input.GetAxis("vertAxis");

            float mouseX = Input.GetAxis("mouseHor");
            float mouseY = Input.GetAxis("mouseVer");


            currHor += (mouseX * Time.deltaTime * rotSpeed);
            if (currHor > 360) {
                currHor -= 360;
            }
            else {
                if (currHor < 0) {
                    currHor += 360;
                }
            }

            currVer += (mouseY * Time.deltaTime * rotSpeed);
            if (currVer > 360) {
                currVer -= 360;
            }
            else {
                if (currVer < 0) {
                    currVer += 360;
                }
            }

            //Vector3 rot = new Vector3(-currVer, currHor, 0);

            camera.localRotation = Quaternion.AngleAxis(-currVer, Vector3.right);

            charCont.transform.rotation = Quaternion.AngleAxis(currHor, Vector3.up);

            charCont.Move(new Vector3(speed * hor, 0, speed * ver) * Time.deltaTime);
        }
    }

    public void SetActive(bool activeState) {
        isActive = activeState;
    }
}
