using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileModule : MonoBehaviour {

    private int layer;
    private GameObject pointerPrefab;
    private GameObject missilePrefab;
    private GameObject missileDecal;

    private GameObject targetImage;
    private Material material;
    private Material pointerMaterial;

    public float launchForce = 1000;
    public float propultion=10000;
    public float missileDamage = 200;
    private Transform target;
    public float explotionForce = 3000;
    public float explotionRadius = 1;
    public Vector3 outputOffset = Vector3.zero;

    private bool isActive = false;
    private CarSetup Setup;

    private GameObject[] cannons;

    private Camera camera;

    private RectTransform targetingRect;
    private RectTransform CanvasRect;
    // Use this for initialization
    void Start () {
        Setup = GetComponent<CarSetup>();

        material = Setup.intenceRed;
        pointerMaterial = Setup.transparentRedTexture;

        layer = gameObject.layer;
        camera = Setup.camera;

        targetImage = Setup.canvas.transform.Find("targeting").gameObject;

        targetingRect = targetImage.GetComponent<RectTransform>();
        CanvasRect = Setup.canvas.GetComponent<RectTransform>();

        targetImage.SetActive(false);
        cannons = CarSetup.FindGameObjectInChildWithName(transform,"cannon");

        

        missilePrefab = new GameObject("missile");
        missilePrefab.SetActive(false);
        missileDecal = new GameObject("missileDecal");
        missileDecal.SetActive(false);
        missileDecal = Resources.Load<GameObject>("explotionDecal");

        pointerPrefab = new GameObject("pointer");
        pointerPrefab.SetActive(false);
        CarSetup.initMesh(pointerPrefab, Resources.Load<Mesh>("pointer"), pointerMaterial);

        CarSetup.initMesh(missilePrefab, Resources.Load<Mesh>("missile"), material);

        MeshCollider collider = missilePrefab.AddComponent<MeshCollider>();
        collider.sharedMesh = Resources.Load<Mesh>("missile");
        collider.convex = true;
        missilePrefab.AddComponent<Rigidbody>().mass = 5;

        proyectileController missileController = missilePrefab.AddComponent<proyectileController>();
        missileController.mode = ProyectileMode.missile;
        missileController.propultion = propultion;
        missileController.damage = missileDamage;
        missileController.layer = layer;
        missileController.explotionForce = explotionForce;
        missileController.explotionRadius = explotionRadius;
        missileController.decal = missileDecal;

    }

    // Update is called once per frame
	void Update () {
        if (isActive) {
            if (Input.GetButtonDown(Setup.square)) {
                if (pointerPrefab.activeSelf) {
                    pointerPrefab.SetActive(false);
                    targetImage.SetActive(false);
                }
                else {
                    pointerPrefab.SetActive(true);
                    targetImage.SetActive(true);
                }
            }
            else {
                if (pointerPrefab.activeSelf) {
                    
                    float hor = Input.GetAxis(Setup.RSHor);
                    float ver = -Input.GetAxis(Setup.RSVer);
                    float size = 2.5f;


                    Vector3 ViewportPosition = new Vector3((hor * 1) + 1, (ver * 1) + 1, 2) * 0.5f;

                    //camera.rect;

                    Vector3 newViewportPosition = new Vector3((hor * CanvasRect.sizeDelta.x) / 2, (ver * CanvasRect.sizeDelta.y) / 2,camera.nearClipPlane);

                    //newViewportPosition += new Vector2(0, CanvasRect.sizeDelta.y / 2);
                    //newViewportPosition = new Vector2((camera.rect.position.x* CanvasRect.sizeDelta.x)+ (newViewportPosition.x * camera.rect.width), (camera.rect.position.y * CanvasRect.sizeDelta.y) + (newViewportPosition.y* camera.rect.height));

                    targetingRect.anchoredPosition = newViewportPosition;
                    Vector3 newPosition = camera.ViewportToWorldPoint(ViewportPosition);
                    Vector3 newDirection = newPosition - camera.transform.position;

                    Debug.DrawRay(camera.transform.position, newDirection, Color.red);

                    RaycastHit hit;
                    if (Physics.Raycast(camera.transform.position, newDirection, out hit,100)) {

                        pointerPrefab.transform.position = hit.point;
                        pointerPrefab.transform.rotation = Quaternion.LookRotation(hit.normal);

                        if (Input.GetButtonDown(Setup.RShoulder)) {

                            if (hit.transform.root.tag.Contains("Player")) {
                                launchMissile(hit.transform);
                            }
                            else {
                                GameObject decoi = new GameObject("decoi");
                                decoi.transform.position = hit.point;
                                launchMissile(decoi.transform);
                            }
                        }
                    }
                }
            }
        }
    }

    private void launchMissile(Transform target) {

        foreach (GameObject cannon in cannons) {
            Vector3 offSet = (cannon.transform.right * outputOffset.x) + (cannon.transform.up * outputOffset.y) + (cannon.transform.forward * outputOffset.z);

            GameObject miss = Instantiate(missilePrefab, cannon.transform.position + offSet + (BasicMovement.velocity * Time.deltaTime), Quaternion.LookRotation(cannon.transform.forward));
            miss.layer = layer;
            miss.GetComponent<proyectileController>().target = target;
            miss.GetComponent<proyectileController>().aditionalSpeed = BasicMovement.velocity;
            miss.SetActive(true);
            miss.GetComponent<Rigidbody>().AddForce(cannon.transform.forward * launchForce, ForceMode.Force);
        }
    }
    public void SetActive(bool activeState) {
        isActive = activeState;
    }
}
