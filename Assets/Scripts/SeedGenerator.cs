using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGenerator : MonoBehaviour
{
    [SerializeField] private int m_Seed;
    [SerializeField] private bool m_RandomizeSeed;

    void Start()
    {
        if (m_RandomizeSeed)
            m_Seed = Random.Range(0, 99999);
        Random.InitState(m_Seed);
    }
}
