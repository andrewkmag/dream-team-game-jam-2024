using System.Collections.Generic;
using UnityEngine;

public class HeartPooling : MonoBehaviour
{
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    public static HeartPooling SharedInstance { get; private set; }

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        for(var i = 0; i < amountToPool; i++)
        {
            var tmp = Instantiate(objectToPool, transform, true);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
    
    public GameObject GetPooledObject()
    {
        for(var i = 0; i < amountToPool; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
    
    public GameObject GetPooledObjectToRemove()
    {
        for(var i = amountToPool - 1; i >= 0; i--)
        {
            if(pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}