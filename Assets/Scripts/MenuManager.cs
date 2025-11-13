using UnityEngine;
using Dreamteck.Splines;

public class MenuManager : MonoBehaviour
{
    [Tooltip("Перетащите сюда объект Cart_Root с компонентами Spline Follower")]
    public SplineFollower mainFollower; // Ссылка на ОСНОВНОЙ Spline Follower

    void Start()
    {
        if (mainFollower != null)
        {
            mainFollower.SetPercent(0.0);
            mainFollower.RebuildImmediate();
        }

        mainFollower.enabled = false;

        var detourFollower = GetComponentInChildren<SplineFollower>(true); 
        if (detourFollower != null && detourFollower != mainFollower)
        {
            detourFollower.enabled = false;
        }
    }

    public void StartRide()
    {
        Debug.Log("Кнопка 'Старт' нажата!");

        if (mainFollower != null)
        {
            mainFollower.enabled = true;
        }

        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Кнопка 'Выход' нажата!");

        Application.Quit();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}