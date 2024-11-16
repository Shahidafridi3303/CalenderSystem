using UnityEngine;
using DPUtils.Systems.DateTime;

public class WeatherManager : MonoBehaviour
{
    public static Weather currentWeather = Weather.Sunny;
    [SerializeField] private GameObject _rain;

    private void OnEnable()
    {
        TimeManager.OnDateTimeChanged += GetRandomWeather;
    }

    private void OnDisable()
    {
        TimeManager.OnDateTimeChanged -= GetRandomWeather;
    }

    private void GetRandomWeather(DateTime dateTime)
    {
        // Change weather condition at midnight (00:00) or any other time
        if (dateTime.Hour == 0 && dateTime.Minutes == 0)
        {
            // Set random weather condition
            currentWeather = (Weather)Random.Range(0, (int)Weather.MAX_WEATHER_AMOUNT + 1);

            // Enable or disable rain GameObject based on weather
            if (currentWeather == Weather.Raining)
            {
                _rain.SetActive(true); // Enable rain GameObject
            }
            else
            {
                _rain.SetActive(false); // Disable rain GameObject
            }
        }
    }
}

public enum Weather
{
    Sunny = 0,
    Raining = 1,
    MAX_WEATHER_AMOUNT = Raining
}