using explorerJam.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private TextMeshProUGUI currentJarCountText;
    [SerializeField] private GameObject jarIcon0;
    [SerializeField] private GameObject jarIcon1;
    [SerializeField] private GameObject jarIcon2;
    [SerializeField] private GameObject jarIcon3;
    [SerializeField] private GameObject jarIcon4;

    public int currentJarCount;
    public int currentJarImageCount;

    private GameManager gameManager;

    private void OnEnable()
    {
        // Subscribe to the collectable collected event
        if (GameManager.Instance != null)
        {
            Debug.Log("Subscribing to jam count event");
            GameManager.OnUpdateJamCount += IncrementJarCount;
        } else
        {
            Debug.Log("GameManager is null");
            gameManager = FindObjectOfType<GameManager>();
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
        if (SceneManager.GetActiveScene().name == "Sweetsvylvania")
        {
            Debug.Log("Scene is Sweetsvylvania");
            currentJarCount = -4;
            currentJarImageCount = -4;
        }
        else
        {
            Debug.Log("Scene is not: " + SceneManager.GetActiveScene().name);
            currentJarCount = 0;
            currentJarImageCount = 0;

        }



        if (currentJarImageCount < 1)
        {
            jarIcon1.SetActive(false);
            jarIcon2.SetActive(false);
            jarIcon3.SetActive(false);
            jarIcon4.SetActive(false);
        }
        UpdateJarCountText();
        UpdateJarImageText();

    }

    private void IncrementJarCount()
    {
        // Increment the current jar count
        currentJarCount += 1;
        currentJarImageCount += 1;

        // Update the current jar count text
        UpdateJarCountText();
        UpdateJarImageText();
    }

    private void UpdateJarCountText()
    {
        // Update the current jar count text
        currentJarCountText.text = currentJarCount.ToString();
    }

    private void UpdateJarImageText()
    {
        Debug.Log("Updating jar image text = " + currentJarImageCount);
        // Update jar icons
        if (currentJarImageCount == 0)
        {
            Debug.Log("Jar 0");
            jarIcon0.SetActive(true);
            return;
        }
        else if (currentJarImageCount == 1)
        {
            Debug.Log("Jar 1");
            jarIcon1.SetActive(true);
            return;
        }
        else if (currentJarImageCount == 2)
        {
            Debug.Log("Jar 2");
            jarIcon2.SetActive(true);
            return;
        }
        else if (currentJarImageCount == 3)
        {
            Debug.Log("Jar 3");
            jarIcon3.SetActive(true);
        }
        else if (currentJarImageCount == 4)
        {
            Debug.Log("Jar 4");
            jarIcon4.SetActive(true);
        }
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
