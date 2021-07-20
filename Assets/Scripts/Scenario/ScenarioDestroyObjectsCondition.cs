using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioDestroyObjectsCondition : ScenarioCondition
{
    [SerializeField] private Destructible[] m_TargetsToDestroy;

    private int m_NumTargetsDead;

    private void Start()
    {
        m_NumTargetsDead = 0;
        foreach (var v in m_TargetsToDestroy)
            v.Death += ApplyDeath;
    }

    private void ApplyDeath()
    {
        m_NumTargetsDead++;
        CheckCondition();
    }

    private void CheckCondition()
    {
        if (IsConditionActive)
        {
            if (m_NumTargetsDead == m_TargetsToDestroy.Length)
                IsTriggered = true;
        }
    }
}
