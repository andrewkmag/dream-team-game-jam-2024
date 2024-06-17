using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeartPooling : MonoBehaviour
{
    #region Fields

    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    #endregion

    #region Properties

    public static HeartPooling SharedInstance { get; private set; }

    #endregion

    #region UnityMethods

    private void Awake()
    {
        SharedInstance = this;
        pooledObjects = new List<GameObject>();
        for (var i = 0; i < amountToPool; i++)
        {
            var tmp = Instantiate(objectToPool, transform, true);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    #endregion

    #region Methods

    public HeartContainer[] GetPooledArray()
    {
        Debug.Log($"Oscar {pooledObjects.Count}");
        var pooled = pooledObjects.ToArray();
        Debug.Log($"Oscar {pooled.Length}");
        var pooledHCs = new List<HeartContainer>(pooled.Length);
        pooledHCs.AddRange(pooled.Select(po => po.GetComponent<HeartContainer>()));
        Debug.Log($"Oscar {pooledHCs.Count}");
        return pooledHCs.ToArray();
    }

    public GameObject GetPooledObject()
    {
        for (var i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }

    public GameObject GetPooledObjectToRemove()
    {
        for (var i = amountToPool - 1; i >= 0; i--)
        {
            if (pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }

    #endregion
}