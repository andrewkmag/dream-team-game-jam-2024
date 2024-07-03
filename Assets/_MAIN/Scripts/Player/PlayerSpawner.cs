using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab; // Reference to the player prefab
    [SerializeField] private Vector3 spawnPosition = new Vector3(-2.57f, 2.68f, 40.42f); // Default spawn position


    private void Awake()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        Debug.Log("Current Scene Index:" + SceneManager.GetActiveScene().buildIndex);
        // If build index is 3, spawn player at the specified position
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            spawnPosition = new Vector3(-2.57f, 2.68f, 40.42f);
            // Instantiate the player at the specified position with no rotation
            //Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        }

    }
}
