using System;
using System.Reflection;
using UnityEngine;
using YVR.Utilities;

public static class ComponentExtension
{
    public static T AutoAddingGetComponent<T>(this Component target) where T : Component
    {
        var result = target.GetComponent<T>();
        if (result == null) result = target.gameObject.AddComponent<T>();
        return result;
    }

    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (type != other.GetType()) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                             BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pInfos = type.GetProperties(flags);
        foreach (PropertyInfo info in pInfos)
        {
            if (!info.CanWrite) continue;
            try
            {
                info.SetValue(comp, info.GetValue(other, null), null);
            } catch (Exception)
            {
                // In case of NotImplementedException being thrown. 
                // For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }

        FieldInfo[] fInfos = type.GetFields(flags);
        foreach (FieldInfo info in fInfos)
        {
            info.SetValue(comp, info.GetValue(other));
        }

        return comp as T;
    }

    public static T SetActive<T>(this T selfComponent, bool active) where T : Component
    {
        return active ? selfComponent.Show() : selfComponent.Hide();
    }


    public static T Show<T>(this T selfComponent) where T : Component
    {
        selfComponent?.gameObject.Show();
        return selfComponent;
    }


    public static T Hide<T>(this T selfComponent) where T : Component
    {
        selfComponent.gameObject.Hide();
        return selfComponent;
    }
}