# Changelog

## [0.5.3] - 2024-4-2

### Changed

- 适配 Utilities 的命名空间

## [0.5.2] - 2023-11-10

### Fixed

- Platform 中修复 调用获取全部内购条目接口时传入 null 返回数据为空的问题

## [0.5.1] - 2023-10-27

### Added

- Platform 中新增 PlatformSample

## [0.5.0] - 2023-09-21

### Added

- Platform 中新增运动数据相关接口

## [0.4.3] - 2023-07-14

### Added

- Platform 排行榜增加积分兑换的头像和边框字段

## [0.4.2] - 2023-07-04

### Fixed

- 重命名 PlatformSDK 中 内购接口名，与 Pico 和 Quest 保持一致

## [0.4.1] - 2023-06-30

### Added

- 添加 YVREntitledManager 方便开发者接入鉴权功能

## [0.4.0] - 2023-06-19

### Added

- Platform 使用 Message 机制重写内购

## [0.3.1] - 2023-04-14

### Added

- Platform 添加 Newtonsoft Json 的依赖

## [0.3.0] - 2023-03-02

### Added

- Platform 相关代码格式化
- YVRError 对象变为公共成员
- 由于后端接口区分大小写，排行榜相关枚举添加 Description，用于接口传参
- 排行榜信息新增用户性别数据

## Fixed

- 排行榜获取用户头像接口返回错误问题修改

## [0.2.2] - 2022-11-04

### Added

- PlatformSDK 中新增内购接口

## [0.2.1] - 2022-10-26

### Added

- PlatformSDK 中新增排行榜接口

## [0.2.0] - 2022-06-22

### Added

- PlatformSDK 中属性和接口名修改
- PlatformSDK 功能解耦

## [0.1.8] - 2022-03-01

### Fixed

- 修改获取用户信息失败的问题

### Added

- 新增错误码定义

## [0.1.7] - 2022-02-21

- 保存 platform 的版本，并在 YVRPlatform 初始化时打印
- 将 Utilities 的依赖版本改为 0.1.18

## [0.1.6] - 2021-12-23

### Fixed

- 鉴权接口解决开发者未登录账户启动应用鉴权出现的空指针问题

## [0.1.5] - 2021-12-22

### Fixed

- 修改鉴权接口 token 过期引起的无回调问题

## [0.1.4] - 2021-12-21

### Fixed

- 鉴权接口更新，解决发送鉴权结果广播给 home 报错问题

## [0.1.3] - 2021-12-19

### Added

- aar 中针对鉴权接口进行处理，未连接网络的情况下不会返回异常的 ErrorCode

## [0.1.2] - 2021-10-14

### Added

- 在有关数据类中重写 ToString 方法以便调试获取信息

## [0.1.1] - 2021-10-14

### Added

- 增加好友相关接口
