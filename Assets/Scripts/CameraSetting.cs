using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSetting : MonoBehaviour {

    private GameObject setCar(GameObject originalCar,Vector3 position, Quaternion rotation , int selfLayer, Camera camera, GameObject canvas, Material material) {

        GameObject car = Instantiate(originalCar, position, rotation);

        camera.transform.position = (car.transform.forward * -20) + (car.transform.up * 10);
        camera.transform.LookAt(car.transform);

        CarSetup.SetLayerAll(car.transform,selfLayer);

        CarSetup carSetup = car.AddComponent<CarSetup>();

        carSetup.asBot = false;
        carSetup.camera = camera;

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
        car.AddComponent<ShieldModule>();
        car.AddComponent<GunModule>();
        car.AddComponent<MissileModule>();
        car.AddComponent<MineModule>();

        carSetup.Set();

        
        car.name = "Car - " + carSetup.playerName;
        newCanvas.name = "Canvas" + carSetup.playerID;
        car.SetActive(true);

        carSetup.setMaterial(material);
        return car;
    }
	// Use this for initialization
	void Start () {
        GameObject canvas = GameObject.Find("Canvas");

        canvas.SetActive(false);
        if (PlayersData.readyCount == 1) {
            CameraController camContrl = gameObject.AddComponent<CameraController>();
            GameObject car = setCar(GameObject.Find("Car1"),new Vector3(-0.5f,1.1f,0.5f),Quaternion.AngleAxis(-45,Vector3.up), LayerMask.NameToLayer("player1"), GetComponent<Camera>(), canvas, Resources.Load<Material>("Materials/defaultMaterial"));

            camContrl.targetCar = car.transform;
        }
        else {
            
            if (PlayersData.readyCount == 2) {
                Rect re1 = new Rect(0, 0.5f, 1, 0.5f);
                Rect re2 = new Rect(0, 0, 1, 0.5f);

                Camera camera1 = GetComponent<Camera>();
                camera1.rect = re1;

                CameraController camContrl1 = gameObject.AddComponent<CameraController>();

                GameObject car1 = setCar(GameObject.Find("Car1"), new Vector3(-0.5f, 1.1f, 0.5f), Quaternion.AngleAxis(-45, Vector3.up), LayerMask.NameToLayer("player1"), camera1, canvas, Resources.Load<Material>("Materials/blue"));
                camContrl1.targetCar = car1.transform;
                //car1.GetComponent<CarSetup>().setMaterial(Resources.Load<Material>("Materials/blue"));
                

                Camera camera2 = new GameObject("Camera_02").AddComponent<Camera>();
                camera2.rect = re2;

                CameraController camContrl2 = camera2.gameObject.AddComponent<CameraController>();

                GameObject car2 = setCar(GameObject.Find("Car1"), new Vector3(0.5f, 1.1f, -0.5f), Quaternion.AngleAxis(135, Vector3.up), LayerMask.NameToLayer("player2"), camera2, canvas, Resources.Load<Material>("Materials/red"));
                camContrl2.targetCar = car2.transform;
                //car2.GetComponent<CarSetup>().setMaterial(Resources.Load<Material>("Materials/red"));

            }
        }
    }
}
