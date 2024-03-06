using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YVR.Core.Editor
{
    public class YVRConfigurationTaskProcessor
    {
        private event Action<YVRConfigurationTaskProcessor> onComplete;

        private Func<IEnumerable<YVRConfigurationTask>, List<YVRConfigurationTask>> filter { get; }

        private YVRConfigurationTaskRegistry registry { get; }

        private IEnumerator<YVRConfigurationTask> m_Enumerator;

        private List<YVRConfigurationTask> m_FilteredTasks;

        public YVRConfigurationTaskProcessor(Func<IEnumerable<YVRConfigurationTask>, List<YVRConfigurationTask>> filter, YVRConfigurationTaskRegistry registry, Action<YVRConfigurationTaskProcessor> onComplete)
        {
            this.filter = filter;
            this.registry = registry;
            this.onComplete += onComplete;
        }

        //private int m_StartTime;

        private bool m_started = false;
        public bool started => m_started;//m_StartTime != -1;
        public bool processing => m_Enumerator != null;
        public bool completed => started && (m_Enumerator == null || m_Enumerator.Current == null);

        public void Start()
        {
            PrepareTask();
            m_started = true;
            //m_StartTime = Environment.TickCount;
        }

        public void Update()
        {
            while (m_Enumerator?.Current != null)
            {
                ProcessTask(m_Enumerator.Current);
                m_Enumerator.MoveNext();
            }
            m_Enumerator = null;
            onComplete?.Invoke(this);
        }

        private void PrepareTask()
        {
            var tasks = registry.GetTasks();
            m_FilteredTasks = filter != null ? filter(tasks) : tasks.ToList();
            m_Enumerator = m_FilteredTasks.GetEnumerator();
            m_Enumerator.MoveNext();
        }

        private void ProcessTask(YVRConfigurationTask task)
        {
            task.Fix();
        }
    }
}
