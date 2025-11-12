using UnityEngine;
using Dreamteck.Splines;

// Этот скрипт должен висеть на том же объекте, что и оба Spline Follower
public class MultiFollowerSwitcher : MonoBehaviour
{
    [Header("Компоненты Spline Follower")]
    [Tooltip("Перетащите сюда компонент Spline Follower для ОСНОВНОГО пути")]
    public SplineFollower mainFollower;

    [Tooltip("Перетащите сюда компонент Spline Follower для ОБХОДНОГО пути")]
    public SplineFollower detourFollower;

    [Header("Точки стыковки")]
    [Tooltip("Процент на ОСНОВНОМ сплайне, куда нужно вернуться ПОСЛЕ обхода")]
    [Range(0f, 1f)]
    public double returnPercentOnMain = 0.5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SwitchToDetour"))
        {
            if (mainFollower.enabled)
            {
                Debug.Log("Переключаемся на обходной маршрут!");
                mainFollower.enabled = false;

                detourFollower.enabled = true;
                detourFollower.SetPercent(0.0);
                detourFollower.RebuildImmediate();
            }
        }

        if (other.CompareTag("SwitchBackToMain"))
        {
            if (detourFollower.enabled)
            {
                Debug.Log("Возвращаемся на основной маршрут!");

                detourFollower.enabled = false;

                mainFollower.enabled = true;
                mainFollower.SetPercent(returnPercentOnMain);
                mainFollower.RebuildImmediate();
            }
        }
    }
}