using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HealthModule))]
public class SelfSlider : MonoBehaviour {

    public Canvas canvas;
    private Slider slider;
    private RectTransform rect;
    private RectTransform CanvasRect;
    public Vector3 offset = Vector3.zero;
    private Camera camera;
    
	// Use this for initialization
	void Start () {
        camera = GameObject.Find("Camera_01").GetComponent<Camera>();
        //rect = GetComponent<HealthModule>().healthSlider.GetComponent<RectTransform>();
        CanvasRect = canvas.GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.
        if(Vector3.Angle(camera.transform.forward,transform.position - camera.transform.position) < 90) { 
            Vector2 ViewportPosition = camera.WorldToViewportPoint(transform.position + offset);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

            //now you can set the position of the ui element
            rect.anchoredPosition = WorldObject_ScreenPosition;
        }
    }
}
