using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Transform carUI;
    public float xRotateSpeed;
    public float yRotateSpeed;
    public int zoomSpeed;
    public float panSpeed;
    public float maxWidth;
    public float maxHeight;

    public float xDeg = 0.0f;
    public float yDeg = 0.0f;
    private int yMinLimit = -90;
    private int yMaxLimit = 90;
    private NavController nav;
    
    void Awake()
    {
        transform.position = new Vector3(0,5,10);
        transform.rotation = Quaternion.Euler(30,180,0);
        carUI.eulerAngles = target.eulerAngles;
    }
    void Start() { Init(); }

    void Update()
    {
        if (true == nav.isMoving)
            return;
        
        if (Input.GetMouseButton(1))
        //if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftControl))
        {
            xDeg += Input.GetAxis("Mouse X") * xRotateSpeed;
            yDeg -= Input.GetAxis("Mouse Y") * yRotateSpeed;

            yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
            transform.rotation = Quaternion.Euler(yDeg, xDeg, 0);
        }
        if (Input.GetMouseButton(2))
        {
            transform.Translate(Vector3.right * -Input.GetAxis("Mouse X") * panSpeed);
            transform.Translate(Vector3.up * -Input.GetAxis("Mouse Y") * panSpeed);
        }
        
        transform.Translate(transform.forward * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed,Space.World);

        Vector3 pos = new Vector3(Screen.width * 0.9f, Screen.height * 0.9f, 10);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -maxWidth, maxWidth),
            Mathf.Clamp(transform.position.y, 0, maxHeight),
            Mathf.Clamp(transform.position.z, -maxWidth, maxWidth));
        carUI.position = Camera.main.ScreenToWorldPoint(pos);
        carUI.eulerAngles = target.eulerAngles;
    }
 
    private void Init()
    {
        nav = carUI.GetComponent<NavController>();

        xDeg = Vector3.Angle(Vector3.right, transform.right );
        yDeg = Vector3.Angle(Vector3.up, transform.up );
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
