using System;

namespace YVR.Platform
{
    public static class Sport
    {
        /// <summary>
        /// Gets a user's basic information and exercise plan.
        /// </summary>
        /// <returns></returns>
        public static YVRRequest<SportUserInfo> GetUserInfo()
        {
            return YVRPlatform.isInitialized
                ? new YVRRequest<SportUserInfo>(YVRPlatform.YVRSportGetUserInfo())
                : null;
        }

        /// <summary>
        /// Get a summary of the user's exercise data for a specified period within the recent 24 hours. The period should not exceed 24 hours.
        /// </summary>
        /// <param name="beginTime">A DateTime struct defining the begin time of the period. The begin time should be no earlier than 24 hours before the current time.</param>
        /// <param name="endTime">A DateTime struct defining the end time of the period.</param>
        /// <returns></returns>
        public static YVRRequest<SportSummary> GetSummary(DateTime beginTime, DateTime endTime)
        {
            return YVRPlatform.isInitialized
                ? new YVRRequest<SportSummary>(YVRPlatform.YVRSportGetSummary(beginTime.ToUnixTimeMillisecond(),
                                                                              endTime.ToUnixTimeMillisecond()))
                : null;
        }

        /// <summary>
        /// Get a summary of the user's exercise data for a specified period within the recent 24 hours. The period should not exceed 24 hours.
        /// </summary>
        /// <param name="beginUnixTimeMillisecond">A unix time in millisecond defining the begin time of the period. The begin time should be no earlier than 24 hours before the current time.</param>
        /// <param name="endUnixTimeMillisecond">A unix time in millisecond defining the end time of the period.</param>
        /// <returns></returns>
        public static YVRRequest<SportSummary> GetSummary(long beginUnixTimeMillisecond, long endUnixTimeMillisecond)
        {
            return YVRPlatform.isInitialized
                ? new YVRRequest<SportSummary>(YVRPlatform.YVRSportGetSummary(beginUnixTimeMillisecond,
                                                                              endUnixTimeMillisecond))
                : null;
        }

        /// <summary>
        /// Gets a summary of the user's daily exercise data for a specified period within the recent 90 days.
        /// </summary>
        /// <param name="beginTime">A DateTime struct defining the begin time of the period. The begin time should be no earlier than 90 days before the current time.</param>
        /// <param name="endTime">A DateTime struct defining the end time of the period, .</param>
        /// <returns></returns>
        public static YVRRequest<SportDailySummaryList> GetDailySummary(DateTime beginTime, DateTime endTime)
        {
            return YVRPlatform.isInitialized
                ? new YVRRequest<SportDailySummaryList>(YVRPlatform
                                                           .YVRSportGetDailySummary(beginTime.ToUnixTimeMillisecond(),
                                                             endTime.ToUnixTimeMillisecond()))
                : null;
        }

        /// <summary>
        /// Gets a summary of the user's daily exercise data for a specified period within the recent 90 days.
        /// </summary>
        /// <param name="beginUnixTimeMillisecond">A unix time in millisecond defining the begin time of the period. The begin time should be no earlier than 90 days before the current time.</param>
        /// <param name="endUnixTimeMillisecond">A unix time in millisecond defining the end time of the period, .</param>
        /// <returns></returns>
        public static YVRRequest<SportDailySummaryList> GetDailySummary(long beginUnixTimeMillisecond,
                                                                        long endUnixTimeMillisecond)
        {
            return YVRPlatform.isInitialized
                ? new YVRRequest<SportDailySummaryList>(YVRPlatform
                                                           .YVRSportGetDailySummary(beginUnixTimeMillisecond,
                                                             endUnixTimeMillisecond))
                : null;
        }
    }
}