using UnityEngine;
using Dreamteck.Splines;

public class CartMovement : MonoBehaviour
{
    public SplineComputer spline;
    public float speed = 10f; // Теперь это скорость в метрах/секунду

    // Используем прогресс от 0 до 1 вместо дистанции
    private float currentProgress = 0f;
    private float splineLength = 0f; // Сохраним здесь общую длину трека

    void Start()
    {
        if (spline == null)
        {
            Debug.LogError("Spline Computer не назначен!");
            this.enabled = false;
            return;
        }

        // Вычисляем общую длину сплайна ОДИН раз при старте для эффективности
        splineLength = spline.CalculateLength();

        // Устанавливаем начальную позицию и ротацию вагонетки в 0% пути
        UpdateCartTransform(0.0);
    }

    void Update()
    {
        // Если длина сплайна равна нулю, ничего не делаем
        if (splineLength <= 0f) return;

        // Вычисляем, какую долю пути (в процентах) мы должны пройти за этот кадр
        // (скорость / общая_длина) - это процент пути, проходимый за секунду
        currentProgress += (speed / splineLength) * Time.deltaTime;

        // Если мы доехали до конца, останавливаемся на 100%
        if (currentProgress >= 1f)
        {
            currentProgress = 1f;
        }

        // Обновляем позицию и ротацию вагонетки
        UpdateCartTransform(currentProgress);
    }

    // Эта функция теперь принимает прогресс от 0.0 до 1.0
    private void UpdateCartTransform(double progress)
    {
        // spline.Evaluate использует процент, чтобы найти точку на кривой
        SplineSample result = spline.Evaluate(progress);

        transform.position = result.position;
        transform.rotation = Quaternion.LookRotation(result.forward, result.up);
    }
}