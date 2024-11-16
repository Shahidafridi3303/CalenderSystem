using UnityEngine;
using DPUtils.Systems.DateTime;

public class EventTrigger : MonoBehaviour
{
    private Season lastSeason;  // To store the last season

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
        // Check if the season has changed
        if (lastSeason != dateTime.Season)
        {
            lastSeason = dateTime.Season;  // Update the last season to the current season

            // Check if it's a specific season (e.g., Summer)
            if (dateTime.Season == Season.Summer)
            {
                Debug.Log("It's Summer season! Perfect time for beach events.");
            }

            // Check if it's a specific season (e.g., Spring)
            if (dateTime.Season == Season.Spring)
            {
                Debug.Log("Rainy Season is here! Expect some rain this week.");
            }

            // Check if it's a specific season (e.g., Winter)
            if (dateTime.Season == Season.Winter)
            {
                Debug.Log("Snowy Festival: Get your festive hat on and enjoy the jingling bells as you walk!");
            }
        }

        // Other time-specific checks and events (like date and time checks) remain the same:

        // Check for a specific date (e.g., 1st day of the month)
        if (dateTime.Hour == 0 && dateTime.Minutes == 0)
        {
            Debug.Log("It's the start of a new day!");
        }

        // Check if it's a specific time (e.g., 12 PM)
        if (dateTime.Hour == 12)
        {
            Debug.Log("It's 12 PM! Time for a break!");
        }

        // Event for the first day of the month at 12 PM in Summer
        if (dateTime.Date == 1 && dateTime.Hour == 12 && dateTime.Season == Season.Summer)
        {
            Debug.Log("Special event: First day of the month, 12 PM in Summer! Prepare for festivities.");
        }

        // Event for Summer Solstice (e.g., Summer 4th)
        if (dateTime.Season == Season.Summer && dateTime.Date == 4)
        {
            Debug.Log("It's the Summer Solstice! The sun is extremely bright today.");
        }

        // Year's Renewal event between Winter and Spring (e.g., Winter 7th to Spring 1st)
        if ((dateTime.Season == Season.Winter && dateTime.Date >= 7) || (dateTime.Season == Season.Spring && dateTime.Date == 1))
        {
            Debug.Log("Year's Renewal: Enjoy fireworks and colorful flashes from evening to morning!");
        }
    }
}
