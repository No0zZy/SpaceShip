using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioSetArrowTarget : ScenarioElement
{
    [SerializeField] private Transform m_ArrowTarget;
    [SerializeField] private ArrowHelper m_Arrow;

    protected override void OnScenarioStateEnter()
    {
        m_Arrow.SetTarget(m_ArrowTarget);
    }

    protected override void OnScenarioStateLeave()
    {

    }
}
