using UnityEngine;
using UnityEngine.Events;

namespace DPUtils.Systems.DateTime
{
    // TimeManager class manages the simulation of time (day, season, and year).
    public class TimeManager : MonoBehaviour
    {
        // Public variables for Date & Time Settings, adjustable from the Unity Inspector
        [Header("Date & Time Settings")]
        [Range(1, 28)] public int dateInMonth; // Current day in the month (1-28)
        [Range(1, 4)] public int season; // Season number (1=Spring, 2=Summer, etc.)
        [Range(1, 99)] public int year; // Current year (1-99)
        [Range(0, 24)] public int hour; // Current hour (0-23)
        [Range(0, 6)] public int minutes; // Current minutes (0-59)

        private DateTime DateTime; // A DateTime object to hold the current time state

        // Tick settings
        [Header("Tick Settings")]
        public int TickMinutesIncreased = 10; // Number of minutes to advance per tick
        public float TimeBetweenTicks = 1; // Time (in seconds) between each tick
        private float currentTimeBetweenTicks = 0; // Keeps track of elapsed time for tick cycle

        // Event fired when the DateTime is updated
        public static UnityAction<DateTime> OnDateTimeChanged;

        private void Awake()
        {
            // Initialize DateTime with the current values from the Inspector
            DateTime = new DateTime(dateInMonth, season - 1, year, hour, minutes * 10);
            Debug.Log($"Current DateTime: {DateTime.ToString()}");
        }

        private void Start()
        {
            // Trigger the OnDateTimeChanged event to notify about the initial DateTime
            OnDateTimeChanged?.Invoke(DateTime);
        }

        private void Update()
        {
            // Update the elapsed time for ticking
            currentTimeBetweenTicks += Time.deltaTime;

            // Check if it's time to advance the time based on the tick cycle
            if (currentTimeBetweenTicks >= TimeBetweenTicks)
            {
                currentTimeBetweenTicks = 0;
                Tick(); // Call tick to advance the time
            }
        }

        // Advance the time by calling the methods to update DateTime
        void Tick()
        {
            AdvanceTime();
        }

        // Advances the time by TickMinutesIncreased and updates the season
        void AdvanceTime()
        {
            DateTime.AdvanceMinutes(TickMinutesIncreased); // Advance time by defined minutes
            UpdateSeasonBasedOnDate(); // Update season based on the date
            OnDateTimeChanged?.Invoke(DateTime); // Notify listeners about the time update
        }

        // Updates the season based on the real-world month
        private void UpdateSeasonBasedOnDate()
        {
            int currentMonth = (dateInMonth - 1) / 28;  // Divide by 28 to determine the current month (0-based)

            // Assign the season based on the current month
            if (currentMonth >= 2 && currentMonth <= 4)  // March-May: Spring
            {
                season = (int)Season.Spring;
            }
            else if (currentMonth >= 5 && currentMonth <= 7)  // June-August: Summer
            {
                season = (int)Season.Summer;
            }
            else if (currentMonth >= 8 && currentMonth <= 10)  // September-November: Autumn
            {
                season = (int)Season.Autumn;
            }
            else  // December-February: Winter
            {
                season = (int)Season.Winter;
            }
        }
    }

    // Enum representing the four seasons
    [System.Serializable]
    public enum Season
    {
        Spring = 0,  // Represents Spring season
        Summer = 1,  // Represents Summer season
        Autumn = 2,  // Represents Autumn season
        Winter = 3   // Represents Winter season
    }

    // DateTime struct represents the date and time system, including day, date, season, and time
    [System.Serializable]
    public struct DateTime
    {
        private Days day;  // The current day of the week
        private int date;  // The day of the month (1-28)
        private int year;  // The current year
        private int hour;  // The current hour (0-23)
        private int minutes;  // The current minutes (0-59)
        private Season season;  // The current season

        private int totalNumDays;  // Total number of days since the beginning
        private int totalNumWeeks; // Total number of weeks since the beginning

