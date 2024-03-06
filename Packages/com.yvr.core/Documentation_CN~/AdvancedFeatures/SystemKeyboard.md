# 系统键盘

用户可以使用应用内键盘在各种场景中轻松输入文字，包括文字聊天和基于文字的信息设置。DreamOS 提供的系统键盘可让开发人员轻松将其纳入应用程序。


## 要求

请在开始之前完成以下步骤：

- 使用 XR 交互工具包 2.1.0 或更高版本。

- 导入 Starter Assets 素材。

- 在场景中添加 XR Origin 并设置手柄。


## 启用系统键盘

1. 在 **Hierarchy** 面板下，完成以下步骤：
    
    - 选择 **+** > **UI** > **Event System** 将事件系统添加至场景中
    - 选择 **+** > **UI** > **Canvas** 将画布添加至场景中


2. 选中 **Canvas** 并在 **Inspector** 面板下，完成以下步骤：

    - 将 **Render Mode** 设置为 **World Space**
    - 将 **Event Camera** 将为 **Main Camera**
    - 将 **Tracked Device Graphics Raycast** 脚本添加至 **Canvas**


3. 在 **Hierachy** 面板下，右击 **Canvas** 并选择 **UI** > **Input Field - TextMeshPro** 从快捷菜单中将输入字段加到场景中。

    ![SystemKeyboard](../AdvancedFeatures/SystemKeyboard/SystemKeyboard.png)


4. 通过以下步骤启动射线与输入字段之间的互动： 

    - 调整输入字段与主摄像头之间的距离
    - 调整射线的长度

    > [!Note]
    > 显示键盘时场景变暗。