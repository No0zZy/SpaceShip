using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform m_Target;
    [SerializeField] private Transform m_TargetCenter;

    public void SetTargets(Transform target, Transform targetCenter)
    {
        m_Target = target;
        m_TargetCenter = targetCenter;
    }

    [SerializeField] private float m_InterpolationLinear;
    [SerializeField] private float m_InterpolationAngular;

    [SerializeField] private float m_ForwardObserveDistance;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, m_Target.position, Time.deltaTime * m_InterpolationLinear);

        Vector3 fw = m_TargetCenter.position + m_ForwardObserveDistance * m_TargetCenter.forward;
        transform.rotation = Quaternion.LookRotation(fw - transform.position, Vector3.Lerp(transform.up, m_Target.up, m_InterpolationAngular * Time.deltaTime));
    }
}
