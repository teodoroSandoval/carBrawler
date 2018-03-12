using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private LayerMask layerMask;
    public Transform targetCar;
    public float camDistance = 2f;
    public float camHeight = 1.2f;
    private Vector3 newPosition;
    public float interpolationFactor = 1.5f;
    void Start () {
        layerMask = (1 << LayerMask.NameToLayer("mapOnly"));
        newPosition = targetCar.position - targetCar.forward * camDistance;
        newPosition.y = targetCar.position.y + camHeight;
        transform.position = newPosition;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        newPosition = targetCar.position - targetCar.forward * camDistance;
        newPosition.y = targetCar.position.y + camHeight;

        RaycastHit hit;
        Debug.DrawRay(targetCar.position, newPosition - targetCar.position);
        if (Physics.Raycast(targetCar.position, newPosition - targetCar.position, out hit, Vector3.Distance(targetCar.position, newPosition), layerMask))
            transform.position = Vector3.Lerp(transform.position, hit.point, interpolationFactor * Time.deltaTime); // hit.point;
        else
            transform.position = Vector3.Lerp(transform.position, newPosition, interpolationFactor * Time.deltaTime);
        transform.LookAt(targetCar);
    }
}
