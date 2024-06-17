using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContextualUIManager : MonoBehaviour
{
    private static ContextualUIManager _instance;
    
    [SerializeField] private GameObject ContextualTextPanel;
    [SerializeField] private TextMeshProUGUI ContextualText;
    
    [SerializeField] private GameObject ContextualButtonPanel;
    [SerializeField] private TextMeshProUGUI ContextualButtonTitle;
    [SerializeField] private TextMeshProUGUI ContextualButtonText;
    
    [SerializeField] private GameObject ContextualOptionPanel;
    [SerializeField] private TextMeshProUGUI ContextualOptionTitle;
    [SerializeField] private TextMeshProUGUI ContextualOptionbtn1Text;
    [SerializeField] private TextMeshProUGUI ContextualOptionbtn2Text;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        hidePanels();
    }

    private void hidePanels()
    {
        if (ContextualTextPanel.activeInHierarchy)
        {
            ContextualTextPanel.SetActive(false);    
        }
        if (ContextualTextPanel.activeInHierarchy)
        {
            ContextualButtonPanel.SetActive(false);  
        }
        if (ContextualOptionPanel.activeInHierarchy)
        {
            ContextualOptionPanel.SetActive(false);  
        }
    }

    public void ShowContextualText(string context)
    {
        ContextualTextPanel.SetActive(true);
        ContextualText.text = context;
    }
    
    public void HideContextualText()
    {
        ContextualTextPanel.SetActive(false);
        ContextualText.text = "";
    }
}
