using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (PlayersData.readyCount == 1) {
            
            GetComponent<CameraController>().targetCar = GameObject.Find("Car1").transform;
        }
        else {
            if (PlayersData.readyCount == 2) {
                Rect re1 = new Rect(0, 0.5f, 1, 0.5f);
                GetComponent<Camera>().rect = re1;
                GetComponent<CameraController>().targetCar = GameObject.Find("Car1").transform;

                Rect re2 = new Rect(0, 0, 1, 0.5f);
                GameObject cam2_obj = new GameObject("Camera_02");
                Camera cam2 = cam2_obj.AddComponent<Camera>();
                cam2.rect = re2;

                GameObject newCarInstance = Instantiate(GameObject.Find("Car1"));
                newCarInstance.transform.position = new Vector3(0.5f,1.2f,-0.5f);
                Destroy(newCarInstance.GetComponent<BasicMovement>());
                cam2_obj.AddComponent<CameraController>().targetCar = newCarInstance.transform;
            }
        }
    }
}
