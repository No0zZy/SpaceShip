using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScenarioElement : MonoBehaviour
{
    protected abstract void OnScenarioStateEnter();
    protected abstract void OnScenarioStateLeave();
}
