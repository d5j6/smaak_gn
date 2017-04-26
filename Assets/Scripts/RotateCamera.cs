using NodeCanvas.DialogueTrees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Loosly based upon: http://answers.unity3d.com/questions/805630/how-can-%C4%B1-rotate-camera-with-touch.html
public class RotateCamera : MonoBehaviour
{
    private bool m_IsEnabled = true;
    private bool m_IsMoving = false;

    private Vector3 m_StartPoint;

    private float m_XAngle = 0.0f;
    private float m_YAngle = 0.0f;
    private float m_XAngleTemp = 0.0f;
    private float m_YAngleTemp = 0.0f;
    
    private void Start()
    {
        DialogueTree.OnDialogueStarted += OnDialogueStarted;
        DialogueTree.OnDialogueFinished += OnDialogueFinished;

    }

    private void OnDestroy()
    {
        DialogueTree.OnDialogueStarted -= OnDialogueStarted;
        DialogueTree.OnDialogueFinished -= OnDialogueFinished;
    }

    private void Update()
    {
        if (!m_IsEnabled)
            return;

        //Initialization our angles of camera
        Vector3 eulerAngles = this.transform.rotation.eulerAngles;
        m_XAngle = eulerAngles.y;
        m_YAngle = eulerAngles.x;

        #if UNITY_ANDROID && !UNITY_EDITOR
                HandleTouchInput();
        #else
                HandleMouseInput();
        #endif
    }

    private void OnDisable()
    {
        StopMoving();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            //Start touch
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                StartMoving(Input.GetTouch(0).position);
            }

            //Handle movement
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                HandleMoving(Input.GetTouch(0).position, true);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                StopMoving();
            }
        }
    }

    private void HandleMouseInput()
    {
        //Start click
        if (Input.GetMouseButtonDown(0))
        {
            StartMoving(Input.mousePosition);
        }

        //Handle movement
        if (Input.GetMouseButton(0))
        {
            HandleMoving(Input.mousePosition, false);
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopMoving();
        }
    }

    private void StartMoving(Vector3 position)
    {
        m_StartPoint = position;

        m_XAngleTemp = m_XAngle;
        m_YAngleTemp = m_YAngle;

        m_IsMoving = true;
    }

    private void HandleMoving(Vector3 position, bool inverse)
    {
        if (!m_IsMoving)
            return;

        Vector3 secondPoint = position;

        //Mainly, about rotate camera. For example, for Screen.width rotate on 180 degree
        float addedXangle = (secondPoint.x - m_StartPoint.x) * 180.0f / Screen.width;
        float addedYangle = (secondPoint.y - m_StartPoint.y) * 90.0f / Screen.height;

        if (inverse)
        {
            addedXangle *= -1.0f;
            addedYangle *= -1.0f;
        }

        m_XAngle = m_XAngleTemp + addedXangle;
        m_YAngle = m_YAngleTemp - addedYangle;

        //Rotate camera
        this.transform.rotation = Quaternion.Euler(m_YAngle, m_XAngle, 0.0f);
    }

    private void StopMoving()
    {
        m_IsMoving = false;
    }

    private void Enable()
    {
        m_IsEnabled = true;
    }

    private void Disable()
    {
        m_IsEnabled = false;
    }

    //Events
    private void OnDialogueStarted(DialogueTree tree)
    {
        Disable();
    }

    private void OnDialogueFinished(DialogueTree tree)
    {
        Enable();
    }
}
