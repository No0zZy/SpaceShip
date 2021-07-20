using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioObjectActivator : ScenarioElement
{
    public enum Mode
    {
        ActivateOnStart,
        DeactivateOnExit
    }

    [SerializeField] private Mode m_Mode;
    [SerializeField] private GameObject[] m_Objects;

    protected override void OnScenarioStateEnter()
    {
        foreach(var v in m_Objects)
        {
            if (m_Mode == Mode.ActivateOnStart)
                v.SetActive(true);
        }    
    }

    protected override void OnScenarioStateLeave()
    {
        foreach (var v in m_Objects)
        {
            if (m_Mode == Mode.DeactivateOnExit)
                v.SetActive(false);
        }
    }
}
