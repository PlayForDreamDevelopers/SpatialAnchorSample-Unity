# Changelog

## [0.14.14] - 2024-01-22

### Added

- CopyTexture可以选择SrcRect和DstRect

## [0.14.13] - 2023-12-19

### Changed

- 给CopyAndroidTextureWithAlpha添加isLinearColorSpace

## [0.14.12] - 2023-12-15

### Added

- 添加CopyAndroidTextureWithAlpha接口

## [0.14.11] - 2023-11-30

### Added

- 添加CopyTextureWithAlpha接口

## [0.14.10] - 2023-11-09

### Added

- 为 Singleton 增加 Reset 接口

## [0.14.9] - 2023-10-10

### Fixed

- 修复单元测试脚本中编译报错

## [0.14.8] - 2023-10-07

### Changed

-  重新整理 Extensions 文件夹，将其分为 System 和 Unity 两个子目录

## [0.14.7] - 2023-09-15

### Added

- 原 YVRLogControllerTest 替换为 YVRLogProjectSettingSOTests

## [0.14.6] - 2023-09-14

### Added

- 将 YVRLog 的配置方式调整为基于 Project Settings 机制

## [0.14.5] - 2023-09-14

### Fixed

- 修复文件夹不存在情况下，YVRProjectSettings 创建失败的问题

## [0.14.4] - 2023-09-14

### Changed

- 修改 YVRProjectSettings 保存的文件夹至 Assets/YVRProjectSettings


## [0.14.3] - 2023-09-14

### Added

- 增加 Project Settings 模块，支持 Packages 设定自己的 Project 范围的 Settings

## [0.14.2] - 2023-09-13

### Fixed

- 修复 YVRLogConfig 配置失效的问题

## [0.14.1] - 2023-09-05

### Fixed

- HideInCallstack 在Unity2021等低版本中编译报错

## [0.14.0] - 2023-09-04

### Add

- 添加assets文件夹拷贝相关

## [0.13.9] - 2023-08-23

### Fixed

- 修复CopyTexture的时候没有Disable Blend导致的渲染错误

## [0.13.8] - 2023-08-23

### Added

- 将 newtonsoft 添加至 Utilities 依赖中
- 增加 YVRLogController 用于管理应用的 Log 的配置。
- 增加关于 YVRLogController 的文档

## [0.13.7] - 2023-08-02

### Fixed

- YLog 输出的日志显示了日志模块的相关堆栈，双击后打开的不是 YLog 调用的位置。
- Log 相关的单元测试不通过。

## [0.13.6] - 2023-07-12

### Added

- 添加了 CommandBuffer 的 Trigger 事件

## [0.13.5] - 2023-07-12

### Fixed

- 修复 Singleton 线程不安全的问题

## [0.13.4] - 2023-06-14

### Fixed

- 修复 ramlog 输出时的冗余 log

## [0.13.3] - 2023-06-12

### Changed

- 调整对日志前缀的处理，将类名前缀改为全类名，增加对字符串的处理。

## [0.13.2] - 2023-06-05

### Changed

- 在 Singleton 初始化时设置 s_Instance，避免重复获取或者获取失败的问题

## [0.13.1] - 2023-05-19

### Added

- YVRLog 增加在静态函数中可使用的无 Context 的 Debug/Info 等函数

## [0.13.0] - 2023-05-18

### Added

- 增加 YVRLogMgr 管理 YVRLog 相关的配置

## [0.12.0] - 2023-04-27

### Removed

- 移除 NoticableList，可用 ObservableCollection 替代

### Added

- 为 string 类型增加 EqualsIgnoreCase 拓展

## [0.11.0] - 2023-04-27

### Removed

- 将 OverdrawMonitor 模块挪至 YVR.Tools 中管理

## [0.10.0] - 2023-04-26

### Changed

- 重写 Log 模块

## [0.9.3] - 2023-03-31

### Changed

- 减少检测 Ray 是否和 RectTransform 是否相交算法的 GC Alloc

## [0.9.2] - 2023-03-30

### Added

- 增加关于 RT 重建和释放的安全检查

## [0.9.1] - 2023-03-24

### Fixed

- 修复 RenderEventMgr 反复注销回调的问题

## [0.9.0] - 2023-03-24

### Added

- 增加 RenderEventMgr 控制渲染相关事件的顺序
- 为 RenderTextureWrapper 增加宽/高参数

### Fixed

- 修复 Gfx Helper 相关函数在 Editor 中执行顺序问题

## [0.8.0] - 2023-03-17

### Added

- 为 double 增加拓展类型，可以将纳秒转换为微秒
- 为 string 增加拓展类型，可以将 string 写入到特定文件中

## [0.7.2] - 2023-02-22

### Added

- 新增操作 AndroidManifest.xml 文件

### Changed

## [0.7.1] - 2023-02-21

### Changed

- 删除函数调用间无用的 in parameter
- 命名规范化以符合标准

### Added

- 增加部分拓展函数单元测试

## [0.7.0] - 2023-01-09

### Added

- 增加 BinaryReader/Writer 拓展

### Changed

- 将 RenderTexture 转换为 Texture2D 的函数 public 化

## [0.6.4] - 2022-11-03

### Added

- 添加 CopyTextureRect 方法，为 MRC 在一张纹理上画前景、前景透明通道、背景做支持

## [0.5.4] - 2022-11-03

