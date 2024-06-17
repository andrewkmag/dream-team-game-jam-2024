using System.Collections.Generic;
using UnityEngine;

public class HeartPooling : MonoBehaviour
{
    [SerializeField] private GameObject heartContainer;
    [SerializeField] private int initialSize = 3;

    private Queue<GameObject> _pool;

    private void Awake()
    {
        _pool = new Queue<GameObject>();

        for (var i = 0; i < initialSize; i++)
        {
            var obj = Instantiate(heartContainer);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (_pool.Count > 0)
        {
            var obj = _pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            var obj = Instantiate(heartContainer);
            obj.SetActive(true);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }
}