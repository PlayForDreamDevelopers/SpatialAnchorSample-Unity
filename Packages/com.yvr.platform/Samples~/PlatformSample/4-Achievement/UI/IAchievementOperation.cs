using System;
using YVR.Platform;

public interface IAchievementOperation
{
    void Initialize(AchievementDefinition definition);
    void UpdateProgress(AchievementProgress progress);
}