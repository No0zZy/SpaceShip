using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class PoolObject : MonoBehaviour
{
    [SerializeField] private int m_PrefabKey;
    public int PrefabKey => m_PrefabKey;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (m_PrefabKey == 0)
        {
            string path = AssetDatabase.GetAssetPath(gameObject);
            m_PrefabKey = path.GetHashCode();
        }
    }
#endif
}
