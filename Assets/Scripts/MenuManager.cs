using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Dreamteck.Splines;


public class MenuManager : MonoBehaviour
{
    [Header("Ссылки на объекты")]
    public SplineFollower mainFollower;
    public SplineFollower detourFollower;
    public Transform cameraTransform;

    [Header("UI Элементы")]
    public GameObject menuCanvasObject; 
    public Button startButton;
    public TextMeshProUGUI startButtonText;

    [Header("Настройки")]
    public float menuDistance = 2f;

    private Vector3 _initialMenuPosition;
    private Quaternion _initialMenuRotation;
    private Transform _initialMenuParent;

    private bool isPaused = false;
    private bool isRideFinished = false;
    private SplineFollower _activeFollowerBeforePause;

    void Start()
    {
        _initialMenuParent = menuCanvasObject.transform.parent;
        _initialMenuPosition = menuCanvasObject.transform.position;
        _initialMenuRotation = menuCanvasObject.transform.rotation;

        SetupMenuForNewGame();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishZone") && !isRideFinished)
        {
            Debug.Log("Въехали в финишную зону!");
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
        if (isPaused) PauseRide();
        else ResumeRide();
    }
    private void StartRide()
    {
        HideMenu();
        mainFollower.enabled = true;
    }

    private void PauseRide()
    {
        if (mainFollower.enabled) _activeFollowerBeforePause = mainFollower;
        else if (detourFollower.enabled) _activeFollowerBeforePause = detourFollower;

        Time.timeScale = 0f;
        AudioListener.pause = true;
        mainFollower.enabled = false;
        detourFollower.enabled = false;

        menuCanvasObject.transform.SetParent(cameraTransform);
        menuCanvasObject.transform.localPosition = new Vector3(0, 0, menuDistance);
        menuCanvasObject.transform.localRotation = Quaternion.identity;
        ShowMenu();
    }

    private void ResumeRide()
    {
        HideMenu();
        Time.timeScale = 1f;
        AudioListener.pause = false;

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
        Debug.Log("Функция FinishRide вызвана!");
        isRideFinished = true;
        mainFollower.enabled = false;
        detourFollower.enabled = false;
        menuCanvasObject.transform.SetParent(_initialMenuParent);
        menuCanvasObject.transform.position = _initialMenuPosition;
        menuCanvasObject.transform.rotation = _initialMenuRotation;
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
        menuCanvasObject.SetActive(true);
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
        menuCanvasObject.transform.SetParent(_initialMenuParent);
        menuCanvasObject.transform.position = _initialMenuPosition;
        menuCanvasObject.transform.rotation = _initialMenuRotation;

        mainFollower.enabled = false;
        detourFollower.enabled = false;
        mainFollower.SetPercent(0.0);
        detourFollower.SetPercent(0.0);
        mainFollower.RebuildImmediate();
        startButtonText.text = "Начать заезд";
        isPaused = false;
        isRideFinished = false;
        Time.timeScale = 1f;
        menuCanvasObject.SetActive(true);
        AudioListener.pause = false;
    }

    private void HideMenu()
    {
        menuCanvasObject.SetActive(false);
    }
}