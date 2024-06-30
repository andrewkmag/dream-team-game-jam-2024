using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    #region Fields

    [Header("Health variables")] [SerializeField]
    private Transform heartContainersParent;

    [SerializeField] private int startingContainers;

    [Header("Debug")] [SerializeField] private bool invincible;

    [Header("Readonly")] [SerializeField] private int maxContainers;
    [SerializeField] private int currentContainers;
    [SerializeField] private int currentHealth;
    [SerializeField] private HeartContainer[] pooledHealthContainers;
    [SerializeField] private bool isdead;

    #endregion

    #region Properties

    public static HealthManager Instance { get; private set; }

    #endregion

    #region Constants

    private const int MIN_CONTAINERS = 1;
    private const int MIN_HEALTH = 1;
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
        KillBox.OnDeath += DamageAll;
    }

    private void OnDisable()
    {
        Shoot.OnPlayerHit -= TakeDamage;
        KillBox.OnDeath -= DamageAll;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
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
        if (currentContainers < startingContainers)
            currentContainers = startingContainers;
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
        if (invincible) return;
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
    
    public void DamageAll()
    {
        if (invincible) return;
        if (isdead) return;
        currentHealth = MIN_HEALTH;
        UpdateHealth();
        Kill();
    }

    public void HealAll()
    {
        if (isdead) return;
        if (currentHealth >= currentContainers) return;
        currentHealth = currentContainers;
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