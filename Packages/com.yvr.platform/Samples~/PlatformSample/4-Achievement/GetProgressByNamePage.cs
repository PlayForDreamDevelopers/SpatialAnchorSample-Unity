using UnityEngine;
using YVR.Platform;

public class GetProgressByNamePage : OperationPage
{
   protected override void OnClickExecute()
   {
      base.OnClickExecute();

      GetProgressByName();
   }

   private void GetProgressByName()
   {
      string[] names = GetInputValueByType(InputOptionType.AchievementName).Split(';');

      Achievement.GetProgressByName(names).OnComplete(GetProgressByNameCallback);
   }

   private void GetProgressByNameCallback(YVRMessage<AchievementProgressList> msg)
   {
      if (msg.isError)
      {
         Debug.LogError($"GetProgressByNamePage.GetProgressByNameCallback: error -> {msg.error}");
         m_TextResult.text = msg.error.ToString();
      }
      else
      {
         Debug.Log($"GetProgressByNamePage.GetProgressByNameCallback: progress -> {msg}");
         m_TextResult.text = msg.data.ToString();
      }
   }
}
