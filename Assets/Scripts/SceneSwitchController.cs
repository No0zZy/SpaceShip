using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitchController : MonoBehaviour
{
    public static readonly string SceneSwitchNickname = "scene_switch";

    public static string TargetScene { get; private set; }
    public static string TargetGate { get; private set; }

    public static void SwitchSystem(string system, string gate)
    {
        TargetScene = system;
        TargetGate = gate;

        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneSwitchNickname);
    }

    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(TargetScene);
    }
}
