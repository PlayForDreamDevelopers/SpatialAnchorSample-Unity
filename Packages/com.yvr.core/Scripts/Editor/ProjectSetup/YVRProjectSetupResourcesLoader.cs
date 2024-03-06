using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace YVR.Core.Editor
{
    public class YVRProjectSetupResourcesLoader : Singleton<YVRProjectSetupResourcesLoader>
    {
        private Dictionary<string, Object> resourceDic = new Dictionary<string, Object>();

        private string m_ElementPath = string.Empty;
        private string elementPath
        {
            get
            {
                if (string.IsNullOrEmpty(m_ElementPath))
                {
                    var projectSetupScriptAsset = AssetDatabase.FindAssets($"YVRProjectSetupItemUIElement");
                    m_ElementPath = AssetDatabase.GUIDToAssetPath(projectSetupScriptAsset[0]);
                    m_ElementPath = Path.GetDirectoryName(m_ElementPath);
                }
                return m_ElementPath;
            }
        }

        private string m_TexturePath = string.Empty;
        private string texturePath
        {
            get
            {
                if (string.IsNullOrEmpty(m_TexturePath))
                {
                    var projectSetupTextureAsset = AssetDatabase.FindAssets($"t:Texture project ProjectSetup_Fixed");
                    m_TexturePath = AssetDatabase.GUIDToAssetPath(projectSetupTextureAsset[0]);
                    m_TexturePath = Path.GetDirectoryName(m_TexturePath);
                }
                return m_TexturePath;
            }
        }

        public T LoadResource<T>(string resourceName) where T : Object
        {
            if (resourceDic.ContainsKey(resourceName))
            {
                return resourceDic[resourceName] as T;
            }
            
            T resource = AssetDatabase.LoadAssetAtPath(resourceName, typeof(T)) as T; //Resources.Load<T>(resourceName);

            resourceDic.Add(resourceName, resource);
            return resource;
        }

        public T LoadElement<T>(string elementName) where T : Object
        {
            string resourcePath = Path.Combine(elementPath, elementName);
            var resource = LoadResource<T>(resourcePath);
            return resource;
        }

        public T LoadTexture<T>(string textureName) where T : Object
        {
            string resourcePath = Path.Combine(texturePath, textureName);
            return LoadResource<T>(resourcePath);
        }

        public GUIContent LoadIcon(string name)
        {
            var texture = LoadTexture<Texture2D>(name);
            GUIContent content = new GUIContent(texture);
            return content;
        }

        public void UnloadAllResource()
        {
            resourceDic.Clear();
        }
    }
}
