using UnityEngine;
using Bhaptics.SDK2;

public class BHaptics_Handler : MonoBehaviour
{
    private string _currentLoopingEvent = "";

    private void OnTriggerEnter(Collider other)
    {
        // 1. ÷» Ћ»„Ќџ≈ Ё‘‘≈ “џ
        // »грают пока мы внутри триггера

        if (other.CompareTag("Haptic_Lift"))
        {
            PlayLoop("liftchain");
        }
        else if (other.CompareTag("Haptic_Wind"))
        {
            PlayLoop("wind_fast");
        }

        // 2. –ј«ќ¬џ≈ Ё‘‘≈ “џ
        // »грают один раз при входе

        else if (other.CompareTag("Haptic_Drop"))
        {
            BhapticsLibrary.Play("gforce_drop");
        }
        else if (other.CompareTag("Haptic_TurnLeft"))
        {
            BhapticsLibrary.Play("turn_force_left");
        }
        else if (other.CompareTag("Haptic_TurnRight"))
        {
            BhapticsLibrary.Play("turn_force_right");
        }
        else if (other.CompareTag("Haptic_Boost"))
        {
            BhapticsLibrary.Play("nitro_boost");
        }
        else if (other.CompareTag("Haptic_Brake"))
        {
            BhapticsLibrary.Play("hard_brake");
        }
        else if (other.CompareTag("Haptic_Bird"))
        {
            BhapticsLibrary.Play("bird_flyby");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Haptic_Lift") && _currentLoopingEvent == "liftchain")
        {
            StopLoop();
        }
        if (other.CompareTag("Haptic_Wind") && _currentLoopingEvent == "wind_Fast")
        {
            StopLoop();
        }
    }

    private void PlayLoop(string eventName)
    {
        StopLoop();

        BhapticsLibrary.PlayLoop(eventName);
        _currentLoopingEvent = eventName;
    }

    private void StopLoop()
    {
        if (!string.IsNullOrEmpty(_currentLoopingEvent))
        {
            BhapticsLibrary.StopByEventId(_currentLoopingEvent);
            _currentLoopingEvent = "";
        }
    }

    // вызвать эту функцию из MenuManager при паузе или финише, чтобы остановить вибрацию
    public void StopAllHaptics()
    {
        BhapticsLibrary.StopAll();
        _currentLoopingEvent = "";
    }
}