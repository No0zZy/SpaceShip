using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScenarioCondition : ScenarioElement
{
    public bool IsTriggered { get; protected set; }
    public bool IsConditionActive { get; private set; }
    protected override void OnScenarioStateEnter()
    {
        IsConditionActive = true;
    }

    protected override void OnScenarioStateLeave()
    {
        IsTriggered = false;

        IsConditionActive = false;
    }
}
