using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHelper : MonoBehaviour
{
    [SerializeField] private Transform m_Target;

    void Update()
    {
        transform.position = Player.Instance.PlayerShip.ArrowPoint.position;

        transform.forward = m_Target.position - transform.position;
    }

    public void SetTarget(Transform target)
    {
        m_Target = target;
    }
}
