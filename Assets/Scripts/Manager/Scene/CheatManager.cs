using UnityEngine;

public class CheatManager : MonoBehaviour
{
    #region Fields

    public GameObject panel;
    private bool _isPanelNotNull;

    #endregion

    #region Constants

    private const int FIRST_CHILD_INDEX = 0;
    private const KeyCode TOGGLE_KEY = KeyCode.O;

    #endregion

    #region UnityMethods

    private void Start()
    {
        _isPanelNotNull = panel != null;
    }

    private void Reset()
    {
        if (transform.childCount <= FIRST_CHILD_INDEX) return;
        panel = transform.GetChild(FIRST_CHILD_INDEX).gameObject;
        _isPanelNotNull = panel != null;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(TOGGLE_KEY)) return;
        TogglePanelVisibility();
    }

    #endregion

    #region Methods

    private void TogglePanelVisibility()
    {
        if (!_isPanelNotNull) return;
        panel.SetActive(!panel.activeSelf);
    }

    #endregion
}