using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioEventSender : ScenarioElement
{
    [SerializeField] private UnityEvent m_Event;

    public enum Mode
    {
        StateEnter,
        StateLeave
    }
    [SerializeField] private Mode m_Mode;

    protected override void OnScenarioStateEnter()
    {
        if (m_Mode == Mode.StateEnter)
            m_Event?.Invoke();
    }

    protected override void OnScenarioStateLeave()
    {
        if (m_Mode == Mode.StateLeave)
            m_Event?.Invoke();
    }
}
