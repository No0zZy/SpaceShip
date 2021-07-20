using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScenarioController : MonoBehaviour
{
    [SerializeField] private ScenarioState m_StateInitial;
    [SerializeField] private ScenarioState m_StateEnd;

    [SerializeField] private UnityEvent m_EventScenarioStarted;
    [SerializeField] private UnityEvent m_EventScenarioFinished;

    [SerializeField] private bool m_AutoStartScenario;

    private ScenarioState m_CurrentState;
    public ScenarioState CurrentState => m_CurrentState;

    [SerializeField] private Text m_MissionText;

    private void Start()
    {
        if (m_AutoStartScenario)
            StartScenario();
    }

    public void StartScenario()
    {
        m_CurrentState = m_StateInitial;
        SetMissionText();
        m_EventScenarioStarted?.Invoke();
    }

    public void StopScenario()
    {
        m_CurrentState = null;

        m_EventScenarioFinished?.Invoke();
    }

    private void Update()
    {
        if(m_CurrentState)
            UpdateStateSwitch();
    }

    private void UpdateStateSwitch()
    {
        if(m_CurrentState != null && m_CurrentState.IsStateFinished)
        {
            var next = m_CurrentState.NextState;

            m_CurrentState.BroadcastMessage("OnScenarioStateLeave", SendMessageOptions.DontRequireReceiver);

            m_CurrentState = next;

            SetMissionText();

            m_CurrentState?.BroadcastMessage("OnScenarioStateEnter", SendMessageOptions.DontRequireReceiver);

            if (!m_CurrentState)
                StopScenario();
            
        }
    }

    private void SetMissionText()
    {
        if (m_CurrentState)
            m_MissionText.text = m_CurrentState.StateText;
        else
            m_MissionText.text = "";
    }
}
