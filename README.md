# QuickVR - v2025.4.1

#### by Jeremy Gibson Bond

## Known Bugs and Issues (2024-10-09)
1. It seems like the Menu button isn't reading properly, and Meta restricts me from reading the Meta button.

## Adding QuickVR to your Unity Project
1. Before importing QuickVR, you may want to first set up your Unity project according to the recommendations in the following two sections (**Proper Unity Setup** & **Git Setup**).
2. In Unity, open the **Package Manager** ( Window > Package Manager ). 
3. Click the **+** (plus) In the top-left corner of the Package Manager window and choose **Add package from git URL...** .
4. Paste the URL of this GitRepo: **https://github.com/gameprof/QuickVR.git** .
5. Click the **Add** button on the far right.
6. Unity will install this package and the other two required packages automatically.
7. The Meta XR Core SDK package will require a restart of Unity, and it will pop up an annoying alert asking you to let Meta track how you use it. This pop-up will appear repeatedly until you follow the steps below to remove it.
8. After removing the pop-up, follow the **Testing QuickVR** steps below to make sure it's working for you.

#### Proper Unity Setup
The Meta XR Core SDK examples were built with the Built-in Render Pipeline, so I recommend starting from a **3D (Built-in Render Pipeline) Core** project template. 

#### Git Setup
I strongly recommend using Git for version control on your project. However, in order to do so, you will need the proper [.gitignore](https://github.com/MSU-mi231/Unity-3D-Template-2022.3/blob/main/.gitignore) and [.gitattributes](https://github.com/MSU-mi231/Unity-3D-Template-2022.3/blob/main/.gitattributes) files in the root level of your project. You are welcome to start a Git Repo as a copy of the [template that my students use for Unity 2022.3](https://github.com/MSU-mi231/Unity-3D-Template-2022.3).

## Required Packages
QuickVR relies on the following two packages, which will be imported automatically:
1. **[Meta XR Core SDK](https://assetstore.unity.com/packages/tools/integration/meta-xr-core-sdk-269169)** (*com.meta.xr.sdk.core*) - This is the core Meta library that allows access to OculusVR and other Quest-related code.
2. **[Oculus XR Plugin](https://docs.unity3d.com/Packages/com.unity.xr.oculus@4.2/manual/index.html)** (com.unity.xr.oculus) - This is used by the Meta library and Unity. It should be a required package for the Meta XR Core SDK but is somehow not required there.


## How to remove the CONSTANT Meta XR PopUp message about telemetry tracking
Every time I have imported the MetaXR library, it has ***constantly*** asked me to enable telemetry and then not accepted the choice regardless of whether I accepted or not. I believe that the issue is the MetaXR code being completely locked and immutable when it is imported into Unity as a UPM package (via the Package Manager). By making it a "Custom" package, we can edit the code and remove the annoying telemetry messages. However, if you update Meta XR Core to a new version, doing so _will_ overwrite these changes, so you will need to make them again.

**To get rid of this pop-up permanently** (or at least until you update Meta XR again), follow these steps:

1. **Move the MetaXR Core Library out of the _Library / PackageCache_ folder and into the _Packages_ folder to make it a Custom Package:**
   1. In the Unity Hierarchy pane, right click on the Assets folder and choose _Show in Explorer_ (macOS: _Show in Finder_).
   2. Quit Unity.
   3. Open the folder _Library / PackageCache_ .
   4. Find the _com.meta.xr.sdk.core@68.0.2_ folder (the digits after `@` will be different if you have a different version installed).
   5. Right click on this folder and choose Cut.
   6. In the Windows File Explorer or macOS Finder, go back up to the root level of your project folder (where you can see the Assets folder) and open the _Packages_ folder. You should see two files inside (manifest.json and packages-lock.json).
   7. Right click and choose Paste. This should move the com.meta.xr... folder into the Packages folder.
   8. Open your project in Unity, and it should load without errors. It may need to restart again since you moved the library around.
   9. To double check that the move worked, open the Package Manager (from _Window > Package Manager_), and you should see the word Custom next to the Meta XR Core SDK package.
2. **Modify the code of OVRTelemetryPopup.cs to make it _never_ pop up**
   1. In the Hierarchy, find the file: `Packages / Meta XR Core SDK / Editor / OVRTelemetry / OVRTelemetryPopup`.
   2. Open `OVRTelemetryPopup` in your code editor.
   3. Find the `ShouldShowPopup()` method and replace it with the following, which will tell the PopUp to never appear.
```csharp
   private static bool ShouldShowPopup()
   {
      // if (Application.isBatchMode)
      // {
      //     return false;
      // }
      //
      // return !UserHasPreviouslyAnswered;
      return false;
   }
```


## Testing QuickVR
Here are some basic instructions to get you going.
1. First, **follow the instructions above to disable the telemetry pop-up**.
2. QuickVR was built using the **ControllerModels** example scene from the Meta XR Core SDK samples. To import this sample scene:
   1. Open the Package Manager to the list of **In Project** Packages.
   2. Select the **Meta XR Core SDK**.
   3. Click the **Samples** tab in the description of the package on the right.
   4. Beside **Sample Scenes**, click **Import** to bring them into your project.
3. Check to make sure that Meta XR doesn't have any issues it needs to fix.
   1. From the menu bar, choose **Edit > Project Settings...** .
   2. Select **Meta XR** from the list on the left.
   3. Under the **Checklist**, fix anything that it needs to auto-fix.
   4. I recommend that you let it make the recommended fixes as well.
4. Drag the **--QuVR--** prefab from the `Packages / QuickVR / Assets` folder into the ControllerModels Sample Scene and set it up:
   1. Open the **ControllerModels** scene in `Samples / Meta XR Core SDK / [60.0.2] / Sample Scenes /`.
   2. The Quest works best with the Universal Render Pipeline, but the textures in the Meta XR Core XDK Samples aren't yet initially set up for URP. If your textures all show up as pink in URP, you will need to:
      1. Follow the directions below to make the Meta XR Core SDK a custom package so that you can upgrade the textures.
      2. From the menu bar, choose Window > Rendering > Render Pipeline Converter.
      3. Check all four options and click Initialize and Convert.  
   3. Open the `Packages / QuickVR / Assets` folder.
   4. Drag the `--QuVR--` prefab from that folder into the Hierarchy.
   5. Select `--QuVR--` in the Hierarchy and assign the three Transforms near the top of the inspector:
      1. Head Anchor should be `OVRPlayerController / OVECameraRig / TrackingSpace / CenterEyeAnchor`
      2. Left Hand Anchor should be `OVRPlayerController / OVECameraRig / TrackingSpace / LeftHandAnchor / LeftControllerAnchor`
      3. Right Hand Anchor should be `OVRPlayerController / OVECameraRig / TrackingSpace / RightHandAnchor / RightControllerAnchor`
   6. Press Play in Unity (assuming you already have your Meta Quest Link set up and running).
   7. Select --QuVR-- in the Hierarchy. You should be able to see the controls moving in the Inspector. 

## Recommended Packages
1. **[Naughty Attributes by Denis Rizov](https://assetstore.unity.com/packages/tools/utilities/naughtyattributes-129996)** (*com.dbrizov.naughtyattributes*) - An excellent little project that makes it easy to set up Editor UI using compiler attributes (e.g., `[Required]` above a field declaration). It is used extensively in the QuVR class to make the Editor Inspector for QuVR easier to read. If you import this, please uncomment the `#define USE_NAUGHTY_ATTRIBUTES` at the top of QuVR.cs.

## Other Packages to Consider
2. **[Meta XR Interaction SDK](https://assetstore.unity.com/packages/tools/integration/meta-xr-interaction-sdk-265014)** (*com.meta.xr.sdk.interaction.ovr*) - QuickVR does not rely on this package, but it can give you some good examples of interactions you can build on. You can check them out by installing the optional Sample files that come with this Package.

