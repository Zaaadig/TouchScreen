using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private float m_minDistance = 0.5f;
    [SerializeField] private float m_maxTime = 0.5f;
    private TouchContactDataClass m_startContactData;
    private TouchContactDataClass m_endContactData;
    private void OnEnable()
    {
        TouchManager.Instance.OnTouchStartEvent += OnSwipeStart;
        TouchManager.Instance.OnTouchEndEvent += OnSwipeEnd;
    }
    private void OnDisable()
    {
        TouchManager.Instance.OnTouchStartEvent -= OnSwipeStart;
        TouchManager.Instance.OnTouchEndEvent -= OnSwipeEnd;
    }

    private void OnSwipeStart(TouchContactDataClass contactData)
    {
        m_startContactData = contactData;
    }

    private void OnSwipeEnd(TouchContactDataClass contactData)
    {
        m_endContactData = contactData;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        float distance = Vector2.Distance(m_startContactData.Position, m_endContactData.Position);
        float duration = m_endContactData.Time - m_startContactData.Time;

        if(distance >= m_minDistance && duration <= m_maxTime) 
        {
            Debug.DrawLine(m_startContactData.Position, m_endContactData.Position, Color.green, 2);
        }
    }
}
