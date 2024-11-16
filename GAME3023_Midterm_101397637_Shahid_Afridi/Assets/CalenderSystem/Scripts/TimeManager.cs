using UnityEngine;
using UnityEngine.Events;

namespace DPUtils.Systems.DateTime
{
    public class TimeManager : MonoBehaviour
    {
        [Header("Date & Time Settings")]
        [Range(1, 28)] public int dateInMonth; // Current day in the month
        [Range(1, 4)] public int season; // Season number (1=Spring, 2=Summer, etc.)
        [Range(1, 99)] public int year;
        [Range(0, 24)] public int hour;
        [Range(0, 6)] public int minutes;

        private DateTime DateTime;

        [Header("Tick Settings")]
        public int TickMinutesIncreased = 10;
        public float TimeBetweenTicks = 1;
        private float currentTimeBetweenTicks = 0;

        public static UnityAction<DateTime> OnDateTimeChanged;

        private void Awake()
        {
            DateTime = new DateTime(dateInMonth, season - 1, year, hour, minutes * 10);

            Debug.Log($"Current DateTime: {DateTime.ToString()}");
        }

        private void Start()
        {
            OnDateTimeChanged?.Invoke(DateTime);
        }

        private void Update()
        {
            currentTimeBetweenTicks += Time.deltaTime;

            if (currentTimeBetweenTicks >= TimeBetweenTicks)
            {
                currentTimeBetweenTicks = 0;
                Tick();
            }
        }

        void Tick()
        {
            AdvanceTime();
        }

        void AdvanceTime()
        {
            DateTime.AdvanceMinutes(TickMinutesIncreased);

            // Update season based on real-world month
            UpdateSeasonBasedOnDate();

            OnDateTimeChanged?.Invoke(DateTime);
        }

        // Updates the season based on the real-world month
        private void UpdateSeasonBasedOnDate()
        {
            int currentMonth = (dateInMonth - 1) / 28;  // Divide by 28 to get the current month (0-based)

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

    [System.Serializable]
    public enum Season
    {
        Spring = 0,
        Summer = 1,
        Autumn = 2,
        Winter = 3
    }

    [System.Serializable]
    public struct DateTime
    {
        private Days day;
        private int date;
        private int year;

        private int hour;
        private int minutes;

        private Season season;

        private int totalNumDays;
        private int totalNumWeeks;

        // Properties
        public Days Day => day;
        public int Date => date;
        public int Hour => hour;
        public int Minutes => minutes;
        public Season Season => season;
        public int Year => year;
        public int TotalNumDays => totalNumDays;
        public int TotalNumWeeks => totalNumWeeks;
        public int CurrentWeek => totalNumWeeks % 16 == 0 ? 16 : totalNumWeeks % 16;

        // Constructors
        public DateTime(int date, int season, int year, int hour, int minutes)
        {
            this.day = (Days)(date % 7);
            if (day == 0) day = (Days)7;
            this.date = date;
            this.season = (Season)season;
            this.year = year;

            this.hour = hour;
            this.minutes = minutes;

            totalNumDays = date + (28 * (int)this.season) + (112 * (year - 1));
            totalNumWeeks = 1 + totalNumDays / 7;
        }

        // Time Advancement
        public void AdvanceMinutes(int SecondsToAdvanceBy)
        {
            if (minutes + SecondsToAdvanceBy >= 60)
            {
                minutes = (minutes + SecondsToAdvanceBy) % 60;
                AdvanceHour();
            }
            else
            {
                minutes += SecondsToAdvanceBy;
            }
        }

        private void AdvanceHour()
        {
            if ((hour + 1) == 24)
            {
                hour = 0;
                AdvanceDay();
            }
            else
            {
                hour++;
            }
        }

        private void AdvanceDay()
        {
            day++;

            if (day > (Days)7)
            {
                day = (Days)1;
                totalNumWeeks++;
            }

            date++;

            if (date % 29 == 0)
            {
                AdvanceSeason();
                date = 1;
            }

            totalNumDays++;
        }

        private void AdvanceSeason()
        {
            if (season == Season.Winter)
            {
                season = Season.Spring;
                AdvanceYear();
            }
            else
            {
                season++;
            }
        }

        private void AdvanceYear()
        {
            date = 1;
            year++;
        }

        // To Strings
        public override string ToString()
        {
            return $"Date: {DateToString()} Season: {season} Time: {TimeToString()} " +
                $"\nTotal Days: {totalNumDays} | Total Weeks: {totalNumWeeks}";
        }

        public string DateToString()
        {
            return $"{Day} {Date} {Year.ToString("D2")}";
        }

        public string TimeToString()
        {
            int adjustedHour = 0;

            if (hour == 0)
            {
                adjustedHour = 12;
            }
            else if (hour >= 13)
            {
                adjustedHour = hour - 12;
            }
            else
            {
                adjustedHour = hour;
            }

            string AmPm = hour < 12 ? "AM" : "PM";

            return $"{adjustedHour.ToString("D2")}:{minutes.ToString("D2")} {AmPm}";
        }
    }

    [System.Serializable]
    public enum Days
    {
        NULL = 0,
        Mon = 1,
        Tue = 2,
        Wed = 3,
        Thu = 4,
        Fri = 5,
        Sat = 6,
        Sun = 7
    }
}
