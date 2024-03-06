# 为什么需要 Project Settings？

目前一些工程中的配置，已经改由 ScriptObject 进行配置，进而达到数据与业务结构的期望。如 `YVRLogConfigSO`：

```C#
    public class YVRLogConfigSO :ScriptableObject
    {
        public bool enable = true;
        public string tag = "Unity";
        public List<LoggerConfig> loggerConfigs;

        public void Apply()
        {
            YVRLog.enable = enable;
            YVRLog.ClearLoggers();
            YLogLogger.ConfigureYLog(tag);

            loggerConfigs?.ForEach(cfg =>
            {
                LoggerBase logger = cfg.loggerType.ToLogger();
                logger.SetPrefix(cfg.prefixType.ToLogPrefix());
                logger.SetController(cfg.ToLoggerController());
                YVRLog.RegisterLogger(logger);
            });
         }
    }

```

通常该 SO 的实例需要被一个 `MonoBehavior` 引用，`MonoBehavior` 负责读取 SO 实例，并使用其中的设置的 Property 来执行进一步的设置，如 `YVRLogConfigSO` 需要通过 `YVRLogController` 进行初始化：

```C#
namespace YVR.Utilities
{
    public class YVRLogController : MonoBehaviour
    {
        [SerializeField] private YVRLogConfigSO m_Config = null;
        // ...

        public YVRLogConfig config { get; private set; }

        private void Start()
        {
            // ...
            ConfigYVRLog(config);
        }

        public void ConfigYVRLog(YVRLogConfig config = null)
        {
            this.config = config ? config : this.config;
            this.config?.Apply();
        }
    }
}
```

可以看到在 `YVRLogController` 的 `Start` 函数中，根据 `YVRLogConfigSO` 的实例进行相应关于 Log 的配置。

> [!Note]
> 下将这类用于 _引用 SO 并通过其中数据进行相应操作的 MonoBehavior_，简称为 Controller

这种做法本身是合理的，但如果该 SO 实际上是一个 **项目级别** 的设置，则存在以下问题：

1. SO 的初始化依赖于 Controller：这意味着场景中必须存在有挂载了该 Controller 的游戏物体，对于多个场景而言增加了开发者的维护负担
    - 明明是 **项目级别** 的设置，却需要每一个场景都做适配
2. Controller 与业务代码存在时序问题：如有业务 MonoBehavior，需要依赖 SO 中配置的某一个行为，但此时 Controller 尚未初始化，则会出现 Bug。

直观上，上述问题可以通过 `MonoBehaviorSingleton` 解决，即让生成挂载了 Controller 游戏物体的操作无感化 ，也通过单例保证了在调用前，资源一定经过了初始化。但这样做的问题是：

1. 需要解决单例模式下 SO 示例的获取问题：对于挂载在 GameObject 上的 Controller，可以通过 Inspector 面板拖拽的方式获取到 SO 的示例。但对于单例而言，如果要在运行时获取到示例，则必须通过写死资源的路径的方式来动态加载 SO 实例，此时会引发额外问题：
    - 写死的路径造成了可维护性的降低
    - 脚本需要关心加载实例的方式，脚本中需要包含有资源加载的逻辑，且很可能需要支持多份（Resources / AB 等等）
2. 多线程调用的问题：如果在非 UI 线程进行了单例的调用，此时如果 SO 实例尚未加载，则会发生尝试在非 UI 线程调用 Unity API 的问题。

所以，为了解决上述问题，我们引入了 Project Settings。