### Fixed

- 修复 Array.ForEach 对尺寸为 0 的数组处理异常

## [0.5.3] - 2022-09-05

### Fixed

- 修复应用暂停和恢复时对 RT 的重绘操作导致透明部分有杂色的问题

## [0.5.2] - 2022-08-19

### Added

- 为 RenderTextureWrapper 增加使用 RenderTexture 的构造函数重载。当外部调用者需要使用较为复杂的 RT 时可通过该重载调用。
- 在 Singleton 相关的类中增加 createdInstance，该 instance 变量在访问时并不会创建 instance

### Changed

- 修改 MonoBehaviorSingleton 中部分变量的命名

## [0.5.1] - 2022-06-21

### Fixed

- 修复 RamLog 偶现 Crash 问题

## [0.5.0-preview.0] - 2022-06-10

### Added

- 为 Generic Enumerable 类型的 Object 增加拓展
- 为 String 增加关于 MD5 的拓展函数

### Fixed

- 修复当任意目标是 Null 时，ReflectionEqual 失败的问题

## [0.4.1] - 2022-06-17

### Fixed

- 修复 unity 2019 对 c# 新语法不支持编译失败的问题

## [0.4.0] - 2022-05-12

### Added

- 增加 RenderTextureWrapper 用以封装 RT 被重新创建时的操作

## [0.3.0] - 2022-05-12

### Changed

- 正式发布 0.3.0

## [0.3.0-preview.0] - 2022-05-12

### Added

- 为 Array 增加拓展，如 ForEach 和 IndexForEach
- 增加 YLog

## [0.2.1-preview.0] - 2022-05-10

### Fixed

- 修复 Singleton 单例在 2019 中编译失败的问题

## [0.2.0] - 2022-04-16

### Changed

- 0.2.0 正式发布

## [0.2.0-preview.1] - 2022-04-16

### Added

- Copy/Write Texture 支持 Texture2DArray 格式

## [0.2.0-preview.0] - 2022-04-13

### Changed

- 使用 Native RenderEvent 替代 RenderEventAndData

## [0.1.20-preview.0] - 2022-4-08

### Added

- 增加 GfxHelper 封装渲染线程操作
- 增加 Color / Color 32 相关拓展函数
- 增加关于 GfxHelper 和部分拓展函数 的单元测试

### Removed

- 删除 ExcludeFromDocs Attribute，可直接使用 UnityEngine.Internal 中的 Attribute 替代

## [0.1.19] - 2022-3-30

### Added

- 添加扩展函数

## [0.1.18] - 2022-2-21

### Fixed

- PringPackagesVersion 的拼写错误。

## [0.1.17] - 2022-2-21

### Added

- 将 PackageVersion 拆分为多个脚本，增加获取和打印指定 package 版本信息的函数。

### Fixed

- 编辑器下运行崩溃

## [0.1.16] - 2022-2-19

### Added

- 版本号存储打印支持列表
- 为 RenderTexture 增加 GetPixel 拓展函数

## [0.1.15] - 2022-2-19

### Added

- 添加运行时打印版本号功能

## [0.1.14] - 2022-2-14

### Added

- 添加 Byte[]、Enum、Collection 扩展方法

## [0.1.13] - 2022-1-05

### Removed

- 删除 localization 相关文件。

# Changelog

## [0.1.12] - 2022-1-05

### Added

- 修改单例类的创建实例方式为通过 new() ,因为通过反射创建打包在安卓不适用。

# Changelog

## [0.1.11] - 2022-1-04

### Added

- 修改单例类的创建实例方式为通过反射创建

# Changelog

## [0.1.10] - 2021-12-16

### fixed

- 打包应用的时候，ConditionalDisplayModifierAttribute.conditionMet 和 ColorModifierAttribute.backupGUIColor 报警告，变量定义了但未使用

## [0.1.9] - 2021-12-01

### Added

- 为 RectTransform 增加 RayIntersect 拓展函数

## [0.1.8] - 2021-11-29

### Added

- 增加 L10nTextMgr.GetFont 可根据 localization.json id:Font_Home 加载字体

## [0.1.7] - 2021-11-04

### Added

- 增加 Localization Demo 场景
- 为 Localization 模块增加注释
- 为本地化模块增加 Manual 文档

### Changed

- 将原 Document 文件夹重命名为 Documentation
- 根据 Android Locale Config 修改 LangPack 中成员变量的命名

## [0.1.6] - 2021-11-03

### Added

- 增加 Text Localization 模块

## [0.1.5] - 2021-06-15

### Added

- 增加 Overdraw Monitor，可以在 URP 中查看 Overdraw 的情况

### Fixed

- 修复因空文件夹产生的 .meta 文件导致的 Warning 消息

## [0.1.4] - 2021-04-22

### Added

- 增加 Singleton 单例基类
- 增加 ExcludeFromDocs Attribute，用来让特定对象不会产生文档
- 初始化文档文件夹

## [0.1.3] - 2021-03-30

### Fixed

- 修复因为缺少 ChangeLog 对应的 meta 文件导致无法编译的问题

## [0.1.2] - 2021-03-29

### Added

- 增加 Change log
- 增加 `NotifiableList` 容器，该容器可以在元素数量发生变化时，以及元素内容发生改变时，分别通过 `onCollectionChanged` 和 `onElementsPropertyChanged` 接口通知观察者
