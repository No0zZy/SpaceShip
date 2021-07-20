using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    private Dictionary<int, Stack<GameObject>> m_PrefabPool;
    private void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        m_PrefabPool = new Dictionary<int, Stack<GameObject>>();
    }

    public GameObject Spawn(GameObject prefab)
    {
        var poolObj = prefab.GetComponent<PoolObject>();

        if (!poolObj)
            return null;

        Stack<GameObject> pool = null;
        m_PrefabPool.TryGetValue(poolObj.PrefabKey, out pool);

        if (pool == null)
        {
            pool = new Stack<GameObject>();
            m_PrefabPool[poolObj.PrefabKey] = pool;
        }

        if (pool.Count == 0)
        {
            return Instantiate(prefab);
        }

        var resued = pool.Pop();

        resued.SetActive(true);
        return resued;
    }

    public void Unspawn(GameObject gameObjectCopy)
    {
        var poolObj = gameObjectCopy.GetComponent<PoolObject>();

        if (!poolObj)
            return;

        Stack<GameObject> pool = null;
        m_PrefabPool.TryGetValue(poolObj.PrefabKey, out pool);

        gameObjectCopy.SetActive(false);
        pool.Push(gameObjectCopy);
    }
}
