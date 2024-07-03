using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PooledShots : MonoBehaviour
{
    [SerializeField] private List<GameObject> pooledObjects;
    [SerializeField] private GameObject objectToPool;
    public int amountToPool;

    public static PooledShots SharedInstance { get; private set; }

    private const int INITIAL_ARRAY = 0;


    private void Awake()
    {
        if (SharedInstance == null)
        {
            SharedInstance = this;
            pooledObjects = new List<GameObject>();
            for (var i = INITIAL_ARRAY; i < amountToPool; i++)
            {
                var tmp = Instantiate(objectToPool, transform, true);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameManager.OnDeath += DeactivateAllPooled;
    }

    private void OnDisable()
    {
        GameManager.OnDeath += DeactivateAllPooled;
    }

    public GameObject GetPooledObject()
    {
        for (var i = INITIAL_ARRAY; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    public void DeactivateAllPooled()
    {
        foreach (var go in pooledObjects.Where(go => go.activeInHierarchy))
        {
            go.SetActive(false);
        }
    }
}