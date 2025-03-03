using System;
using UnityEngine;

namespace YVR.Platform
{
    /// <summary>
    /// The user's sport info.
    /// </summary>
    public class SportUserInfo
    {
        /// <summary>
        /// The user's gender, 1:man, 2:women, 3:unknown
        /// </summary>
        public readonly int gender;

        /// <summary>
        /// The user's year of birth
        /// </summary>
        public readonly int yearOfBirth;

        /// <summary>
        /// The height of the user (in cm).
        /// </summary>
        public readonly int stature;

        /// <summary>
        /// The weight of the user (in kg).
        /// </summary>
        public readonly int weight;

        /// <summary>
        /// The plan calories of the user.
        /// </summary>
        public readonly int planCalories;

        /// <summary>
        /// The planned daily sport duration (in seconds).
        /// </summary>
        public readonly int planDurationInSeconds;

        /// <summary>
        /// The planned weekly sport days.
        /// </summary>
        public readonly int daysPerWeek;

        public SportUserInfo(AndroidJavaObject obj)
        {
            gender = YVRPlatform.YVRSportGetGender(obj);
            yearOfBirth = YVRPlatform.YVRSportGetBirthday(obj);
            stature = YVRPlatform.YVRSportGetStature(obj);
            weight = YVRPlatform.YVRSportGetWeight(obj);
            planCalories = YVRPlatform.YVRSportGetPlanCalories(obj);
            planDurationInSeconds = YVRPlatform.YVRSportGetPlanDurationInSeconds(obj);
            daysPerWeek = YVRPlatform.YVRSportGetDaysPerWeek(obj);
        }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            str.Append($"gender:[{gender}],\n\r");
            str.Append($"birthday:[{yearOfBirth}],\n\r");
            str.Append($"stature:[{stature}],\n\r");
            str.Append($"weight:[{weight}],\n\r");
            str.Append($"planCalories:[{planCalories}],\n\r");
            str.Append($"planDurationInSeconds:[{planDurationInSeconds}],\n\r");
            str.Append($"daysPerWeek:[{daysPerWeek}],\n\r");

            return str.ToString();
        }
    }
}