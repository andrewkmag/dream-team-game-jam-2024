using UnityEngine;

public class CheatManager : MonoBehaviour
{
#if UnityEditor
    #region Fields

    public GameObject panel;
    private bool isCursorVisible = false;
    private bool _isPanelNotNull;

    #endregion

    #region Constants

    private const int FIRST_CHILD_INDEX = 0;
    private const KeyCode TOGGLE_KEY = KeyCode.O;

    #endregion

    #region UnityMethods

    private void Start()
    {
        panel.SetActive(false);
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
        ToggleCursorVisibility();
    }

    #endregion

    #region Methods

    private void TogglePanelVisibility()
    {
        if (!_isPanelNotNull) return;
        panel.SetActive(!panel.activeSelf);
    }

    private void ToggleCursorVisibility()
    {
        isCursorVisible = !isCursorVisible;
        if (isCursorVisible)
        {
            ShowCursor();
        }
        else
        {
            HideCursor();
        }
    }

    private void ShowCursor()
    {
        isCursorVisible = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void HideCursor()
    {
        isCursorVisible = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    #endregion
#endif
}