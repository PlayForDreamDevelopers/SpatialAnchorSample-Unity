# Logger Controller

`Logger Controller` 用来对 [Logger](./Logger.md) 进行相应控制。当一个 [Logger](./Logger.md) 被设置了 Controller 后，该 Logger 的每个输出函数中都会调用 [IsLogValid](<xref:YVR.Utilities.LoggerControllerBase.IsLogValid(System.Object,System.String,YVR.Utilities.LogPriority,System.String)>) 函数判断该 Log 函数是否该被输出。

## LoggerPriorityController

[LoggerPriorityController](xref:YVR.Utilities.LoggerPriorityController) 根据其中的参数 [priority](xref:YVR.Utilities.LoggerPriorityController.priority) 进行 Log 等级的筛选，如果需要输出的 Log 等级小于[priority](xref:YVR.Utilities.LoggerPriorityController.priority) 则不输出该 Log。

## LoggerPrefixPriorityController

[LoggerPrefixPriorityController](xref:YVR.Utilities.LoggerPrefixPriorityController) 继承 [LoggerPriorityController](xref:YVR.Utilities.LoggerPriorityController)，在其基础上增加了对于特定前缀重写过滤等级的功能。

可通过函数 [AddPrefix2PriorityMap](<xref:YVR.Utilities.LoggerPrefixPriorityController.AddPrefix2PriorityMap(System.String,YVR.Utilities.LogPriority)>)，[RemovePrefixPriorityMap](<xref:YVR.Utilities.LoggerPrefixPriorityController.RemovePrefixPriorityMap(System.String)>) 添加或删除需要重写的前缀，以及重写的过滤等级。

### Example

如下代码，通过设定 [priority](xref:YVR.Utilities.LoggerPriorityController.priority) 将过滤等级设为最高（没有 Log 可以输出），并通过函数 [AddPrefix2PriorityMap](<xref:YVR.Utilities.LoggerPrefixPriorityController.AddPrefix2PriorityMap(System.String,YVR.Utilities.LogPriority)>) 将 `ClassForTest` 前缀的等级设为最低，因此可以输出该类中输出的 Log。

```csharp
m_ControllerForTest.priority = LogPriority.Lowest;
m_YLogLogger.SetController(m_ControllerForTest);
m_YLogLogger.SetPrefix(m_ContextPrefix);

// ...

m_ControllerForTest.priority = LogPriority.Highest;
m_ControllerForTest.AddPrefix2PriorityMap(nameof(ClassForTestPrefix), LogPriority.Lowest);
m_ClassForTestPrefix.Debug("Call Debug");
```

此时的输出结果为：

```text
[Context: ClassForTestPrefix] Call Debug
```
