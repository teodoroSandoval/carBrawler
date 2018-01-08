using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarSetup))]

public class BasicMovement : MonoBehaviour {

    public static Vector3 velocity { get; private set; }

    private string directionAxis;

    // wheels
    private GameObject[] wheelsDirAxis;
    private WheelCollider[] wheelsDirCollider;
    private WheelCollider[] wheelsTracCollider;
    private Quaternion[] defaultRotation;

    // misc objects
    private Rigidbody rigidBody;

    //movement
    public float engineForce = 200f;
    public float brakeForce = 0.1f;
    public float maxWheelDirAngle = 10;
    private float power = 0.0f;
    private float steer = 0.0f;
    private float brake = 0.0f;

    private bool stoped = false;


    private bool isActive = false;

    private CarSetup Setup;
    // Use this for initialization
    void Start() {

        Setup = GetComponent<CarSetup>();

        wheelsDirAxis = Setup.GetDirectionalAxis();
        wheelsDirCollider= new WheelCollider[wheelsDirAxis.Length];
        defaultRotation = new Quaternion[wheelsDirAxis.Length];

        for (int a=0; a< wheelsDirAxis.Length; a++) {
            wheelsDirCollider[a] = wheelsDirAxis[a].GetComponentInChildren<WheelCollider>();
            defaultRotation[a] = wheelsDirAxis[a].transform.localRotation;
        }

        wheelsTracCollider = Setup.GetWheelsTractionColliders();
        rigidBody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        velocity = rigidBody.velocity;
        if (isActive)
            move();
	}

    private void setTraction (float powerF, float brakeF) {

        foreach(WheelCollider wheel in wheelsTracCollider) {
            wheel.motorTorque = powerF;
            wheel.brakeTorque = brakeF;
        }
    }

    private void setDirection (float steerAngle) {
        for (int a = 0; a < wheelsDirAxis.Length; a++) {

            wheelsDirAxis[a].transform.localRotation = defaultRotation[a]*Quaternion.AngleAxis(steerAngle, Vector3.up);

            wheelsDirCollider[a].steerAngle = steerAngle * Mathf.Sign(wheelsDirAxis[a].transform.up.y) * Mathf.Sign(transform.up.y);
        }
    }

    void move() {
        
        if (Input.GetAxis(Setup.accelerationAxis) > 0.001 || Input.GetAxis(Setup.accelerationAxis) < -0.001) {
            brake = 0.0f;
            stoped = false;
            float value = Input.GetAxis(Setup.accelerationAxis);
            power = Mathf.Sign(value) * (value * value) * engineForce;

            setTraction(power, brake);
        }
        else {
            if (!stoped) {
                stoped = true;

                power = 0.0f;
                brake = brakeForce;

                setTraction(power,brake);
            }
        }

        steer = Input.GetAxis(Setup.LSHor) * maxWheelDirAngle;

        setDirection(steer);
        //wheelsCollider[FL].steerAngle = wheelsCollider[FR].steerAngle = (steer);
        //wheelsCollider[BL].steerAngle = wheelsCollider[BR].steerAngle = (-steer);

        //wheelsDirAxis[FL].transform.localRotation = wheelsDirAxis[FR].transform.localRotation = Quaternion.AngleAxis(steer, Vector3.up);
        //wheelsDirAxis[BL].transform.localRotation = wheelsDirAxis[BR].transform.localRotation = Quaternion.AngleAxis(-steer, Vector3.up);

    }

    public void addVelocity(Vector3 velocityVector) {
        rigidBody.velocity += velocityVector;
    }
    public void addForce(Vector3 forceVector) {
        rigidBody.AddRelativeForce(forceVector,ForceMode.Force);
    }
    public void SetActive(bool activeState) {
        isActive = activeState;
    }

}
