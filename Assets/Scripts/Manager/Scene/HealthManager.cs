using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private Transform heartContainersParent;
    [SerializeField] private int maxContainers;
    [SerializeField] private int currentContainers;
    [SerializeField] private int currentHealth;
    [SerializeField] private HeartContainer[] pooledHealthContainers;
    [SerializeField] private bool invincible;
    [SerializeField] private bool isdead;

    #endregion

    #region Constants

    private const int MIN_CONTAINERS = 3;
    private const int MIN_HEALTH = 3;
    private const int FIRST_INDEX = 0;
    private const int NO_HEALTH = 0;

    #endregion

    #region Events

    public delegate void Action();

    public static event Action OnDeath;
    public static event Action OnRespawn;


    #endregion

    #region UnityMethods

    private void OnEnable()
    {
        Shoot.OnPlayerHit += TakeDamage;
    }

    private void OnDisable()
    {
        Shoot.OnPlayerHit -= TakeDamage;
    }

    private void Start()
    {
        maxContainers = HeartPooling.SharedInstance.amountToPool;
        InitializeHeartContainers();
        pooledHealthContainers = HeartPooling.SharedInstance.GetPooledArray();
        currentHealth = currentContainers;
    }

    #endregion

    #region Methods

    private void InitializeHeartContainers()
    {
        if (currentContainers < MIN_CONTAINERS)
            currentContainers = MIN_CONTAINERS;
        for (var i = FIRST_INDEX; i < currentContainers; i++)
        {
            AddHeartPooled();
        }
    }

    private void AddHeartPooled()
    {
        if (currentContainers >= maxContainers) return;
        var go = HeartPooling.SharedInstance.GetPooledObject();
        if (go == null) return;
        go.transform.SetParent(heartContainersParent);
        go.SetActive(true);
        currentHealth++;
        UpdateHealth();
    }

    public void AddHeartContainer()
    {
        if (isdead) return;
        if (currentContainers >= maxContainers) return;
        var go = HeartPooling.SharedInstance.GetPooledObject();
        if (go == null) return;
        go.transform.SetParent(heartContainersParent);
        go.SetActive(true);
        currentContainers++;
        UpdateHealth();
    }

    public void TakeDamage()
    {
        if(invincible) return;
        if (isdead) return;
        if (currentHealth <= MIN_HEALTH)
        {
            Kill();
            return;
        }

        currentHealth--;
        UpdateHealth();
    }

    public void Heal()
    {
        if (isdead) return;
        if (currentHealth >= currentContainers) return;
        currentHealth++;
        UpdateHealth();
    }
    
    public void Kill()
    {
        if (isdead) return;
        isdead = true;
        OnDeath?.Invoke();
        currentHealth = NO_HEALTH;
        UpdateHealth();
    }
    
    public void Respawn()
    {
        if (!isdead) return;
        isdead = false;
        OnRespawn?.Invoke();
        HealAll();
    }
    
    public void HealAll()
    {
        if (isdead) return;
        if (currentHealth >= currentContainers) return;
        currentHealth=currentContainers;
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        for (var index = FIRST_INDEX; index < pooledHealthContainers.Length; index++)
        {
            if (index < currentHealth)
            {
                pooledHealthContainers[index].Healed();
            }
            else
            {
                pooledHealthContainers[index].Damaged();
            }
        }
    }

    public void RemoveHeartContainer()
    {
        if (isdead) return;
        if (currentContainers <= MIN_CONTAINERS) return;
        var go = HeartPooling.SharedInstance.GetPooledObjectToRemove();
        if (go == null) return;
        go.SetActive(false);
        currentContainers--;
        if (currentHealth <= currentContainers) return;
        currentHealth = currentContainers;
        UpdateHealth();
    }

    #endregion
}