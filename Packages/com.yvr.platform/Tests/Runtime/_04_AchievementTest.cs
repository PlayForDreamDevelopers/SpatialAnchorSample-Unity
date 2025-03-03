using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class _04_AchievementTest
{
    private  string m_NormaAchievementName = "achievement01";
    private  string m_NumberBasedAchievementName = "achievement02";
    private  string m_BitfieldAchievementName = "achievement04";
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        LogAssert.ignoreFailingMessages = true;
    }
    
    [UnityTest, Order(0)]
    public IEnumerator _00_Achievement_GetAllDefinitions_LogCallbackName()
    {
        PlatformTester.GetAllAchievementDefinitions();

        yield return new WaitForSecondsRealtime(2f);

        LogAssert.Expect(LogType.Log, nameof(PlatformTester.GetAllAchievementDefinitionsCallback));
    }

    [UnityTest, Order(1)]
    public IEnumerator _01_Achievement_GetAchievementDefinitionsByName_LogCallbackName()
    {
        PlatformTester.GetAchievementDefinitionByName(m_NormaAchievementName);

        yield return new WaitForSecondsRealtime(2f);

        LogAssert.Expect(LogType.Log, nameof(PlatformTester.GetAchievementDefinitionByNameCallback));
    }

    [UnityTest, Order(2)]
    public IEnumerator _02_Achievement_GetAllProgress_LogCallbackName()
    {
        PlatformTester.GetAllAchievementProgress();

        yield return new WaitForSecondsRealtime(2f);

        LogAssert.Expect(LogType.Log, nameof(PlatformTester.GetAllAchievementProgressCallback));
    }

    [UnityTest, Order(3)]
    public IEnumerator _03_Achievement_GetAchievementProgressByName_LogCallbackName()
    {
        PlatformTester.GetAchievementProgressByName(m_NormaAchievementName);

        yield return new WaitForSecondsRealtime(2f);

        LogAssert.Expect(LogType.Log, nameof(PlatformTester.GetAchievementProgressByNameCallback));
    }

    [UnityTest, Order(4)]
    public IEnumerator _04_Achievement_AchievementAddCount_CountIncreasedByOne()
    {
        PlatformTester.GetAchievementProgressByName(m_NumberBasedAchievementName);

        yield return new WaitForSecondsRealtime(2f);
        int beforeProgress = PlatformTester.achievementProgress;
        
        PlatformTester.AchievementAddCount(m_NumberBasedAchievementName, 1);
        
        yield return new WaitForSecondsRealtime(2f);
        PlatformTester.GetAchievementProgressByName(m_NumberBasedAchievementName);
        
        yield return new WaitForSecondsRealtime(2f);
        int afterProgress = PlatformTester.achievementProgress;

        LogAssert.Expect(LogType.Log, nameof(PlatformTester.AchievementAddCountCallback));
        Assert.That(afterProgress, Is.EqualTo(beforeProgress + 1));
    }
    
    [UnityTest, Order(5)]
    public IEnumerator _05_Achievement_AchievementAddField_LogCallbackName()
    {
        PlatformTester.AchievementAddFields(m_BitfieldAchievementName, "001");

        yield return new WaitForSecondsRealtime(2f);

        LogAssert.Expect(LogType.Log, nameof(PlatformTester.AchievementAddFieldsCallback));
    }
    
    [UnityTest, Order(6)]
    public IEnumerator _06_Achievement_AchievementUnlock_LogCallbackName()
    {
        PlatformTester.UnlockAchievement(m_NormaAchievementName);

        yield return new WaitForSecondsRealtime(2f);
        
        LogAssert.Expect(LogType.Log, nameof(PlatformTester.UnlockAchievementCallback));
    }
}