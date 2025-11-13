using UnityEngine;
using Dreamteck.Splines;

public class MultiFollowerSwitcher : MonoBehaviour
{
    [Header("Компоненты Spline Follower")]
    public SplineFollower mainFollower;
    public SplineFollower detourFollower;

    [Header("Точки стыковки")]
    [Range(0f, 1f)]
    public double returnPercentOnMain = 0.5;

    [Header("Интерфейс выбора")]
    public GameObject choiceInterfaceParent; 

    private bool _choiceMadeThisRun = false;

    void Start()
    {
        if (choiceInterfaceParent != null)
        {
            choiceInterfaceParent.SetActive(false);
        }
    }

    public void MakeChoice(bool choseDetour)
    {
        if (_choiceMadeThisRun) return;

        _choiceMadeThisRun = true;

        if (choiceInterfaceParent != null)
        {
            choiceInterfaceParent.SetActive(false);
        }

        if (choseDetour)
        {
            Debug.Log("Игрок выбрал ОБХОДНОЙ путь");
            SwitchToDetour();
        }
        else
        {
            Debug.Log("Игрок выбрал ОСНОВНОЙ путь");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SwitchToDetour"))
        {
            _choiceMadeThisRun = false; 
            if (choiceInterfaceParent != null)
            {
                choiceInterfaceParent.SetActive(true);
            }
        }

        if (other.CompareTag("SwitchBackToMain") && detourFollower.enabled)
        {
            SwitchToMain();
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