# LogPrefix

LogPrefix 用来为 Log 增加额外的前缀信息。所有的 LogPrefix 都派生自 `LogPrefixBase` 类，且可以包含其他的 `LogPrefixBase`。将被包含的 `LogPrefixBase` 称为 `wrappedLogPrefix`。

所有的 LogPrefix 都需要重写 [GetPrefix](<xref:YVR.Utilities.PriorityLogPrefix.GetPrefix(System.Object,System.String,YVR.Utilities.LogPriority)>) 函数，该函数中指定需要输出的 Prefix。如 [PriorityLogPrefix](xref:YVR.Utilities.PriorityLogPrefix) 的 `GetPrefix` 实现如下：

```csharp
protected override string GetPrefix(object context, string log, LogPriority priority)
{
    return $"Priority: {priority.ToString()}";
}
```

当 [LoggerBase](xref:YVR.Utilities.LoggerBase) 中通过调用 [LogPrefixBase.GetCombinedPrefix](<xref:YVR.Utilities.LogPrefixBase.GetCombinedPrefix(System.Object,System.String,YVR.Utilities.LogPriority)>) 获取具体的 Log 前缀。

该函数会触发 `wrappedLogPrefix` 以及自身的 [GetPrefix](<xref:YVR.Utilities.PriorityLogPrefix.GetPrefix(System.Object,System.String,YVR.Utilities.LogPriority)>) 函数，并将结果结合在一起。

目前实现了三种 Prefix：

## CustomLogPrefix

构造 [CustomLogPrefix](xref:YVR.Utilities.CustomLogPrefix) 时需要传入一个字符串表示自定义的前缀信息。该前缀信息会直接被输出。

## ContextLogPrefix

[ContextLogPrefix](xref:YVR.Utilities.ContextLogPrefix) 中的 [GetPrefix](<xref:YVR.Utilities.PriorityLogPrefix.GetPrefix(System.Object,System.String,YVR.Utilities.LogPriority)>) 函数会返回调用 Log 函数的对象的类信息。对于 `GameObject` 会输出 `GameObject.name`，对于 `MonoBehaviour` 会返回 `GameObject.name - MonoBehaviour.name`，对于其他类型会返回该类型的类型名。

[ContextLogPrefix](xref:YVR.Utilities.ContextLogPrefix) 会将上述的类信息，以 `Context: <contextInfo>` 的格式被输出。

## PriorityLogPrefix

[PriorityLogPrefix](xref:YVR.Utilities.ContextLogPrefix) 中的 [GetPrefix](<xref:YVR.Utilities.PriorityLogPrefix.GetPrefix(System.Object,System.String,YVR.Utilities.LogPriority)>) 函数会返回调用 Log 函数的优先级，并会以 `Priority: <PriorityInfo>` 的格式被输出。

# Example

如下代码展示了多个 Prefix 合并输出的效果：

```csharp
m_YLogLogger = new YLogLogger();
m_ClassForTestPrefix = new ClassForTestPrefix();
m_GOForTest = new GameObject("GameObjectForTest", typeof(MonoBehaviourForTest));

m_YLogLogger.SetPrefix(new ContextLogPrefix(new PriorityLogPrefix(new CustomLogPrefix("YVR"))));
m_GOForTest.Debug("Call Debug");
```

此时输出的 Log 为：

```text
[YVR Priority: Debug Context: GameObjectForTest] Call Debug
```
