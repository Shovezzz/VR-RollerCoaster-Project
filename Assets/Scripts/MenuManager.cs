using UnityEngine;
using TMPro;
using Dreamteck.Splines;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Ссылки на объекты")]
    public SplineFollower mainFollower;
    public SplineFollower detourFollower;

    [Header("UI Элементы")]
    public Button startButton;
    public TextMeshProUGUI startButtonText;

    private bool isPaused = false;
    private bool isRideFinished = false;

    private SplineFollower _activeFollowerBeforePause;

    void Start()
    {
        gameObject.SetActive(true);
        SetupMenuForNewGame();
    }

    void Update()
    {
        if (!isRideFinished && mainFollower.enabled && mainFollower.result.percent >= 0.999)
        {
            FinishRide();
        }
    }

    public void OnStartButtonPressed()
    {
        if (isRideFinished) RestartRide();
        else if (isPaused) ResumeRide();
        else StartRide();
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void TogglePauseMenu()
    {
        if (isRideFinished || (!mainFollower.enabled && !detourFollower.enabled && !isPaused)) return;

        isPaused = !isPaused;
        if (isPaused)
        {
            PauseRide();
        }
        else
        {
            ResumeRide();
        }
    }

    private void StartRide()
    {
        HideMenu();
        mainFollower.enabled = true;
    }

    private void PauseRide()
    {
        if (mainFollower.enabled)
        {
            _activeFollowerBeforePause = mainFollower;
        }
        else if (detourFollower.enabled)
        {
            _activeFollowerBeforePause = detourFollower;
        }

        Time.timeScale = 0f;
        mainFollower.enabled = false;
        detourFollower.enabled = false;
        ShowMenu();
    }

    private void ResumeRide()
    {
        HideMenu();
        Time.timeScale = 1f;

        if (_activeFollowerBeforePause != null)
        {
            _activeFollowerBeforePause.enabled = true;
        }
        else
        {
            mainFollower.enabled = true;
        }

        isPaused = false;
    }

    private void FinishRide()
    {
        isRideFinished = true;
        mainFollower.enabled = false;
        detourFollower.enabled = false;
        ShowMenu();
    }

    private void RestartRide()
    {
        isRideFinished = false;
        SetupMenuForNewGame(); 
        StartRide();
    }

    private void ShowMenu()
    {
        gameObject.SetActive(true);
        if (isRideFinished)
        {
            startButtonText.text = "Начать заново";
        }
        else if (isPaused)
        {
            startButtonText.text = "Продолжить";
        }
    }

    private void SetupMenuForNewGame()
    {
        mainFollower.enabled = false;
        detourFollower.enabled = false;
        mainFollower.SetPercent(0.0);
        detourFollower.SetPercent(0.0);
        mainFollower.RebuildImmediate();
        startButtonText.text = "Начать заезд";
        isPaused = false;
        isRideFinished = false;
        Time.timeScale = 1f;
    }

    private void HideMenu()
    {
        gameObject.SetActive(false);
    }
}