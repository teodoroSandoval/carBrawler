using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAssembly : MonoBehaviour {

    public float throwForce = 2;

    private bool isActive = false;
    private bool isOpen = false;
    public float speed = 700;
    public float hspeed = 1500;
    // Use this for initialization
    public GameObject[] armsA;
    public GameObject[] armsB;
    private float lastPositionY;


    private Vector3[] rotsArmsA;
    private Vector3[] rotsArmsB;

    public Vector3[] targRotsArmsA;
    public Vector3[] targRotsArmsB;

    private GameObject[] helices;

    public float normalDrag = 0;
    public float flyDrag = 10f;

    void Start () {

        helices = CarSetup.FindGameObjectInChildWithName(transform, "helice");

        if (armsA != null)
            armsA = CarSetup.FindGameObjectInChildWithName(transform, "armA");
        if (armsB != null)
            armsB = CarSetup.FindGameObjectInChildWithName(transform, "armB");

        rotsArmsA = new Vector3[armsA.Length];
        rotsArmsB = new Vector3[armsB.Length];



        for (int a = 0; a < armsA.Length; a++) {
            rotsArmsA[a] =  armsA[a].transform.rotation.eulerAngles;
            
        }
        for (int a = 0; a < armsB.Length; a++) {
            rotsArmsB[a] = armsB[a].transform.rotation.eulerAngles;
        }

        if (targRotsArmsA == null) {
            for (int a = 0; a < rotsArmsA.Length; a++) 
                targRotsArmsA[a] = rotsArmsA[a];
            
        }
        if (targRotsArmsB == null) {
            for (int a = 0; a < rotsArmsB.Length; a++)
                targRotsArmsB[a] = rotsArmsB[a];
            
        }

        lastPositionY = transform.position.y;
        GetComponent<Rigidbody>().AddForce(Vector3.up* throwForce, ForceMode.Impulse);

	}

    // Update is called once per frame
    void Update() {

        if (isActive) {

            if (isOpen){
                foreach(GameObject helice in helices) {
                    helice.transform.localRotation *= Quaternion.AngleAxis(hspeed*Time.deltaTime,Vector3.up);
                    
                }
            }
            else {
                for (int a = 0; a < armsA.Length; a++) {
                    if (armsA[a].transform.rotation != Quaternion.Euler(targRotsArmsA[a])) {
                        armsA[a].transform.rotation = Quaternion.RotateTowards(armsA[a].transform.rotation, Quaternion.Euler(targRotsArmsA[a]), speed * Time.deltaTime);
                        isOpen = false;
                    }
                    else {
                        isOpen = true;
                    }
                }

                for (int a = 0; a < armsB.Length; a++) {
                    if (armsB[a].transform.rotation != Quaternion.Euler(targRotsArmsB[a])) {
                        armsB[a].transform.rotation = Quaternion.RotateTowards(armsB[a].transform.rotation, Quaternion.Euler(targRotsArmsB[a]), speed * Time.deltaTime);
                        isOpen = false;
                    }
                    else {
                        isOpen = true;
                    }
                }

                if (isOpen) {
                    transform.SendMessage("SetActive", true);
                    GetComponent<Rigidbody>().drag = flyDrag;
                    Debug.Log("drone totaly deployd");
                }
                    
            }


        }
        else {

            if (transform.position.y < lastPositionY) {
                isActive = true;
                Debug.Log("drone activated");
            }
        }
        lastPositionY = transform.position.y;
    }

    public void SetActive(bool activeState) {
        isActive = activeState;
    }
}
