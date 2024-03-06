public abstract class Singleton<T> where T : Singleton<T>, new()
{
    private static object s_Locker = new object();

    private static T s_Instance;

    public static T instance
    {
        get
        {
            if (s_Instance == null)
            {
                lock (s_Locker)
                {
                    if (s_Instance == null)
                    {
                        // Must first init instance, then sent to s_Instance
                        var t = new T();
                        t.OnInit();
                        s_Instance = t;
                    }
                }
            }

            return s_Instance;
        }
    }

    public static T createdInstance => s_Instance;

    protected virtual void OnInit() { }

    /// <summary>
    /// Reset the instance to null which will force the next call to instance to re-initialize the singleton.
    /// </summary>
    public void Reset() { s_Instance = null; }
}