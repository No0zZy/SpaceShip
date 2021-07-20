using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCamera : MonoBehaviour
{
    [SerializeField] private Transform m_Camera;

    [SerializeField] private float m_Scale;

    private void Update()
    {
        transform.rotation = m_Camera.rotation;

        transform.position = m_Camera.position * m_Scale;
    }
}
