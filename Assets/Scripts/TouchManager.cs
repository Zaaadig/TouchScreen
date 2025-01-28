using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class TouchContactDataClass
{
    public Vector2 Position;
    public float Time;
}

[DefaultExecutionOrder(-1)]
public class TouchManager : MonoBehaviour
{
    [SerializeField] private InputActionReference m_onTouchContactInput;
    [SerializeField] private InputActionReference m_onTouchPositionInput;

    private static TouchManager m_instance;

    public static TouchManager Instance { get => m_instance; set => m_instance = value; }

    //Event
    public delegate void StartTouch(TouchContactDataClass contactData);
    public event StartTouch OnTouchStartEvent;
    public delegate void Endtouch(TouchContactDataClass contactData);
    public event Endtouch OnTouchEndEvent;

    private void Awake()
    {
        TouchSimulation.Enable();
        
        if(m_instance != null)
            Destroy(gameObject);
        m_instance = this;
    }

    private void OnEnable()
    {
        m_onTouchContactInput.action.started += OnTouchStart;
        m_onTouchContactInput.action.canceled += OnTouchEnd;
    }

    private void OnDisable()
    {
        m_onTouchContactInput.action.started -= OnTouchStart;
        m_onTouchContactInput.action.canceled -= OnTouchEnd;
    }

    private void OnTouchStart(InputAction.CallbackContext context)
    {
        TouchContactDataClass data = new TouchContactDataClass();
        data.Position = GetTouchWorldPosition();
        data.Time = (float)context.time;
        if (OnTouchStartEvent != null) OnTouchStartEvent(data);
        
        Debug.Log("Touch Start " + data.Time);
    }

    private void OnTouchEnd(InputAction.CallbackContext context)
    {
        TouchContactDataClass data = new TouchContactDataClass();
        data.Position = GetTouchWorldPosition();
        data.Time = (float)context.time;
        if(OnTouchEndEvent != null) OnTouchEndEvent(data);

        Debug.Log("Touch End " + data.Time);
    }

    public Vector2 GetTouchWorldPosition()
    {
        return ScreenToWorld(Camera.main, m_onTouchPositionInput.action.ReadValue<Vector2>());
    }

    private Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = -camera.transform.position.z;
        return camera.ScreenToWorldPoint(position);
    }
}
