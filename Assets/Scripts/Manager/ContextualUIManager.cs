using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContextualUIManager : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private GameObject ContextualTextPanel;
    [SerializeField] private TextMeshProUGUI ContextualText;

    [SerializeField] private GameObject ContextualButtonPanel;
    [SerializeField] private TextMeshProUGUI ContextualButtonTitle;
    [SerializeField] private Button ContextualButton;
    [SerializeField] private TextMeshProUGUI ContextualButtonText;

    [SerializeField] private GameObject ContextualOptionPanel;
    [SerializeField] private TextMeshProUGUI ContextualOptionTitle;
    [SerializeField] private TextMeshProUGUI ContextualOptionbtn1Text;
    [SerializeField] private Button ContextualButtonOp1;
    [SerializeField] private TextMeshProUGUI ContextualOptionbtn2Text;
    [SerializeField] private Button ContextualButtonOp2;

    private bool isCursorVisible = false;
    #endregion

    #region PROPERTIES

    public static ContextualUIManager Instace { get; private set; }

    #endregion

    #region CONSTANTS

    private const int READONLY_OPTIONS_NUMBER = 2;
    private const int FIRST_BUTTON = 0;
    private const int SECOND_BUTTON = 1;

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        if (Instace == null)
        {
            Instace = this;
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

    #endregion

    #region METHODS

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

    public Button ShowContextualButton(string context, string button)
    {
        ContextualButtonPanel.SetActive(true);
        ContextualButtonTitle.text = context;
        ContextualButtonText.text = button;
        ContextualButton.onClick.RemoveAllListeners();
        ContextualButton.onClick.AddListener(() => HideContextualButton());
        ShowCursor();
        return ContextualButton;
    }

    public void HideContextualButton()
    {
        ContextualButtonPanel.SetActive(false);
        ContextualButtonTitle.text = "";
        ContextualButtonText.text = "";
        HideCursor();
    }

    public Button[] ShowContextualOption(string context, string button1, string button2)
    {
        ContextualOptionPanel.SetActive(true);
        ContextualOptionTitle.text = context;
        ContextualOptionbtn1Text.text = button1;
        ContextualOptionbtn2Text.text = button2;
        var aux = new Button[READONLY_OPTIONS_NUMBER];
        aux[FIRST_BUTTON] = ContextualButtonOp1;
        aux[SECOND_BUTTON] = ContextualButtonOp2;
        ContextualButtonOp1.onClick.RemoveAllListeners();
        ContextualButtonOp1.onClick.AddListener(HideContextualOption);
        ContextualButtonOp2.onClick.RemoveAllListeners();
        ContextualButtonOp2.onClick.AddListener(HideContextualOption);
        ShowCursor();
        return aux;
    }

    public void HideContextualOption()
    {
        ContextualOptionPanel.SetActive(false);
        ContextualOptionTitle.text = "";
        ContextualOptionbtn1Text.text = "";
        ContextualOptionbtn2Text.text = "";
        HideCursor();
    }
    
    private void ShowCursor()
    {
        if(isCursorVisible)return;
        isCursorVisible = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void HideCursor()
    {
        if(!isCursorVisible)return;
        isCursorVisible = false; 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    #endregion
}