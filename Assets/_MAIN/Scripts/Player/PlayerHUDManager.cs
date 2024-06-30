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
    [SerializeField] private Image siblingImage;

    #region Constants

    private const float PERCENT_25=0.25f;
    private const float PERCENT_50=0.50f;
    private const float PERCENT_75=0.75f;
    private const float PERCENT_100=1.00f;

    #endregion
    
    private void OnEnable()
    {
        // Subscribe to the collectable collected event

        GameManager.OnUpdateJamCount += UpdateJars;
        GameManager.OnUpdateSibling += UpdateSiblingImage;
    }

    private void OnDisable()
    {
        // Subscribe to the collectable collected event
        GameManager.OnUpdateJamCount -= UpdateJars;
        GameManager.OnUpdateSibling -= UpdateSiblingImage;
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
        UpdateJarCountImage(collectedItems,maxItems);
    }

    private void UpdateJarCountText(int collected, int maxItems)
    {
        // Update the current jar count text
        currentJarCountText.text = $"{collected} / {maxItems}";
    }

    private void UpdateJarCountImage(int collected, int maxItems)
    {
        var percentage = (float)collected / maxItems;

        currentJarCountImage.sprite = percentage switch
        {
            < PERCENT_25 => SpriteLibrary.Instance.GetSprite("JamJar", "0"),
            < PERCENT_50 => SpriteLibrary.Instance.GetSprite("JamJar", "25"),
            < PERCENT_75 => SpriteLibrary.Instance.GetSprite("JamJar", "50"),
            _ => percentage < PERCENT_100 ? SpriteLibrary.Instance.GetSprite("JamJar", "75") :
                percentage >= PERCENT_100 ? SpriteLibrary.Instance.GetSprite("JamJar", "100") :
                currentJarCountImage.sprite
        };
    }
    
    private void UpdateSiblingImage()
    {
        siblingImage.sprite = SpriteLibrary.Instance.GetSprite("Sibling", GameManager.Instance.RequisiteAchieved ? "Unlocked" : "Locked");
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
