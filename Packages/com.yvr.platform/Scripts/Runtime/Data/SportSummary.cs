using UnityEngine;

namespace YVR.Platform
{
    /// <summary>
    /// User's sport summary of today.
    /// </summary>
    public class SportSummary
    {
        /// <summary>
        /// The sport duration (in seconds).
        /// </summary>
        public readonly long durationInSeconds;

        /// <summary>
        /// The calorie burnt (in kilo calorie).
        /// </summary>
        public readonly double calorie;

        public SportSummary(AndroidJavaObject obj)
        {
            durationInSeconds = YVRPlatform.YVRSportGetSummaryDurationInSeconds(obj);
            calorie = YVRPlatform.YVRSportGetSummaryCalorie(obj);
        }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            str.Append($"durationInSeconds:[{durationInSeconds}],\n\r");
            str.Append($"calorie:[{calorie}],\n\r");

            return str.ToString();
        }
    }
}