using UnityEngine;
using UnityEngine.InputSystem;

public class UIScreenFlowController : MonoBehaviour
{
    private enum ScreenState
    {
        Title,
        Play,
        Pause,
        Result
    }

    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject playHudScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject resultScreen;

    [Header("HUD")]
    [SerializeField] private HUDView hudView;

    [Header("Input")]
    [SerializeField] private InputActionReference pauseAction;

    private ScreenState currentState;

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed += OnPauseInput;
            pauseAction.action.Enable();
        }

        if (hudView != null)
        {
            hudView.OnHealthDepleted += OnHealthDepleted;
            hudView.OnTimeDepleted += OnTimeDepleted;
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPauseInput;
            pauseAction.action.Disable();
        }

        if (hudView != null)
        {
            hudView.OnHealthDepleted -= OnHealthDepleted;
            hudView.OnTimeDepleted -= OnTimeDepleted;
        }
    }

    private void Start()
    {
        ShowTitle();
    }

    private void OnPauseInput(InputAction.CallbackContext context)
    {
        if (currentState == ScreenState.Play)
        {
            PauseGame();
        }
        else if (currentState == ScreenState.Pause)
        {
            ResumeGame();
        }
    }

    private void OnHealthDepleted()
    {
        ShowResult();
    }

    public void ShowTitle()
    {
        ChangeScreen(ScreenState.Title);
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        if (hudView != null)
        {
            hudView.ResetHUD();
        }
        ChangeScreen(ScreenState.Play);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void PauseGame()
    {
        ChangeScreen(ScreenState.Pause);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        ChangeScreen(ScreenState.Play);
        Time.timeScale = 1f;
    }

    public void ShowResult()
    {
        ChangeScreen(ScreenState.Result);
        Time.timeScale = 1f;
    }

    private void OnTimeDepleted()
    {
        ShowResult();
    }
    
    private void ChangeScreen(ScreenState nextState)
    {
        currentState = nextState;

        titleScreen.SetActive(currentState == ScreenState.Title);

        playHudScreen.SetActive(
            currentState == ScreenState.Play ||
            currentState == ScreenState.Pause
        );

        pauseScreen.SetActive(currentState == ScreenState.Pause);

        resultScreen.SetActive(currentState == ScreenState.Result);
    }
}