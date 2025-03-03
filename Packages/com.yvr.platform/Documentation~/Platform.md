# Introduction

The YVR Platform SDK aims to help developers create a social game experience. For now, it provides the following features:

-   [Entitlement Check](./EntitlementCheck.md): Check used to check whether the current device user has purchased or owned your application.
-   [Account And Friend](./AccountAndFriends.md): Get logged user and users' friend information.
-   [Achievement](./Achievement.md): Handle achievements which set by developers on Developer center dashboard.
-   [Deep Link](./DeepLink.md): Deeplink allows users to join multiplayer session.
-   [Leaderboard](./Leaderboard.md): Ranking scores of users and friends, listing them in ascending or descending order.
-   [In-App Purchase](./IAPClient.md): User can purchase in-app items by topping up Y coins in user center.
-   [Sports Data Authorization](./SportsDataAuthorization.md): Obtain user sports data from "YVR GO", and sports apps can use the data to analyze the user's exercise.


## Get Started

Before using any platform features, developers have to initialize Platform SDK firstly:

```csharp
YVR.Platform.YVRPlatform.Initialize(<appId>);

if (YVRPlatform.IsInitialized)
    Debug.Log("Platform SDK is Successfully initialized.");
else
    Debug.LogError("Platform SDK initialize failed.");
```

> [!Note]
> In order to get an AppID, you need to first register as a developer on our [developer platform](https://developer.yvr.cn/yvrdvcenter/) and then create your application.


## Sample

> [!Important]
> YVR system version requirement: 1.2.6 and above

Please follow the steps to import Sample Project. [Platform Sample](https://github.com/YVRDeveloper/PlatformSample-Unity) is also available at [YVR Developer Github](https://github.com/YVRDeveloper).

1. Import Platform SDK.

2. Go to **Window** > **Package Manager** > **Platform** > **Samples**, select **Import** button to import Platform Samples.
    <br />
    ![ImportSamples](./Samples/ImportSamples.png)

3. Under Project panel, go to **Assets** > **Samples** > **YVR Platform** > *[**platform version number**]*. Select **Initialize** folder to view the sample project. 
    <br />
    ![ProjectFolder](./Samples/ProjectFolder.png)
    <br />
    > [!Note]
    > The platform version number depends on the version you have imported, e.g. 0.5.1.

4. Build and install on YVR device. 
    <br />
    ![InitializeSample](./Samples/InitializeSample.png)
    <br /><br />
    ![PlatformSample](./Samples/PlatformSample.png)

