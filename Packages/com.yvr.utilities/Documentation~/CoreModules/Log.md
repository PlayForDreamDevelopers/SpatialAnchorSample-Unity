# Overview

Log 模块旨在为 Unity 默认的 `UnityEngin.Debug` 模块添加如下功能：

1. 使用相同的接口封装不同实现的 Logger，如直接使用 Unity Debug 的 [UnityLogger](xref:YVR.Utilities.UnityLogger)，使用安卓 print 的 [YLogLogger](xref:YVR.Utilities.YLogLogger)
2. 为 Log 增控制能力，如使用 [LoggerPriorityController](xref:YVR.Utilities.LoggerPriorityController) 控制 Runtime 时设置输出 Log 的过滤级
3. 全局为输出的 Log 增加特定信息，如使用 [ContextLogPrefix](xref:YVR.Utilities.ContextLogPrefix) 在每条 Log 输出前添加打出该 Log 的游戏物体的名称

该模块主要分为如下部分：

1. [YVRLog](./Log/YVRLog.md)：模块的核心，实现了对各 Logger 的管理控制，并通过拓展函数的方式为 `system.object` 类提供输出 Log 的方式。
2. [Logger](./Log/Logger.md)：各 Logger 的具体实现
3. [LogPrefix](./Log/LogPrefix.md)：为各 Log 提供增加前缀信息的功能。
4. [LoggerController](./Log/LoggerController.md)：为 Logger 提供控制函数
5. [YVRLogController](./Log/YVRLogController.md): 为 YVRLog 提供工程级别的配置，可根据工程中的 [YVRLogConfig](xref:YVR.Utilities.YVRLogConfig) 或本地的 Json 文件进行配置。


其中 [Logger](./Log/Logger.md)，[LogPrefix](./Log/LogPrefix.md)，[LoggerController](./Log/LoggerController.md) 都以装饰器模式设计，保证了高拓展性。

# Example

> [!Note]
> 下例子只是展现了 YVRLog 的功能，实际使用中，通常不需要编写诸如 `YVRLog.RegisterLogger` 这样的样板代码，而可以通过 [YVRLogController](./Log/YVRLogController.md) 进行配置。

## 注册 Logger，配置 LogPrefix，LoggerController

下例子实现了在 YVRLog 中同时注册两个 Logger，分别为 [UnityLogger](xref:YVR.Utilities.UnityLogger) 和 [YLogLogger](xref:YVR.Utilities.YLogLogger)。

针对 `UnityLogger` 和 `YLogLogger` 各设置了一个 [ContextLogPrefix](xref:YVR.Utilities.ContextLogPrefix)——`m_ContextCustomPrefix4YLog` 和 `m_ContextCustomPrefix4Unity`，这两个 `ContextLogPrefix` 都各自内包含了一个 [CustomLogPrefix](xref:YVR.Utilities.CustomLogPrefix)—— `m_CustomLogPrefix4YLog` 和 `m_CustomLogPrefix4YUnity`。此时输出每一条语句，都会带上 [CustomLogPrefix](xref:YVR.Utilities.CustomLogPrefix) 的自定义前缀和 [ContextLogPrefix](xref:YVR.Utilities.ContextLogPrefix) 的上下文信息。

定义了两个 [LoggerPrefixPriorityController](xref:YVR.Utilities.LoggerPrefixPriorityController) 为 Logger 提供根据前缀修改优先级的控制： `m_YLoggerPrefixPriorityController` 和 `m_UnityPrefixPriorityController` 分别赋值给 `UnityLogger` 和 `YLogLogger`。

```csharp

m_YLogLogger = new YLogLogger();

m_UnityLogger = new UnityLogger();
m_CustomLogPrefix4YLog = new CustomLogPrefix("Prefix 4 YLog");
m_CustomLogPrefix4YUnity = new CustomLogPrefix("Prefix 4 Unity");
m_ContextCustomPrefix4YLog = new ContextLogPrefix(m_CustomLogPrefix4YLog);
m_ContextCustomPrefix4Unity = new ContextLogPrefix(m_CustomLogPrefix4YUnity);
m_YLogLogger.SetPrefix(m_CustomLogPrefix4YLog);
m_UnityLogger.SetPrefix(m_CustomLogPrefix4YUnit

m_YLoggerPrefixPriorityController = new LoggerPrefixPriorityController();
m_UnityPrefixPriorityController = new LoggerPrefixPriorityController
m_YLogLogger.SetController(m_YLoggerPrefixPriorityController);
m_UnityLogger.SetController(m_UnityPrefixPriorityController);

YVRLog.ClearLoggers();
YVRLog.RegisterLogger(m_YLogLogger);
YVRLog.RegisterLogger(m_UnityLogger);
```

## 配置 LoggerPrefixPriorityController

将 `m_YLoggerPrefixPriorityController` 的过滤等级设为最低，将 `m_UnityPrefixPriorityController` 的过滤等级设为最高，即默认情况下，`UnityLogger` 不会输出任何的 Log，而 `YLogLogger` 会输出所有的 Log。

对于 `m_UnityPrefixPriorityController` 将前缀包含有 `ClassForTest` 类型的 Log 过滤等级设为最低，即 `UnityLogger` 会输出任何前缀带有 `ClassForTest` 字样的 Log。

```csharp
// ...
m_UnityPrefixPriorityController.priority = LogPriority.Highest;

m_UnityPrefixPriorityController.AddPrefix2PriorityMap(nameof(ClassForTest), LogPriority.Lowest);

m_YLogLogger.SetPrefix(m_ContextCustomPrefix4YLog);
m_UnityLogger.SetPrefix(m_ContextCustomPrefix4Unity);
```

## 日志输出

使用如下三条语句测试 YVRLog 的表现：
```csharp
this.Debug("Call Debug");

m_ClassForTest.Debug("Debug for class");
```

其中，`this.Debug` 语句仅会产生如下 Log 输出，因为默认 UnityLogger 不会输出任何的 Log。

```text
[Prefix 4 YLog Context: MultiLoggerAMultiControllersTests] Call Debug
```

`m_ClassForTest.Debug("Debug for class");` 会输出两条语句，因为这条 Log 的输出带有前缀 `ClassForTest`：

```text
[Prefix 4 YLog Context: ClassForTest] Debug for class
[Prefix 4 Unity Context: ClassForTest] Debug for class
```

`m_Class2ForTest.Debug("Debug for class2");` 仅会输出如下语句，因为它的前缀不包含有 `ClassForTest`，所以 UnityLogger 不会输出该 Log。

```text
[Prefix 4 YLog Context: Class2ForTest] Debug for class2
```
