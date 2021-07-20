using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTextureOffset : MonoBehaviour
{

    [SerializeField] private MeshRenderer m_Target;

    [SerializeField] private float m_Speed;


    private Material m_Material;

    private void Start()
    {
        m_Material = m_Target.material;
    }


    private void Update()
    {
        m_Material.mainTextureOffset += new Vector2(m_Speed * Time.deltaTime, 0);
    }
}
