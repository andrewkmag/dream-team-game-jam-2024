using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject optionsMenuUI;
    [SerializeField] private PlayerHUDManager playerHUDManager;

    [Header("Settings")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    // Local variables
    private bool isPaused;

    private void Awake()
    {
        // Find the pause menu UI in the scene
        pauseMenuUI = GameObject.Find("PauseMenu");

        // Find the options menu UI in the scene
        optionsMenuUI = GameObject.Find("OptionsMenu");

        // Assign button listeners
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            resumeButton.onClick.AddListener(ResumeGame);
            mainMenuButton.onClick.AddListener(GoToMainMenu);
            optionsButton.onClick.AddListener(OpenOptionsMenu);
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    public void TogglePause()
    {
        if (pauseMenuUI == null) return;

        isPaused = !isPaused;

        if (isPaused)
        {
            ShowPauseMenu();
            playerHUDManager.HideHud();

        }
        else
        {
            HidePauseMenu();
            playerHUDManager.ShowHud();
        }
    }

    private void ShowPauseMenu()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void HidePauseMenu()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void ResumeGame()
    {
        isPaused = false;
        HidePauseMenu();
        playerHUDManager.ShowHud();
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f;  // Ensure game time is running normally
        // Load the first scene in the build settings
        SceneManager.LoadScene(0);
    }

    private void OpenOptionsMenu()
    {
        if (optionsMenuUI != null)
        {
            optionsMenuUI.SetActive(true);
        }
    }

    private void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Stop playing the game in the Unity Editor
#endif
    }
}
