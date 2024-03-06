using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;
using YVR.Core.Editor;

namespace YVR.Core.Editor
{
    public class YVRConfigurationSummary
    {
        public event Action<YVRConfigurationSummary> onSummaryUpdated;

        private Dictionary<YVRProjectSetup.TaskLevel, List<YVRConfigurationTask>> m_OutStandingTasksDic;
        private YVRConfigurationTaskRegistry registry { get; }

        private IEnumerator<YVRConfigurationTask> m_Enumerator;

        public YVRConfigurationSummary(YVRConfigurationTaskRegistry registry)
        {
            this.registry = registry;
            m_OutStandingTasksDic = new Dictionary<YVRProjectSetup.TaskLevel, List<YVRConfigurationTask>>();
            for (var i = YVRProjectSetup.TaskLevel.Required; i >= YVRProjectSetup.TaskLevel.Optional; i--)
            {
                m_OutStandingTasksDic.Add(i, new List<YVRConfigurationTask>());
            }
        }

        private void Reset()
        {
            for (var i = YVRProjectSetup.TaskLevel.Required; i >= YVRProjectSetup.TaskLevel.Optional; i--)
            {
                m_OutStandingTasksDic[i].Clear();
            }
        }

        private void UpdateOutStandingTask()
        {
            var tasks = registry.GetTasks();
            m_Enumerator = tasks.GetEnumerator();
            m_Enumerator.MoveNext();
            while (m_Enumerator?.Current != null)
            {
                YVRConfigurationTask task = m_Enumerator.Current;
                if (!task.isDone())
                {
                    m_OutStandingTasksDic[task.taskLevel].Add(task);
                }
                m_Enumerator.MoveNext();

            }
        }

        public void Update()
        {
            Reset();
            UpdateOutStandingTask();
            onSummaryUpdated?.Invoke(this);
        }

        public string GetOutStandingString(ref YVRProjectSetup.TaskLevel level, ref int count)
        {
            for (var i = YVRProjectSetup.TaskLevel.Required; i >= YVRProjectSetup.TaskLevel.Optional; i--)
            {
                if (m_OutStandingTasksDic[i].Count > 0)
                {
                    level = i;
                    count = m_OutStandingTasksDic[i].Count;
                    string isOrAre = count == 1 ? "is" : "are";
                    string outStandingLevel = i == YVRProjectSetup.TaskLevel.Required ? "critical issue" : "recommended items";
                    return $"There {isOrAre} {count} {outStandingLevel}, Open YVR Unity Project Setup Tool";
                }
            }
            level = default;
            count = 0;
            return "The YVR Unity Project Setup Tool detected no issues";
        }
    }
}