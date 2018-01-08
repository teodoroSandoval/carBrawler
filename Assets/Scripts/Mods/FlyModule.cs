using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BasicMovement))]

public class FlyModule : MonoBehaviour {

    private BasicMovement moveManager;
    private Cooldown flyCooldown;
    private Cooldown flyGauge;
    private Slider flySlider;

    public float flyCD = 5.0f;
    public float flyLength = 2.0f;
    public float recoveryCD = 2.0f;
    private bool canFly = false;

    public float jumpForce = 4f;
    public float flyForceUp = 300;
    public float flyForceForward = 500;

    public float airRotationSpeed = 100;
    private bool isActive = false;
    private CarSetup Setup;

    // Use this for initialization
    void Start () {
        Setup = GetComponent<CarSetup>();

        flySlider = Setup.canvas.transform.Find("Slider2").GetComponent<Slider>();
        moveManager = GetComponent<BasicMovement>();
        flyGauge = new Cooldown();
        flyGauge.init(flyLength,flyCD, recoveryCD);
        flySlider.maxValue = flyLength;
        flySlider.value = flySlider.maxValue;
    }
	
	// Update is called once per frame
	void Update () {
        if (isActive) {
            if (Setup.IsCarGrounded()) {
                canFly = false;
                if (Input.GetButtonDown(Setup.cross)) {
                    moveManager.addVelocity(Vector3.up * jumpForce);
                }
            }
            else {
                Quaternion pitch = Quaternion.Euler(Input.GetAxis(Setup.LSVer) * airRotationSpeed * Time.deltaTime, Input.GetAxis(Setup.LSHor) * airRotationSpeed * Time.deltaTime, 0);
                transform.rotation *= pitch;

                //Vector3 rotation = new Vector3(Input.GetAxis(Setup.LSVer), Input.GetAxis(Setup.LSHor), 0) * airRotationSpeed * Time.deltaTime;
                //GetComponent<Rigidbody>().AddTorque(rotation, ForceMode.Force);

                if (Input.GetButtonDown(Setup.cross) && !canFly) {
                    canFly = true;
                }


                if (Input.GetButton(Setup.cross) && canFly) {
                    if (!flyGauge.onCooldown && flyGauge.use()) {
                        moveManager.addForce((Vector3.up * flyForceUp) + (Vector3.forward * flyForceForward));

                    }
                    else {
                        canFly = false;
                    }
                }
            }

            flySlider.value = flyGauge.currentLoad;
            flyGauge.customUpdate();
        }
    }

    public void SetActive(bool activeState) {
        isActive = activeState;
    }
}
