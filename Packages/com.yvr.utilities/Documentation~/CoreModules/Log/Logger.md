# Logger

Logger 负责具体的将 Log 输出的工作，所有实现都必须派生自 [LoggerBase](xref:YFramework.Log.LoggerBase) 类 。

对于一个 LoggerBase 类，主要有以下特性：

-   Log Handle 函数处理

    各派生类中必须实现 `DebugHandle`，`InfoHandle`, `WarnHandle`, `ErrorHandle` 四个函数作为 Log 输出的具体操作，如在 [UnityLogger](#unitylogger) 中 `DebugHandle` 的实现如下：

    ```csharp
    protected override void DebugHandle(string msg) { UnityEngine.Debug.Log(msg); }
    ```

    对于每个 Log 函数，都会调用其对应的 Handle 函数。

-   Logger 嵌套

    一个 Logger 还可以包含其他的 Logger，将被包含的 Logger 称为 `wrappedLogger`，对于每个 Logger 的 Log 函数会先触发其 `wrappedLogger` 的 Log 函数，再触发自身的 Log 函数。

-   LogPrefix 嵌套

    在 [LoggerBase](xref:YVR.Utilities.LoggerBase) 中默认的 Log 函数实现中，还会先调用 [MsgWithPrefix](<xref:YVR.Utilities.LoggerBase.MsgWithPrefix(System.String,System.String)>) 函数将需要输出的 Log 信息与由 [LogPrefix](./LogPrefix.md) 定义的 Log 前缀信息结合，再交由具体的 Logger 进行输出。

-   Log Controller 检查
    在 `LoggerBase` 的每一个 Log 输出前，会首先通过 [LoggerController](./LoggerController.md) 中的 [IsLogValid](<xref:YVR.Utilities.LoggerControllerBase.IsLogValid(System.Object,System.String,YVR.Utilities.LogPriority,System.String)>) 函数检查该 Log 是否应该被输出。当 Log 被认为不应该输出时，会调用函数 [OnInvalidLogImpl](<xref:YVR.Utilities.LoggerBase.OnInvalidLogImpl(System.Object,System.String,YVR.Utilities.LogPriority,System.String)>)，派生类可根据自己的需求重写该函数，并对过滤掉的 Log 进行二次处理，如可以选择保存至本地，或强制输出等。[YLogLogger](#yloglogger) 即重载了该函数，并在 Log 被过滤时，选择写入至 RamLog。

如下为使用嵌套 Logger 并结合 LogPrefix 的实例：

使用如下方法在 `LogTest` 类中定义了一个名为 `DoubleLogger` 的 Logger，并将其赋值给 `YVRLog`：

```csharp
ContextLogPrefix contextInfo = new ContextLogPrefix();
UnityLogger unityLogger = new UnityLogger();
UnityLogger doubleLogger = new UnityLogger(unityLogger, contextInfo);
YVRLog.SetLogger(doubleLogger);
```

此时调用在 `LogTest` 中调用 `this.Debug("This is a debug")` 的结果如下：

```text
This is a debug
[Context: Main Camera-Main Camera]  This is a debug
```

其中第一条 Log 由作为 `wrappedLogger` 的 `unityLogger` 打出，第二条 Log 由带有 `contextInfo` 的 `doubleLogger` 本身打出，在输出第二条时还结合了定义的 [ContextLogPrefix](./LogPrefix.md#contextlogprefix)。

目前实现了下述两种 Logger：

1. [UnityLogger](#unitylogger)：封装 Unity 默认的 `Debug.Log` 逻辑。
2. [YLogLogger](#yloglogger)：封装 SDK 中提供的 YLog 功能。

## UnityLogger

封装 Unity 默认的 `Debug` 逻辑，即`DebugHandle`，`InfoHandle`, `WarnHandle`, `ErrorHandle` 分别实际调用 `UnityEngine.Debug.Log`，`UnityEngine.Debug.Log`，`UnityEngine.Debug.LogWarning` 和 `UnityEngine.Debug.Error`。

当需要将 Log 输出到 Unity 的 Logcat 中以查看堆栈时，可使用该 Logger。

## YLogLogger

封装 SDK 中提供的 `YLog` 功能，`YLog` 拥有通过 android print 输出 Log 和向 RAM 中写入 Log 两种功能。具体 `YLog` 的实现根据平台分别通过 [AndroidAdapter](xref:YVR.Utilities.YLogAndroidAdapter) 和 [EditorAdapter](xref:YVR.Utilities.YLogEditorAdapter) 实现。

> [!Note]
> 通过 Android Print 输出至 logcat 中相较于 Unity 默认的输出有更高的性能，但 Unity Log 可提供更多的信息，如函数调用的堆栈等。因此对于需要查看堆栈的情况，建议使用 [UnityLogger](#unitylogger)，而其他情况建议使用 [YLogLogger](#yloglogger)。
> 经测试，使用 Unity Debug.Log 的耗时大约是 Android Print 的 100 倍。

> [!Note]
> 直接向内存中写入 Log 信息，相较于使用 Android Print 有更高的性能。因此可以将高频的信息写入内存中，并在需要的时候将数据从内存中保存至磁盘中查看。
> 经测试，使用 Android Print 的耗时大约是向 ram 中输出 log 的 5 倍。

[YLogLogger](xref:YVR.Utilities.YLogLogger) 中的 `DebugHandle`，`InfoHandle`, `WarnHandle`, `ErrorHandle` 实现为使用 android print 进行 Log 输出。

[YLogLogger](xref:YVR.Utilities.YLogLogger) 重写了 [OnInvalidLogImpl](<xref:YVR.Utilities.LoggerBase.OnInvalidLogImpl(System.Object,System.String,YVR.Utilities.LogPriority,System.String)>) 函数，以在 Log 被过滤时可以将 Log 写入内存中。

写入内存中的标准由 [ramLogPriorityTHold](xref:YVR.Utilities.YLogLogger.ramLogPriorityTHold) 决定，仅当输出的 Log 等级高于该等级时才会写入内存中。

> [!Caution]
> 因为仅当 Log 被过滤时，才会触发 [OnInvalidLogImpl](<xref:YVR.Utilities.LoggerBase.OnInvalidLogImpl(System.Object,System.String,YVR.Utilities.LogPriority,System.String)>) ，所以一条 Log 无法同时被输出到内存与 Logcat 中。

可通过函数 `ConfigureYLog` 对 `YLog` 进行配置，包括前缀名与为 RamLog 分配的内存大小（默认为 3 MB）。当输出到内存中的 Log 超过了预定大小，则该内存中最老的 Log 会被覆盖。

可通过函数 `SaveLog` 将内存中的 Log 保存至 `sdcard` 中。
