using System.Collections.Generic;
using UnityEngine;

namespace YVR.Platform
{
    /// <summary>
    /// Encapsulate properties of achievement progress data
    /// </summary>
    public class AchievementProgress
    {
        /// <summary>
        /// ID of achievement
        /// </summary>
        public readonly int id;

        /// <summary>
        /// Api name of achievement
        /// </summary>
        public readonly string apiName;

        /// <summary>
        /// target of achievement
        /// </summary>
        public readonly int target;

        /// <summary>
        /// Count progress of achievement
        /// </summary>
        public readonly int countProgress;

        /// <summary>
        /// Bitfield progress of achievement
        /// </summary>
        public readonly string bitfieldProgress;

        /// <summary>
        /// Is this achievement unlocked
        /// </summary>
        public readonly bool isUnlocked;

        /// <summary>
        /// Time when this achievement unlocked
        /// </summary>
        public readonly long unlockTime;

        public AchievementProgress(AndroidJavaObject obj)
        {
            id = YVRPlatform.YVRAchievementProgressGetIdFromElementOfAchievementProgress(obj);
            countProgress = YVRPlatform.YVRAchievementProgressGetCountProgressFromElementOfAchievementProgress(obj);
            bitfieldProgress = YVRPlatform.YVRAchievementProgressGetBitfieldProgressFromElementOfAchievementProgress(obj);
            isUnlocked = YVRPlatform.YVRAchievementProgressIsUnlockedFromElementOfAchievementProgress(obj);
            unlockTime = YVRPlatform.YVRAchievementProgressGetUnlockTimeFromElementOfAchievementProgress(obj);
            AndroidJavaObject definition = YVRPlatform.YVRAchievementProgressGetDefinitionFromElementOfAchievementProgress(obj);
            apiName = YVRPlatform.YVRAchievementProgressGetNameFromDefinitionOfAchievementProgress(definition);
            target = YVRPlatform.YVRAchievementProgressGetTargetFromDefinitionOfAchievementProgress(definition);
        }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            str.Append($"id:[{id}],\n\r");
            str.Append($"api name:[{apiName ?? "null"}],\n\r");
            str.Append($"target:[{target}],\n\r");
            str.Append($"countProgress:[{countProgress}],\n\r");
            str.Append($"bitfieldProgress:[{bitfieldProgress ?? "null"}],\n\r");
            str.Append($"isUnlocked:[{isUnlocked}],\n\r");
            str.Append($"unlockTime:[{unlockTime}],\n\r");

            return str.ToString();
        }
    }

    /// <summary>
    /// The storage of achievement progress data
    /// </summary>
    public class AchievementProgressList : DeserializableList<AchievementProgress>
    {
        public AchievementProgressList(AndroidJavaObject obj)
        {
            int count = YVRPlatform.YVRAchievementProgressGetSizeOfAchievementProgress(obj);

            data = new List<AchievementProgress>(count);

            for (int i = 0; i < count; i++)
                data.Add(new AchievementProgress(
                    YVRPlatform.YVRAchievementProgressGetElementOfAchievementProgress(obj, i)));
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