using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotation : MonoBehaviour
{
    [SerializeField] private float m_Sens;
    private float limit = 90;
    private float X, Y;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            X = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * m_Sens;
            Y += Input.GetAxis("Mouse Y") * m_Sens;
            Y = Mathf.Clamp(Y, -limit, limit);
            transform.localEulerAngles = new Vector3(-Y, X, 0);
        }
    }
}
