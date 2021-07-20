using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioConditionOnDestroy : ScenarioCondition
{
    [SerializeField] private Destructible m_Target;

    private void Start()
    {
        m_Target.OnDestroyed.AddListener(OnTargetDestroyed);
    }

    private void OnTargetDestroyed()
    {
        m_Target.OnDestroyed.RemoveListener(OnTargetDestroyed);

        IsTriggered = true;
    }
}
