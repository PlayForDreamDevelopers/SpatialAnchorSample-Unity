using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace YVR.Core.Editor
{
    public class YVRConfigurationTaskRegistry
    {
        private Dictionary<Hash128, YVRConfigurationTask> taskDic = new Dictionary<Hash128, YVRConfigurationTask>();
        private List<YVRConfigurationTask> tasks = new List<YVRConfigurationTask>();

        public void AddTask(YVRConfigurationTask task)
        {
            var uid = task.Uid;
            if (taskDic.ContainsKey(uid))
            {
                Debug.LogError($"task:{uid} with same uid already exist , {task.issueMessage}");
                return;
            }

            tasks.Add(task);
            taskDic.Add(uid, task);
        }

        public YVRConfigurationTask GetTask(Hash128 uid)
        {
            taskDic.TryGetValue(uid, out var task);
            return task;
        }

        public IEnumerable<YVRConfigurationTask> GetTasks()
        {
            return tasks;
        }
    }
}
