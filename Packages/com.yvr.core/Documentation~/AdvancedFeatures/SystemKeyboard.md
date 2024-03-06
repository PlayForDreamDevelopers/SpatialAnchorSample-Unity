# System Keyboard

Users can effortlessly input text in a wide range of scenarios using in-app keyboard, including text chats and text-based information settings. DreamOS provides a system keyboard that allows developers to incorporate it easily into the app. 


## Requirements

Please complete the following before you start:

- Use XR Interaction Toolkit version 2.1.0 or later. 

- Import Starter Assets.

- Add XR Origin to your scene and setup controllers. 


## Enable System Keyboard

1. In the Hierarchy window, complete the following steps: 
    
    - Select **+** > **UI** > **Event System** to add event system to the scene. 
    - Select **+** > **UI** > **Canvas** to add canvas to the scene. 


2. Select **Canvas** and in the **Inspector** window, complete the following steps: 

    - Set **Render Mode** to **World Space**
    - Set **Event Camera** to **Main Camera**
    - Add **Tracked Device Graphics Raycast** script to **Canvas**


3. In the **Hierachy** window, right-click **Canvas** and select **UI** > **Input Field - TextMeshPro** from the shortcut menu to add input field to the scene. 

    ![SystemKeyboard](../AdvancedFeatures/SystemKeyboard/SystemKeyboard.png)


4. Enable the interaction between the ray and input field via the following steps: 

    - Adjust the distance between the input field and the main camera
    - Adjust the length of the ray

    > [!Note]
    > Scene is dimmed when the keyboard is shown. 