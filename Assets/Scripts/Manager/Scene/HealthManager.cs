using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Transform heartContainersParent;
    [SerializeField] private HeartPooling heartContainerPool;
    [SerializeField] private int actualHearts;
    
    private void Awake()
    {
        actualHearts = 2;
        InitializeHeartContainers();
    }
    
    private void InitializeHeartContainers()
    {
        if (actualHearts<=0) return;
        for (var i = 0; i < actualHearts; i++)
        {
            AddHeartContainer();
        }
    }

    public void AddHeartContainer()
    {
        
        var newHeartContainer = heartContainerPool.GetObject();
        newHeartContainer.transform.SetParent(heartContainersParent);
        var heartContainer = newHeartContainer.GetComponent<HeartContainer>();
        heartContainer.ID = heartContainersParent.childCount - 1;
        actualHearts++;
    }

    public void RemoveHeartContainer()
    {
        if (actualHearts <= 0) return;
        var lastChild = heartContainersParent.GetChild(heartContainersParent.childCount - 1);
        heartContainerPool.ReturnObject(lastChild.gameObject);
        actualHearts--;
    }
}
