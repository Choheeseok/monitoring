using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public CameraController cameraController;
    public Transform left;
    public Transform right;
    public Transform front;
    public Transform back;
    public Transform top;
    
    public Transform movePanelButton;

    public Transform menuOriginPos;
    public Transform menuActivePos;
    public Transform menuPanel;

    public float moveSpeed;
    private bool moveMenuPanel;
    private bool moveMenuPanelBack;
    
    private bool rotationCompleted;
    private bool translateCompleted;
    private IEnumerator rotationRoutine;
    private IEnumerator positionRoutine;

    private Transform desiredTransform;
    public bool isMoving;
    
    void Start()
    {
        menuPanel.position = menuOriginPos.position;
    }

    void Update()
    {
        // move panel
        if (true == moveMenuPanel)
        {
            menuPanel.position = Vector3.Lerp(menuPanel.position, menuActivePos.position,
                moveSpeed * Time.deltaTime);
            if (Mathf.Abs(menuPanel.position.x - menuActivePos.position.x) < 1f)
            {
                moveMenuPanel = false;
                menuPanel.position = menuActivePos.position;
            }
        }

        if (true == moveMenuPanelBack)
        {
            menuPanel.position = Vector3.Lerp(menuPanel.position, menuOriginPos.position,
                moveSpeed * Time.deltaTime);
            
            // button appear
            if (Mathf.Abs(menuPanel.position.x - menuOriginPos.position.x) < 20f)
            {
                movePanelButton.gameObject.SetActive(true);
            }
            if (Mathf.Abs(menuPanel.position.x - menuOriginPos.position.x) < 1f)
            {
                moveMenuPanelBack = false;
                menuPanel.position = menuOriginPos.position;

                print("im hre");
                // animation end
                menuPanel.gameObject.SetActive(false);
                
            }
        }
        
        // move camera
        if (true == isMoving)
        {
            if (null == rotationRoutine)
            {
                rotationRoutine = InterpolateQuaternion(Camera.main.transform.rotation, desiredTransform.rotation, .5f);
                StartCoroutine(rotationRoutine);
            }

            if (null == positionRoutine)
            {
                positionRoutine = InterpolateVector3(Camera.main.transform.position, desiredTransform.position, .5f);
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

    public void MovePanel()
    {
        moveMenuPanelBack = false;
        moveMenuPanel = true;
        movePanelButton.gameObject.SetActive(false);
        menuPanel.gameObject.SetActive(true);
    }

    public void MovePanelBack()
    {
        moveMenuPanel = false;
        moveMenuPanelBack = true;
    }

    public void Left()
    {
        desiredTransform = left;
        isMoving = true;
    }

    public void Right()
    {
        desiredTransform = right;
        isMoving = true;
    }

    public void Front()
    {
        desiredTransform = front;
        isMoving = true;
    }

    public void Back()
    {
        desiredTransform = back;
        isMoving = true;
    }

    public void Top()
    {
        desiredTransform = top;
        isMoving = true;
    }
    
    private IEnumerator InterpolateQuaternion(Quaternion src, Quaternion dest, float time)
    {
        Quaternion source = new Quaternion(src.x,src.y,src.z,src.w);
        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            Camera.main.transform.rotation = Quaternion.Slerp(source, dest, (Time.time - startTime) / time);
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
