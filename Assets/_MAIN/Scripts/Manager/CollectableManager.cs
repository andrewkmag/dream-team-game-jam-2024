using System;
using UnityEngine;

namespace explorerJam.Managers
{
    public class CollectableManager : MonoBehaviour
    {
        public static CollectableManager Instance;

        public event Action OnCollectableCollected;

        private void Awake()
        {
            // Ensure there's only one instance of the CollectableManager
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void CollectableCollected()
        {
            OnCollectableCollected?.Invoke();
        }
    }
}