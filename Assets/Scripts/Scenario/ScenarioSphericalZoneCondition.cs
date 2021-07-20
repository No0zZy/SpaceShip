using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioSphericalZoneCondition : ScenarioCondition
{
    [SerializeField] private Transform m_Target;

    public void SetTarget(Transform tr)
    {
        m_Target = tr;
    }

    [SerializeField] private AIPointPatrol m_Zone;

    [SerializeField] private bool m_AutoSetPlayerShipAsTarget;

    private void Update()
    {
        if(IsConditionActive)
        {
            if (m_AutoSetPlayerShipAsTarget && m_Target == null)
            {
                SetTarget(Player.Instance.PlayerShip.transform);
            }    

            if((m_Target.position - m_Zone.transform.position).sqrMagnitude < m_Zone.Radius * m_Zone.Radius)
            {
                IsTriggered = true;
            }
        }
    }
}
