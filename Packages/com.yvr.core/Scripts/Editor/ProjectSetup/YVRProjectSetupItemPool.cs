using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace YVR.Core.Editor
{
    public class YVRProjectSetupItemPool : Singleton<YVRProjectSetupItemPool>
    {
        private Stack<VisualElement> itemPool = new Stack<VisualElement>();

        public VisualElement Get()
        {
            if (itemPool.Count > 0)
            {
                return itemPool.Pop();
            }
            return YVRProjectSetupResourcesLoader.instance.LoadElement<VisualTreeAsset>("YVRProjectSetupItemUIElement.uxml").Instantiate();
        }

        public void Return(VisualElement item)
        {
            itemPool.Push(item);
        }

    }
}