        // Properties for accessing the DateTime fields
        public Days Day => day;
        public int Date => date;
        public int Hour => hour;
        public int Minutes => minutes;
        public Season Season => season;
        public int Year => year;
        public int TotalNumDays => totalNumDays;
        public int TotalNumWeeks => totalNumWeeks;
        public int CurrentWeek => totalNumWeeks % 16 == 0 ? 16 : totalNumWeeks % 16;

        // Constructor initializes the DateTime struct with given values
        public DateTime(int date, int season, int year, int hour, int minutes)
        {
            this.day = (Days)(date % 7); // Assign the day of the week (0-based, 7 maps to Sunday)
            if (day == 0) day = (Days)7; // Correct 0 to 7 for Sunday
            this.date = date;
            this.season = (Season)season;
            this.year = year;
            this.hour = hour;
            this.minutes = minutes;

            // Calculate total number of days and weeks
            totalNumDays = date + (28 * (int)this.season) + (112 * (year - 1));
            totalNumWeeks = 1 + totalNumDays / 7;
        }

        // Advances time by a specified number of minutes
        public void AdvanceMinutes(int minutesToAdvanceBy)
        {
            if (minutes + minutesToAdvanceBy >= 60)
            {
                minutes = (minutes + minutesToAdvanceBy) % 60; // Wrap around minutes
                AdvanceHour(); // Advance the hour if minutes exceed 60
            }
            else
            {
                minutes += minutesToAdvanceBy;
            }
        }

        // Advances the hour and checks for wrapping around 24 hours
        private void AdvanceHour()
        {
            if ((hour + 1) == 24)
            {
                hour = 0;  // Reset hour to 0 after 23
                AdvanceDay();  // Advance to the next day
            }
            else
            {
                hour++;
            }
        }

        // Advances the day and updates the day of the week
        private void AdvanceDay()
        {
            day++;  // Move to the next day

            if (day > (Days)7)
            {
                day = (Days)1;  // Reset to Monday if past Sunday
                totalNumWeeks++;  // Increment the total number of weeks
            }

            date++;  // Advance the date (1-28)

            // If a month completes (28 days), advance to the next season
            if (date % 29 == 0)
            {
                AdvanceSeason();
                date = 1;  // Reset date to 1
            }

            totalNumDays++;  // Increment the total number of days
        }

        // Advances the season, wrapping from Winter to Spring if needed
        private void AdvanceSeason()
        {
            if (season == Season.Winter)
            {
                season = Season.Spring;  // Switch to Spring if it's Winter
                AdvanceYear();  // Advance the year when going from Winter to Spring
            }
            else
            {
                season++;  // Otherwise, just go to the next season
            }
        }

        // Advances the year (reset date and increment year)
        private void AdvanceYear()
        {
            date = 1;  // Reset date to 1
            year++;  // Increment the year
        }

        // Converts the DateTime to a string representation
        public override string ToString()
        {
            return $"Date: {DateToString()} Season: {season} Time: {TimeToString()}";
        }

        // Converts the date part of DateTime to a string
        public string DateToString()
        {
            return $"{Day} {Date} {Year.ToString("D2")}";
        }

        // Converts the time part of DateTime to a string in 12-hour format
        public string TimeToString()
        {
            int adjustedHour = 0;

            // Adjust hour for 12-hour format
            if (hour == 0)
            {
                adjustedHour = 12;  // Midnight case
            }
            else if (hour >= 13)
            {
                adjustedHour = hour - 12;  // Convert PM hours
            }
            else
            {
                adjustedHour = hour;  // AM hours remain unchanged
            }

            string AmPm = hour < 12 ? "AM" : "PM";  // Determine AM/PM based on the hour

            return $"{adjustedHour.ToString("D2")}:{minutes.ToString("D2")} {AmPm}";
        }
    }

    // Enum representing the days of the week (1=Mon, 2=Tue, ..., 7=Sun)
    public enum Days
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7
    }
}
