using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_Prefabs;

    [SerializeField] private Transform m_Container;

    [SerializeField] private int m_NumInstances;
    [SerializeField] private float m_Radius;

    [SerializeField] private float m_Scale;
    [SerializeField] private bool m_RandomizeScale;
    [SerializeField] private float m_MinScale;
    [SerializeField] private float m_MaxScale;

    // TODO: scaling
    // Generator seed

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        for(int i = 0; i< m_NumInstances; i++)
        {
            var quad = Instantiate(m_Prefabs[Random.Range(0,m_Prefabs.Count)], m_Container);

            quad.transform.position = m_Container.position + Random.onUnitSphere * m_Radius;
            quad.transform.LookAt(m_Container);
            quad.transform.forward = -quad.transform.forward;

            quad.transform.Rotate(Vector3.forward * Random.Range(-180, 180), Space.Self);

            if(m_RandomizeScale)
                m_Scale  = Random.Range(m_MinScale, m_MaxScale);
            quad.transform.localScale = new Vector3(m_Scale, m_Scale, m_Scale);
        }
    }
}
