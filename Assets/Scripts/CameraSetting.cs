using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSetting : MonoBehaviour {

    private GameObject setCar(GameObject originalCar,Vector3 position, Quaternion rotation , int selfLayer, Camera camera, GameObject canvas) {



        GameObject car = Instantiate(originalCar, position, rotation);

        CarSetup.SetLayerAll(car.transform,selfLayer);

        CarSetup carSetup = car.AddComponent<CarSetup>();

        carSetup.asBot = false;
        carSetup.camera = camera;

        //GameObject newGuiElements = new GameObject("guiElements");

        GameObject newCanvas = Instantiate(canvas);

        foreach(Transform guiElement in newCanvas.transform) {

            RectTransform rect = guiElement.GetComponent<RectTransform>();

            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x*camera.rect.width, rect.anchoredPosition.y * camera.rect.height);
        }

        newCanvas.SetActive(true);
        carSetup.canvas = newCanvas.GetComponent<Canvas>();
        carSetup.canvas.renderMode = RenderMode.ScreenSpaceCamera;
        carSetup.canvas.worldCamera = camera;
        carSetup.canvas.planeDistance = camera.nearClipPlane+CarSetup.minSurfaceDistance;


        car.AddComponent<BasicMovement>();
        car.AddComponent<HealthModule>();
        car.AddComponent<FlyModule>();
        car.AddComponent<shieldModule>();
        car.AddComponent<gunModule>();
        car.AddComponent<missileModule>();
        car.AddComponent<mineModule>();

        carSetup.Set();

        
        car.name = "Car - " + carSetup.playerName;
        newCanvas.name = "Canvas" + carSetup.playerID;
        car.SetActive(true);

        return car;
    }
	// Use this for initialization
	void Start () {
        GameObject canvas = GameObject.Find("Canvas");

        canvas.SetActive(false);
        if (PlayersData.readyCount == 1) {

            GameObject car = setCar(GameObject.Find("Car1"),new Vector3(-0.5f,1.1f,0.5f),Quaternion.AngleAxis(-45,Vector3.up), LayerMask.NameToLayer("player1"), GetComponent<Camera>(), canvas);

            GetComponent<CameraController>().targetCar = car.transform;
        }
        else {
            
            if (PlayersData.readyCount == 2) {
                Rect re1 = new Rect(0, 0.5f, 1, 0.5f);
                Rect re2 = new Rect(0, 0, 1, 0.5f);

                Camera camera1 = GetComponent<Camera>();
                camera1.rect = re1;

                GameObject car1 = setCar(GameObject.Find("Car1"), new Vector3(-0.5f, 1.1f, 0.5f), Quaternion.AngleAxis(-45, Vector3.up), LayerMask.NameToLayer("player1"), camera1, canvas);

                GetComponent<CameraController>().targetCar = car1.transform;


                Camera camera2 = new GameObject("Camera_02").AddComponent<Camera>();
                camera2.rect = re2;

                GameObject car2 = setCar(GameObject.Find("Car1"), new Vector3(0.5f, 1.1f, -0.5f), Quaternion.AngleAxis(135, Vector3.up), LayerMask.NameToLayer("player2"), camera2, canvas);

                camera2.gameObject.AddComponent<CameraController>().targetCar = car2.transform;
            }
        }
    }
}
