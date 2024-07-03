using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject mapPanel;

    private void Start()
    {
        mapPanel.SetActive(false);
    }

    public void ToggleMap()
    {         
        mapPanel.SetActive(true);
    }
}
