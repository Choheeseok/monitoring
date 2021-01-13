using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 targetOffset;
    public float distance = 5.0f;
    public float maxDistance = 20;
    public float minDistance = .6f;
    public float xRotateSpeed = 1.0f;
    public float yRotateSpeed = 1.0f;
    public int yMinLimit = -90;
    public int yMaxLimit = 90;
    public int zoomRate = 40;
    public float panSpeed = 0.3f;
    public float zoomDampening = 0.5f;
 
    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;
 
    void Start() { Init(); }

    public void Init()
    {
        if (!target)
        {
            GameObject go = new GameObject("Cam Target");
            go.transform.position = new Vector3(0, 0, 0);
            target = go.transform;
        }
 
        distance = Vector3.Distance(transform.position, target.position);
        currentDistance = distance;
        desiredDistance = distance;
        
        position = transform.position;
        rotation = transform.rotation;
        desiredRotation = transform.rotation;
 
        xDeg = Vector3.Angle(Vector3.right, transform.right );
        yDeg = Vector3.Angle(Vector3.up, transform.up );
    }
    
    void Update()
    {
        // Orbit
        if (Input.GetMouseButton(1))
        //if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftControl))
        {
            xDeg += Input.GetAxis("Mouse X") * xRotateSpeed;
            yDeg -= Input.GetAxis("Mouse Y") * yRotateSpeed;
 
            ////////OrbitAngle
 
            //Clamp the vertical axis for the orbit
            yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
            // set camera rotation S
            desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);

            rotation = desiredRotation;
            transform.rotation = rotation;
            // rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
            //transform.rotation = rotation;
        }
        // oif middle mouse is selected, we pan by way of transforming the target in screenspace
        else if (Input.GetMouseButton(2))
        {
            //grab the rotation of the camera so we can move in a psuedo local XY space
            target.rotation = transform.rotation;
            target.Translate(Vector3.right * -Input.GetAxis("Mouse X") * panSpeed);
            target.Translate(transform.up * -Input.GetAxis("Mouse Y") * panSpeed, Space.World);
        }
 
        ////////Orbit Position
        
        // affect the desired Zoom distance if we roll the scrollwheel
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        //clamp the zoom min/max
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance
        //currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);
        currentDistance = desiredDistance;
        // calculate position based on the new currentDistance 
        position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
        transform.position = position;
    }
 
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
