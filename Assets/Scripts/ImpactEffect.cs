using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    [SerializeField] private float m_Lifetime;
    private float m_Timer;

    private void OnEnable()
    {
        m_Timer = 0;
    }
    private void Update()
    {
        m_Timer += Time.deltaTime;

        if (m_Timer > m_Lifetime)
            OnLifeTimeEnd();
    }

    private void OnLifeTimeEnd()
    {
        PoolManager.Instance.Unspawn(gameObject);
    }
}