using System;
using System.Collections.Generic;
using UnityEngine;

namespace YVR.Platform
{
    /// <summary>
    /// The summary of daily sport info. Users' daily sports info is recorded in the local database. This structure indicates the sports info generated someday.
    /// </summary>
    public class SportDailySummary
    {
        /// <summary>
        /// The date when the summary was generated.
        /// </summary>
        public readonly string date;

        /// <summary>
        /// The planned sport duration (in seconds).
        /// </summary>
        public readonly int planDurationInSeconds;

        /// <summary>
        /// The sport duration (in seconds).
        /// </summary>
        public readonly long durationInSeconds;

        /// <summary>
        /// The actual calorie burnt (in kilo calorie).
        /// </summary>
        public readonly double calorie;

        /// <summary>
        /// The planned calorie to burn.
        /// </summary>
        public readonly double planCalorie;

        public SportDailySummary(AndroidJavaObject obj)
        {
            date = YVRPlatform.YVRSportGetDailySummaryDate(obj);
            planDurationInSeconds = YVRPlatform.YVRSportGetDailySummaryPlanDurationInSeconds(obj);
            durationInSeconds = YVRPlatform.YVRSportGetDailySummaryDurationInSeconds(obj);
            calorie = YVRPlatform.YVRSportGetDailySummaryCalorie(obj);
            planCalorie = YVRPlatform.YVRSportGetDailySummaryPlanCalorie(obj);
        }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            str.Append($"date:[{date}],\n\r");
            str.Append($"planDurationInSeconds:[{planDurationInSeconds}],\n\r");
            str.Append($"durationInSeconds:[{durationInSeconds}],\n\r");
            str.Append($"calorie:[{calorie}],\n\r");
            str.Append($"planCalorie:[{planCalorie}],\n\r");

            return str.ToString();
        }
    }

    /// <summary>
    /// Each element is SportDailySummary
    /// </summary>
    public class SportDailySummaryList : DeserializableList<SportDailySummary>
    {
        public SportDailySummaryList(AndroidJavaObject obj)
        {
            int count = YVRPlatform.YVRSportGetDailySummarySize(obj);

            data = new List<SportDailySummary>(count);

            for (int i = 0; i < count; i++)
                data.Add(new SportDailySummary(YVRPlatform.YVRSportGetDailySummaryByIndex(obj, i)));
        }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            foreach (var item in data)
                str.Append(item + "\n\r");

            return str.ToString();
        }
    }
}