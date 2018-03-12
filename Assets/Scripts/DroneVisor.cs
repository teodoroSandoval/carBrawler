using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneVisor : MonoBehaviour {

    private DroneCamera droneCam;
    public GameObject drone;
    private GameObject user;
    private GameObject userCamBase;
    private bool isOnUse = false;

    // Use this for initialization
    void Start () {
        droneCam = drone.GetComponent<DroneCamera>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void transferDroneControl( GameObject playerCamera, GameObject player) {
        isOnUse = true;
        userCamBase = playerCamera.transform.parent.gameObject;

        user = player;
        
        droneCam.hostCamera(playerCamera);
        player.SendMessage("SetActive", false);
        drone.SendMessage("SetHumanActive", true);
    }

    public bool isBeingUsed() {
        return isOnUse;
    }
}
