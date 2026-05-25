using UnityEngine;
using TMPro;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject pausePanel;
    public TMP_Text arButtonText;

    private bool isPauseMenuActive = false;
    private bool isInSubMenu = false;

    void Awake()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    void Start()
    {
        Time.timeScale = 1f;
        isPauseMenuActive = false;
        isInSubMenu = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC pressed");
            if (!isPauseMenuActive)
            {
                OpenPause();
            }
            else if (isInSubMenu)
            {
                CloseSubMenu();
            }
            else
            {
                Resume();
            }
        }
    }

    public void OpenPause()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isPauseMenuActive = true;
        isInSubMenu = false;
    }

    public void Resume()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPauseMenuActive = false;
        isInSubMenu = false;
    }

    public void OpenSettings()
    {
        Debug.Log("Open settings");
        isInSubMenu = true;
    }

    public void CloseSubMenu()
    {
        Debug.Log("Back to game");
        isInSubMenu = false;
    }

    public void ExitToMenu()
    {
        Debug.Log("Back to main menu");
        GameManager.SavePlayerData();

        // останавливаем сеть перед сменой сцены
        if (Unity.Netcode.NetworkManager.Singleton != null)
            Unity.Netcode.NetworkManager.Singleton.Shutdown();

        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public bool IsPauseMenuActive => isPauseMenuActive;
}