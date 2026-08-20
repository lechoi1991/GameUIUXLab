using UnityEngine;

public class MainMenuUIController : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Sub Panels")]
    [SerializeField] private GameObject createServerPanel;
    [SerializeField] private GameObject findRoomPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject quitConfirmPanel;

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);

        createServerPanel.SetActive(false);
        findRoomPanel.SetActive(false);
        settingsPanel.SetActive(false);
        quitConfirmPanel.SetActive(false);
    }

    public void OpenCreateServer()
    {
        createServerPanel.SetActive(true);
    }

    public void OpenFindRoom()
    {
        findRoomPanel.SetActive(true);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void OpenQuitConfirm()
    {
        quitConfirmPanel.SetActive(true);
    }

    public void CancelQuit()
    {
        quitConfirmPanel.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void CancelSubPanel()
    {
        ShowMainMenu();
    }
}