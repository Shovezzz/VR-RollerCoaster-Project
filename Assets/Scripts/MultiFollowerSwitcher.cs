using Dreamteck.Splines;
using UnityEngine;

public class MultiFollowerSwitcher : MonoBehaviour
{
    private enum PlayerChoice { None, Main, Detour }

    [Header("Компоненты Spline Follower")]
    public SplineFollower mainFollower;
    public SplineFollower detourFollower;

    [Header("Точки стыковки")]
    [Range(0f, 1f)]
    public double returnPercentOnMain = 0.5;

    [Header("Интерфейс выбора")]
    public GameObject choiceInterfaceParent;

    private PlayerChoice _playerChoice = PlayerChoice.None;
    private bool _isInChoiceZone = false;

    void Start()
    {
        if (choiceInterfaceParent != null)
        {
            choiceInterfaceParent.SetActive(false);
        }
    }

    public void MakeChoice(bool choseDetour)
    {
        if (_playerChoice != PlayerChoice.None) return;

        if (choseDetour)
        {
            _playerChoice = PlayerChoice.Detour;
            Debug.Log("Выбор игрока ЗАПОМНЕН: Обходной путь");
        }
        else
        {
            _playerChoice = PlayerChoice.Main;
            Debug.Log("Выбор игрока ЗАПОМНЕН: Основной путь");
        }

        if (choiceInterfaceParent != null)
        {
            choiceInterfaceParent.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SwitchToDetour")) 
        {
            if (_isInChoiceZone) return;

            _isInChoiceZone = true;
            _playerChoice = PlayerChoice.None; 

            if (choiceInterfaceParent != null)
            {
                choiceInterfaceParent.SetActive(true);
            }
            Debug.Log("Въехали в зону выбора (Показ стрелок)");
        }

        if (other.CompareTag("ExecuteSwitchZone")) 
        {
            Debug.Log("Въехали в зону переключения. Текущий выбор: " + _playerChoice);
            ExecuteSwitch();
        }

        if (other.CompareTag("SwitchBackToMain") && detourFollower.enabled)
        {
            SwitchToMain();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SwitchToDetour"))
        {
            Debug.Log("Вышли из триггера показа стрелок (но выбор все еще можно сделать).");
        }
    }

    private void ExecuteSwitch()
    {
        _isInChoiceZone = false;

        if (choiceInterfaceParent != null)
        {
            choiceInterfaceParent.SetActive(false);
        }

        if (_playerChoice == PlayerChoice.Detour)
        {
            Debug.Log("ВЫПОЛНЯЕМ переключение на обходной путь!");
            SwitchToDetour();
        }
        else
        {
            Debug.Log("ВЫПОЛНЯЕМ переключение: остаемся на основном пути.");
        }
    }
    private void SwitchToDetour()
    {
        mainFollower.enabled = false;
        detourFollower.enabled = true;
        detourFollower.SetPercent(0.0);
        detourFollower.RebuildImmediate();
    }

    private void SwitchToMain()
    {
        detourFollower.enabled = false;
        mainFollower.enabled = true;
        mainFollower.SetPercent(returnPercentOnMain);
        mainFollower.RebuildImmediate();
    }
}