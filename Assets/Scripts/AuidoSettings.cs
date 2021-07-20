using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuidoSettings : MonoBehaviour
{
    public static AuidoSettings Instance;

    [SerializeField] private Slider m_AudioSlider;
    public float AudioValue => m_AudioSlider.value;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
