 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSetup : MonoBehaviour {

    public static float minSurfaceDistance = 0.0001f;


    public bool asBot = false;

    // buttons
    public string LSHor { get; private set; }
    public string LSVer { get; private set; }
    public string RSHor { get; private set; }
    public string RSVer { get; private set; }
    public string accelerationAxis { get; private set; }

    public string triangle { get; private set; }
    public string cross { get; private set; }
    public string circle { get; private set; }
    public string square { get; private set; }

    public string RShoulder { get; private set; }
    public string LShoulder { get; private set; }

    public bool isActive = false;
    public string playerName { get; private set; }
    public int playerID { get; private set; }

    private GameObject[] wheelsDirAxis;
    private GameObject[] wheelsRotAxis;
    private WheelCollider[] wheelsCollider;
    private WheelCollider[] wheelsTractionCollider;
    public Material defaultMaterial;
    public Material deadTexture;
    public PhysicMaterial defaultPhMat;
    // Use this for initialization

    
    void Start() {

        deadTexture = Resources.Load<Material>("Materials/gray");

        //defaultMaterial = Resources.Load<Material>("Materials/defaultMaterial");
        defaultPhMat = Resources.Load<PhysicMaterial>("PhysicsMaterials/defaultObject");
        if (asBot) {
            playerName = "bot";
            isActive = true;
            gameObject.SendMessage("SetActive", true);

            Debug.Log("setted a bot");
        }
        else {
            playerID = PlayersData.getPlayer();
            if (playerID < 0) {
                isActive = false;
                gameObject.SendMessage("SetActive", false);
            }
            else {
                isActive = true;
                playerName = "P" + playerID;
                gameObject.SendMessage("SetActive", true);
            }
            Debug.Log("setted a human");
        }

        LSHor = playerName + "_LeftHor";
        LSVer = playerName + "_LeftVer";
        RSHor = playerName + "_RightHor";
        RSVer = playerName + "_RightVer";
        accelerationAxis = playerName + "_Acceleration";
        triangle = playerName + "_Fire3";
        square = playerName + "_Fire2";
        circle = playerName + "_Fire1";
        cross = playerName + "_Jump";
        LShoulder = playerName + "_Launch1";
        RShoulder = playerName + "_Launch2";

        // SET BASE CAR
        Rigidbody carRB = gameObject.AddComponent<Rigidbody>();
        carRB.mass = 50;
        carRB.drag = 0;
        carRB.angularDrag = 0.05f;
        carRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
        carRB.centerOfMass = Vector3.zero;

        GameObject[] carColliders = FindGameObjectInChildWithName(transform, "collider");

        GameObject carMesh = FindGameObjectInChildWithName(transform, "mesh")[0];
        carMesh.GetComponent<MeshRenderer>().material = defaultMaterial;
        foreach (GameObject collider in carColliders) {
            MeshCollider meshColl = carMesh.AddComponent<MeshCollider>();
            meshColl.sharedMesh = collider.GetComponent<MeshFilter>().sharedMesh;
            meshColl.convex = true;
            meshColl.material = defaultPhMat;
            collider.SetActive(false);
        }

        //GameObject[] axisArray = FindGameObjectInChildWithName(transform, "Axis");

        // LOAD WHEELS
        wheelsDirAxis = FindGameObjectInChildWithName(transform, "direction");
        wheelsRotAxis = FindGameObjectInChildWithName(transform, "rotation");

        wheelsCollider = new WheelCollider[wheelsRotAxis.Length];

        JointSpring spring = new JointSpring();
        spring.spring = 10000;
        spring.damper = 70;
        spring.targetPosition = 0.5f;

        WheelFrictionCurve forwardFrCurve = new WheelFrictionCurve();
        forwardFrCurve.extremumSlip = 1.4f;
        forwardFrCurve.extremumValue = 1f;
        forwardFrCurve.asymptoteSlip = 1.8f;
        forwardFrCurve.asymptoteValue = 0.5f;
        forwardFrCurve.stiffness = 1.3f;

        WheelFrictionCurve sideFrCurve = new WheelFrictionCurve();
        sideFrCurve.extremumSlip = 1.2f;
        sideFrCurve.extremumValue = 1f;
        sideFrCurve.asymptoteSlip = 1.5f;
        sideFrCurve.asymptoteValue = 0.75f;
        sideFrCurve.stiffness = 0.8f;

        GameObject wheelAsset = Resources.Load<GameObject>("defaultWheel");

        Mesh wheelMesh = wheelAsset.transform.Find("wheelMesh").GetComponent<MeshFilter>().sharedMesh;
        Mesh wheelAxisMesh = wheelAsset.transform.Find("wheelAxisMesh").GetComponent<MeshFilter>().sharedMesh;
        Mesh wheelRad = wheelAsset.transform.Find("wheelRadius").GetComponent<MeshFilter>().sharedMesh;
        Mesh wheelColl = wheelAsset.transform.Find("wheelCollider").GetComponent<MeshFilter>().sharedMesh;

        float wheelRadius = wheelRad.bounds.size.x;

        Quaternion rotationR = Quaternion.LookRotation(transform.forward, transform.right);
        

        for (int a = 0; a < wheelsRotAxis.Length; a++) {
            GameObject wheel = new GameObject("wheelMesh");
            GameObject axis = new GameObject("axisMesh");

            wheel.layer = gameObject.layer;
            wheel.tag = gameObject.tag;
            axis.layer = gameObject.layer;
            axis.tag = gameObject.tag;

            initMesh(wheel, wheelMesh, defaultMaterial);
            initMesh(axis, wheelAxisMesh, defaultMaterial);

            wheel.transform.position = wheelsRotAxis[a].transform.position;
            wheel.transform.parent = wheelsRotAxis[a].transform;
            MeshCollider meshColl =  wheel.AddComponent<MeshCollider>();
            meshColl.sharedMesh = wheelColl;
            meshColl.convex = true;
            meshColl.material = defaultPhMat;
            axis.transform.position = wheelsRotAxis[a].transform.parent.position;
            axis.transform.parent = wheelsRotAxis[a].transform.parent;

            wheel.transform.rotation = rotationR;
            axis.transform.localRotation = wheel.transform.localRotation;

            GameObject collider = new GameObject("wheelCollider");

            collider.layer = gameObject.layer;
            collider.tag = gameObject.tag;

            collider.transform.parent = wheelsRotAxis[a].transform.parent;
            collider.transform.position = wheelsRotAxis[a].transform.position;

            wheelsCollider[a]= collider.AddComponent<WheelCollider>();
            wheelsCollider[a].mass = 10;
            wheelsCollider[a].radius = wheelRadius;
            wheelsCollider[a].wheelDampingRate = 0.2f;
            wheelsCollider[a].suspensionDistance = 0.01f;
            wheelsCollider[a].suspensionSpring = spring;
            wheelsCollider[a].forwardFriction = forwardFrCurve;
            wheelsCollider[a].sidewaysFriction = sideFrCurve;
        }

        GameObject[] tractionObjects = FindGameObjectInChildWithName(transform, "traction");
        wheelsTractionCollider = new WheelCollider[tractionObjects.Length];

        for (int a = 0; a < tractionObjects.Length; a++) {
            wheelsTractionCollider[a] = tractionObjects[a].GetComponentInChildren<WheelCollider>();
        }
    }


    // Update is called once per frame
    void Update () {
        if (isActive) {
            Vector3 positionTmp;
            Quaternion rotationTmp;

            for (int a = 0; a < 4; a++) {
                wheelsCollider[a].GetWorldPose(out positionTmp, out rotationTmp);
                wheelsRotAxis[a].transform.position = positionTmp;
                wheelsRotAxis[a].transform.rotation = rotationTmp;
            }
        }
    }
    public GameObject[] GetDirectionalAxis() {
        return wheelsDirAxis;
    }
    public WheelCollider[] GetWheelsColliders() {
        return wheelsCollider;
    }
    public WheelCollider[] GetWheelsTractionColliders() {
        return wheelsTractionCollider;
    }
    


    public static void initMesh(GameObject target, Mesh mesh, Material material) {
        target.AddComponent<MeshFilter>();
        target.AddComponent<MeshRenderer>();
        target.GetComponent<MeshRenderer>().sharedMaterial = material;
        target.GetComponent<MeshFilter>().mesh = mesh;
    }

    static public GameObject[] FindGameObjectInChildWithName(Transform parent, string name) {
        if (parent.childCount > 0) {
            List<GameObject> childList = new List<GameObject>();
            foreach (Transform tr in parent) {
                if (tr.name.Contains(name)) {
                    childList.Add(tr.gameObject);
                }
                if (tr.childCount > 0) {
                    foreach (GameObject obj in FindGameObjectInChildWithName(tr, name)) {
                        childList.Add(obj);
                    }
                }
            }
            return childList.ToArray();
        }
        else {
            return null;
        }
    }

    public bool IsCarGrounded() {
        bool isGr = false;
        foreach (WheelCollider wheel in wheelsCollider) {
            if (wheel.isGrounded) {
                isGr = true;
            }
        }
        return isGr;
    }

    public void destroyCar() {
        FindGameObjectInChildWithName(transform, "mesh")[0].GetComponent<MeshRenderer>().material = deadTexture;

        foreach(GameObject axis in wheelsDirAxis) {
            axis.transform.Find("axisMesh").GetComponent<MeshRenderer>().material = deadTexture;
        }

        foreach(GameObject wheels in wheelsRotAxis) {

            Transform mesh = wheels.transform.Find("wheelMesh");
            Rigidbody rb = mesh.gameObject.AddComponent<Rigidbody>();
            rb.mass = 10;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            mesh.GetComponent<MeshRenderer>().material = deadTexture;
            MeshCollider coll = mesh.GetComponent<MeshCollider>();
            coll.sharedMesh = mesh.GetComponent<MeshFilter>().sharedMesh;
            coll.convex = true;
        }

        foreach (WheelCollider wcoll in wheelsCollider) {
            Destroy(wcoll);
        }

        isActive = false;
        gameObject.SendMessage("SetActive", false);
    }
}
