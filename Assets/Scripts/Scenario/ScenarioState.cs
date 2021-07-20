using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioState : MonoBehaviour
{
    private ScenarioCondition[] m_Conditions;

    [SerializeField] private string m_StateText;
    public string StateText => m_StateText;

    private void Start()
    {
        m_Conditions = GetComponentsInChildren<ScenarioCondition>();
    }

    [SerializeField] private ScenarioState m_NextState;
    public ScenarioState NextState => m_NextState;

    public bool IsStateFinished
    {
        get
        {
            if (m_Conditions == null || m_Conditions.Length == 0)
                return true;

            foreach(var v in m_Conditions)
            {
                if (!v.IsTriggered)
                    return false;
            }

            return true;
        }
    }
}
