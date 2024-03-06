using UnityEngine;

public abstract class MonoBehaviorSingleton<T> : MonoBehaviour where T : MonoBehaviorSingleton<T>
{
    private bool m_Initialized = false;

    private static object s_Locker = new object();
    private static T s_Instance;

    public static T instance
    {
        get
        {
            if (!s_Instance)
            {
                // Ensure Thread Safety
                lock (s_Locker)
                {
                    if (!s_Instance)
                    {
                        var ins = FindObjectOfType<T>();
                        if (!ins)
                        {
                            // When there is no such object in the scene, create an object that will not be destroyed with the scene switch to add the component,
                            // so as to ensure that it is not empty when used
                            ins = new GameObject(typeof(T).Name).AddComponent<T>();
                            Debug.Log($"The [{typeof(T).Name}] type was not found in the scene, a new one was created");
                            DontDestroyOnLoad(ins.gameObject);
                        }

                        if (ins != null && !ins.m_Initialized)
                            ins.Init();

                        s_Instance = ins;
                    }
                }
            }

            return s_Instance;
        }
    }

    public static T createdInstance => s_Instance;

    protected virtual void Init()
    {
        m_Initialized = true;
        s_Instance = (T) this;
    }

    protected virtual void Start()
    {
        if (!m_Initialized)
            Init();
    }
}