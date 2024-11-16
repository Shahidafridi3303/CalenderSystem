using UnityEngine;
using DPUtils.Systems.DateTime;

public class EventManager : MonoBehaviour
{
    private void OnEnable()
    {
        // Subscribe to the event when DateTime changes
        TimeManager.OnDateTimeChanged += UpdateDateTime;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event to avoid memory leaks
        TimeManager.OnDateTimeChanged -= UpdateDateTime;
    }

    private void UpdateDateTime(DateTime dateTime)
    {
        // Example checks

        // Check for a specific date (e.g., 1st day of the month)
        if (dateTime.Date == 1)
        {
            Debug.Log("It's the 1st day of the month!");
        }

        // Check if it's a specific time (e.g., 12 PM)
        if (dateTime.Hour == 12)
        {
            Debug.Log("It's 12 PM!");
        }

        // Check if it's a specific season (e.g., Summer)
        if (dateTime.Season == Season.Summer)
        {
            Debug.Log("It's Summer season!");
        }

        // You can also combine checks to fire a specific event, for example:
        if (dateTime.Date == 1 && dateTime.Hour == 12 && dateTime.Season == Season.Summer)
        {
            // Trigger a specific event when it's the first day of the month at 12 PM in Summer
            Debug.Log("Special event: It's the first day of the month at 12 PM in Summer!");
        }
    }
}