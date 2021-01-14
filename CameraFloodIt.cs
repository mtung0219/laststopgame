using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFloodIt : MonoBehaviour
{
    public float speed;

    float cameraDistanceMax = 200f;
    float cameraDistanceMin = 5f;
    float cameraDistance = 10f;
    float scrollSpeed = 0.5f;

    public Camera cameraFreeWalk;
    public float zoomSpeed = 20f;
    public float minZoomFOV = 5f;
    public float maxSize;
    private float scrollData;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }

        scrollData = Input.GetAxis("Mouse ScrollWheel");

        if ((cameraFreeWalk.orthographicSize <= maxSize && scrollData > 0) || scrollData < 0)
        {
            cameraFreeWalk.orthographicSize += scrollData * zoomSpeed;
        }
    }
}
