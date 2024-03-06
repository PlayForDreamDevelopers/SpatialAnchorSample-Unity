# Focus Awareness

Focus awareness (FA) allows the system UI to be displayed as an overlay on top of a scene. Therefore, players can access the system UI without exiting the current application to have an uninterrupted immersive XR experience.


## Note

- Only events are provided. No API.
- If FA is disabled for your app, 2 sets of controller models may overlap on the screen when users press the Home button.


## Events

The focus events are described as follows:

| **Event** | **Description** | 
| --------- | --------------- |
| YVRManager.instance.eventsManager.onFocusGained | This event indicates that the application has lost input focus. For example, if a player presses the Home button while an application is running, the system UI will show up, causing the application to lose focus. At this time, the developer can pause the application, and disable this player's input capability (e.g., the controller) or notify other online players that this player is not focusing on the current application. | 
| YVRManager.instance.eventsManager.onFocusLost | This event indicates that the application has acquired input focus. When the system UI is closed by a player, this event will be triggered. At this time, the developer can continue the application and re-enable this player's input capability. | 