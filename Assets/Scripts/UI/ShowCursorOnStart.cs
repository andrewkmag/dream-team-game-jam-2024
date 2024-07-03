using UnityEngine;

public class ShowCursorOnStart : MonoBehaviour
{
    private void Start()
    {
        ShowCursor();
    }

    private static void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
