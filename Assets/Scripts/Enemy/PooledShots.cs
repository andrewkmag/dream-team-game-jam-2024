using System.Collections;
using System.Collections.Generic;
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
}