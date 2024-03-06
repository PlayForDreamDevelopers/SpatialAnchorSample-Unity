# OS Splash Screen 

After the user clicks on an app, YVR system will take some time to initialize the rendering system and the XR system before launching the app. YVR System Splash Screen is introduced to reduce the initialization time and supports to add an image as the app's splash screen. (i.e., the screen before the appearance of the Unity log and the "MADE WITH Unity" text ). You can set up the splash screen in the Unity Editor.


## Use Case

- Reduce the duration of the initialization loading screen
<br />
    For a good user experience, the initialization loading screen of your app should last no more than 5 seconds. To determine the app's initialization duration, close all background apps and launch the target app. If the app takes longer than 5 seconds to initialize, consider adding a splash screen for a better user experience.

- Promote app ideas
<br />
    You can design relevant materials on the splash screen based on the app's theme to create a memorable impression on users.


- Increase brand awareness
<br />
    You can add developer information on the splash screen to increase brand awareness.


## Set Up Splash Screen 

1. Open an existing scene or create a new scene in the Unity Editor.

2. Go to **Edit** > **Project Settings** > **XR Plug-in Management** > **YVR**.

3. Select an image as the System Splash Screen.