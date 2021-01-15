using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.WSA;

public class NavController : MonoBehaviour
{
    public CameraController cameraController;
    public Transform targetObject;
    public Transform left;
    public Transform right;
    public Transform up;
    public Transform down;
    public Transform front;
    public Transform back;


    public bool isMoving;
    private Transform desiredTransform;
    private HashSet<Transform> directions;

    private bool rotationCompleted;
    private bool translateCompleted;
    private IEnumerator rotationRoutine;
    private IEnumerator positionRoutine;
    void Start()
    {
        isMoving = false;
        rotationCompleted = false;
        translateCompleted = false;
        rotationRoutine = null;
        positionRoutine = null;
        directions = new HashSet<Transform>()
        {
            left, right, up, down, front, back
        };
        foreach (var t in directions)
        {
            t.LookAt(transform);
        }
    }
    void Update()
    {
        if (false == isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    if (directions.Contains(hit.transform))
                    {
                        isMoving = true;
                        desiredTransform = hit.transform;
                    }
                }
            }
        }

        if (true == isMoving )
        {
            if (null == rotationRoutine)
            {
                Quaternion desiredRotation = desiredTransform.rotation;
                rotationRoutine = InterpolateQuaternion(Camera.main.transform.rotation, desiredRotation, .5f);
                StartCoroutine(rotationRoutine);
            }

            if (null == positionRoutine)
            {
                Vector3 desiredPosition = targetObject.position - desiredTransform.forward * 10.0f;
                desiredPosition.y += 2.0f;
                positionRoutine = InterpolateVector3(Camera.main.transform.position, desiredPosition, .5f);
                StartCoroutine(positionRoutine);
            }
            
            if (true == translateCompleted && true == rotationCompleted)
            {
                isMoving = false;
                translateCompleted = false;
                rotationCompleted = false;
                rotationRoutine = null;
                positionRoutine = null;
                desiredTransform = null;
            }
        }
    }

    private IEnumerator InterpolateQuaternion(Quaternion src, Quaternion dest, float time)
    {
        Quaternion source = new Quaternion(src.x,src.y,src.z,src.w);
        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            Camera.main.transform.rotation = Quaternion.Lerp(source, dest, (Time.time-startTime)/time);
            cameraController.xDeg = Camera.main.transform.rotation.eulerAngles.y;
            cameraController.yDeg = Camera.main.transform.rotation.eulerAngles.x;
            yield return null;
        }
        Camera.main.transform.rotation = dest;
        cameraController.xDeg = Camera.main.transform.rotation.eulerAngles.y;
        cameraController.yDeg = Camera.main.transform.rotation.eulerAngles.x;
        rotationCompleted = true;
        yield return null;
    }
    
    private IEnumerator InterpolateVector3(Vector3 src, Vector3 dest, float time)
    {
        Vector3 source = new Vector3(src.x,src.y,src.z);
        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            Camera.main.transform.position = Vector3.Lerp(source, dest, (Time.time-startTime)/time);
            yield return null;
        }
        Camera.main.transform.position = dest;
        translateCompleted = true;
        yield return null;
    }
}
