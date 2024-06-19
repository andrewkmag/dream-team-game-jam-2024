using explorerJam.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private TextMeshProUGUI currentJarCountText;

    public int currentJarCount;

    private void OnEnable()
    {
        // Subscribe to the collectable collected event
        if (GameManager.Instance != null)
        {
            GameManager.OnUpdateJamCount += IncrementJarCount;
        }
    }

    private void OnDisable()
    {
        // Subscribe to the collectable collected event
        if (GameManager.Instance != null)
        {
            GameManager.OnUpdateJamCount += IncrementJarCount;
        }
    }

    private void Awake()
    {
        // Set the current jar count to 0
        currentJarCount = -4;
        UpdateJarCountText();
    }

    private void IncrementJarCount()
    {
        // Increment the current jar count
        currentJarCount += 1;

        // Update the current jar count text
        UpdateJarCountText();
    }

    private void UpdateJarCountText()
    {
        // Update the current jar count text
        currentJarCountText.text = currentJarCount.ToString();
    }

    public void HideHud()
    {
        // Hide the player HUD
        playerHUD.SetActive(false);
    }

    public void ShowHud()
    {
        // Show the player HUD
        playerHUD.SetActive(true);
    }
}
