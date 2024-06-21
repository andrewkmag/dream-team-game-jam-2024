using System;
using explorerJam.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private TextMeshProUGUI currentJarCountText;
    [SerializeField] private Image currentJarCountImage;

    #region Constants

    private const int PERCENT_0=0;
    private const int PERCENT_25=25;
    private const int PERCENT_50=50;
    private const int PERCENT_75=75;
    private const int PERCENT_100=100;

    #endregion
    
    private void OnEnable()
    {
        // Subscribe to the collectable collected event

        GameManager.OnUpdateJamCount += UpdateJars;
    }

    private void OnDisable()
    {
        // Subscribe to the collectable collected event
        GameManager.OnUpdateJamCount -= UpdateJars;
    }

    private void Start()
    {
        UpdateJars();
    }

    private void UpdateJars()
    {
        // Update the current jar count
        var maxItems = GameManager.Instance.MaxItemsRequired;
        var collectedItems = maxItems-GameManager.Instance.RequiredItemsRemaining;

        // Update the current jar count text
        UpdateJarCountText(collectedItems,maxItems);
    }

    private void UpdateJarCountText(int collected, int maxItems)
    {
        // Update the current jar count text
        currentJarCountText.text = $"{collected} / {maxItems}";
    }

    public void UpdateImage(int collected, int maxItems)
    {
        var percentage = (float)collected / maxItems;
        currentJarCountImage.sprite = percentage switch
        {
            <= PERCENT_0 => SpriteLibrary.Instance.GetSprite("JamJar", "0"),
            <= PERCENT_25 => SpriteLibrary.Instance.GetSprite("JamJar", "25"),
            <= PERCENT_50 => SpriteLibrary.Instance.GetSprite("JamJar", "50"),
            <= PERCENT_75 => SpriteLibrary.Instance.GetSprite("JamJar", "75"),
            >= PERCENT_100 => SpriteLibrary.Instance.GetSprite("JamJar", "100"),
            _ => currentJarCountImage.sprite
        };
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
