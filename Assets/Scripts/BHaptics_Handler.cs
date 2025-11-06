using UnityEngine;
using Bhaptics.SDK2;

public class BHaptics_Handler : MonoBehaviour
{
    // === КОНСТАНТЫ ДЛЯ ИМЕН СОБЫТИЙ ===
    // Позже добавлять сюда новые имена.
    private const string RUMBLE_EVENT = "TrackRumble";
    private const string WATER_SPLASH_EVENT = "WaterSplash"; 
    private const string WIND_RUSH_EVENT = "WindRush"; 

    private string _currentLoopingEffect = "";

    private void OnTriggerEnter(Collider other)
    {
        // Если въехали в зону тряски
        if (other.CompareTag("RumbleZone"))
        {
            BhapticsLibrary.PlayLoop(RUMBLE_EVENT);
        }
        // Здесь добавлять другие зоны
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RumbleZone") && _currentLoopingEffect == RUMBLE_EVENT)
        {
            BhapticsLibrary.StopByEventId(RUMBLE_EVENT);
        }
        // Здесь добавлять другие зоны
    }
}