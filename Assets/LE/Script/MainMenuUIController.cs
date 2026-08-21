using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Main Menu Buttons")]
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button findRoomButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    [Header("Sub Panels")]
    [SerializeField] private GameObject createRoomPanel;
    [SerializeField] private GameObject findRoomPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject quitConfirmPanel;

    [Header("Input")]
    [SerializeField] private InputActionReference backAction;

    private readonly Stack<GameObject> panelStack = new();

    private void OnEnable()
    {
        if (backAction != null)
        {
            backAction.action.performed += OnBackInput;
            backAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (backAction != null)
        {
            backAction.action.performed -= OnBackInput;
            backAction.action.Disable();
        }
    }

    private void Start()
    {
        ShowMainMenu();
    }

    private void OnBackInput(InputAction.CallbackContext context)
    {
        CloseCurrentPanel();
    }

    public void ShowMainMenu()
    {
        ClearPanelStack();

        mainMenuPanel.SetActive(true);

        SetMainMenuNavigation(true);

        EventSystem.current.SetSelectedGameObject(
            createRoomButton.gameObject
        );
    }

    public void OpenCreateRoom()
    {
        OpenPanel(createRoomPanel);
    }

    public void OpenFindRoom()
    {
        OpenPanel(findRoomPanel);
    }

    public void OpenSettings()
    {
        OpenPanel(settingsPanel);
    }

    public void OpenQuitConfirm()
    {
        OpenPanel(quitConfirmPanel);
    }

    private void OpenPanel(GameObject panel)
    {
        if (panel == null)
            return;

        panel.SetActive(true);

        panelStack.Push(panel);

        SetMainMenuNavigation(false);

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void CloseCurrentPanel()
    {
        if (panelStack.Count == 0)
            return;

        GameObject currentPanel = panelStack.Pop();

        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
        }

        if (panelStack.Count == 0)
        {
            SetMainMenuNavigation(true);

            EventSystem.current.SetSelectedGameObject(
                createRoomButton.gameObject
            );
        }
    }

    private void SetMainMenuNavigation(bool enabled)
    {
        SetButtonNavigation(createRoomButton, enabled);
        SetButtonNavigation(findRoomButton, enabled);
        SetButtonNavigation(settingsButton, enabled);
        SetButtonNavigation(exitButton, enabled);
    }

    private void SetButtonNavigation(Button button, bool enabled)
    {
        if (button == null)
            return;

        Navigation navigation = button.navigation;

        navigation.mode = enabled
            ? Navigation.Mode.Explicit
            : Navigation.Mode.None;

        button.navigation = navigation;
    }

    private void ClearPanelStack()
    {
        while (panelStack.Count > 0)
        {
            GameObject panel = panelStack.Pop();

            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}