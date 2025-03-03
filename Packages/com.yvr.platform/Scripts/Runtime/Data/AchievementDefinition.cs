using System.Collections.Generic;
using UnityEngine;

namespace YVR.Platform
{
    /// <summary>
    /// Encapsulate properties of achievement definition data
    /// </summary>
    public class AchievementDefinition
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
        /// Type of achievement
        /// </summary>
        public readonly int type;

        /// <summary>
        /// Policy of achievement
        /// </summary>
        public readonly int entryWritePolicy;

        /// <summary>
        /// Target of achievement
        /// </summary>
        public readonly int target;

        /// <summary>
        /// Bit field length of achievement
        /// </summary>
        public readonly int bitfieldLength;

        /// <summary>
        /// Is this achievement archived
        /// </summary>
        public readonly bool isArchived;

        /// <summary>
        /// Title of achievement
        /// </summary>
        public readonly string title;

        /// <summary>
        /// Description of achievement
        /// </summary>
        public readonly string description;

        /// <summary>
        /// Unlock description of achievement
        /// </summary>
        public readonly string unlockedDescription;

        /// <summary>
        /// Is this achievement secret
        /// </summary>
        public readonly bool isSecret;

        /// <summary>
        /// Locked image url of achievement
        /// </summary>
        public readonly string lockedImageFile;

        /// <summary>
        /// Unlocked image url of achievement
        /// </summary>
        public readonly string unlockedImageFile;

        /// <summary>
        /// Time when this achievement created
        /// </summary>
        public readonly long createTime;

        /// <summary>
        /// Time when this achievement updated
        /// </summary>
        public readonly long updateTime;

        public AchievementDefinition(AndroidJavaObject obj)
        {
            id = YVRPlatform.YVRAchievementDefinitionGetIdFromElementOfDefinitions(obj);
            apiName = YVRPlatform.YVRAchievementDefinitionGetApiNameFromElementOfDefinitions(obj);
            type = YVRPlatform.YVRAchievementDefinitionGetTypeFromElementOfDefinitions(obj);
            entryWritePolicy = YVRPlatform.YVRAchievementDefinitionGetPolicyFromElementOfDefinitions(obj);
            target = YVRPlatform.YVRAchievementDefinitionGetTargetFromElementOfDefinitions(obj);
            bitfieldLength = YVRPlatform.YVRAchievementDefinitionGetBitfieldLengthFromElementOfDefinitions(obj);
            isArchived = YVRPlatform.YVRAchievementDefinitionGetIsAchievedFromElementOfDefinitions(obj);
            title = YVRPlatform.YVRAchievementDefinitionGetTitleFromElementOfDefinitions(obj);
            description = YVRPlatform.YVRAchievementDefinitionGetDescriptionFromElementOfDefinitions(obj);
            unlockedDescription = YVRPlatform.YVRAchievementDefinitionGetUnlockedDescriptionFromElementOfDefinitions(obj);
            isSecret = YVRPlatform.YVRAchievementDefinitionGetIsSecretFromElementOfDefinitions(obj);
            lockedImageFile = YVRPlatform.YVRAchievementDefinitionGetLockedImageFromElementOfDefinitions(obj);
            unlockedImageFile = YVRPlatform.YVRAchievementDefinitionGetUnlockedImageFromElementOfDefinitions(obj);
            createTime = YVRPlatform.YVRAchievementDefinitionGetCreatedTimeFromElementOfDefinitions(obj);
            updateTime = YVRPlatform.YVRAchievementDefinitionGetUpdateTimeFromElementOfDefinitions(obj);
        }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            str.Append($"id:[{id}],\n\r");
            str.Append($"api name:[{apiName ?? "null"}],\n\r");
            str.Append($"type:[{type}],\n\r");
            str.Append($"entryWritePolicy:[{entryWritePolicy}],\n\r");
            str.Append($"target:[{target}],\n\r");
            str.Append($"bitfieldLength:[{bitfieldLength}],\n\r");
            str.Append($"isArchived:[{isArchived}],\n\r");
            str.Append($"title:[{title ?? "null"}],\n\r");
            str.Append($"description:[{description ?? "null"}],\n\r");
            str.Append($"unlockedDescription:[{unlockedDescription ?? "null"}],\n\r");
            str.Append($"isSecret:[{isSecret}],\n\r");
            str.Append($"lockedImageFile:[{lockedImageFile ?? "null"}],\n\r");
            str.Append($"unlockedImageFile:[{unlockedImageFile ?? "null"}],\n\r");
            str.Append($"createTime:[{createTime}],\n\r");
            str.Append($"updateTime:[{updateTime}],\n\r");

            return str.ToString();
        }
    }

    /// <summary>
    /// The storage of achievement definition data
    /// </summary>
    public class AchievementDefinitionList : DeserializableList<AchievementDefinition>
    {
        public AchievementDefinitionList(AndroidJavaObject obj)
        {
            int count = YVRPlatform.YVRAchievementDefinitionGetSizeOfAllDefinitions(obj);

            data = new List<AchievementDefinition>(count);

            for (int i = 0; i < count; i++)
                data.Add(new AchievementDefinition(
                    YVRPlatform.YVRAchievementDefinitionGetElementOfDefinitions(obj, i)));
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