using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Transform heartContainersParent;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    private const int MIN_HEALTH=1;
    private const int FIRST_INDEX=0;
    
    private void Start()
    {
        if (maxHealth > HeartPooling.SharedInstance.amountToPool)
            maxHealth = HeartPooling.SharedInstance.amountToPool;
        InitializeHeartContainers();
    }
    
    private void InitializeHeartContainers()
    {
        if (currentHealth < MIN_HEALTH)
            currentHealth = MIN_HEALTH;
        for (var i = FIRST_INDEX; i < currentHealth; i++)
        {
            AddHeartPooled();
        }
    }

    private void AddHeartPooled()
    {
        if (currentHealth>=maxHealth) return;
        var go = HeartPooling.SharedInstance.GetPooledObject();
        if (go == null) return;
        go.transform.SetParent(heartContainersParent);
        go.SetActive(true);
    }

    public void AddHeartContainer()
    {
        if (currentHealth>=maxHealth) return;
        var go = HeartPooling.SharedInstance.GetPooledObject();
        if (go == null) return;
        go.transform.SetParent(heartContainersParent);
        go.SetActive(true);
        currentHealth++;
    }

    public void RemoveHeartContainer()
    {
        if (currentHealth<=MIN_HEALTH) return;
        var go = HeartPooling.SharedInstance.GetPooledObjectToRemove();
        if (go == null) return;
        go.SetActive(false);
        currentHealth--;
    }
}
