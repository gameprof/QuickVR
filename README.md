# QuickVR
by Jeremy Gibson Bond

### Required Package
QuickVR relies on the following two packages:
1. **[Meta XR Core SDK](https://assetstore.unity.com/packages/tools/integration/meta-xr-core-sdk-269169)** (*com.meta.xr.sdk.core*) - This is the core Meta library that allows access to OculusVR and other Quest-related code.

### Testing QuickVR
Here are some basic instructions to get you going.
1. QuickVR was built using the **ControllerModels** example scene from the Meta XR Core SDK samples. To import this sample scene:
   1. Open the Package Manager to the list of **In Project** Packages.
   2. Select the **Meta XR Core SDK**.
   3. Click the **Samples** tab in the description of the package on the right.
   4. Beside **Sample Scenes**, click **Import** to bring them into your project.
2. Add the --QuVR-- prefab to the ControllerModels Sample Scene and set it up:
   1. Open the **ControllerModels** scene in `Samples / Meta XR Core SDK / [60.0.2] / Sample Scenes /`.
   2. Open the `Packages / QuickVR / Assets` folder.
   3. Drag the `--QuVR--` prefab from that folder into the Hierarchy.
   4. Select `--QuVR--` in the Hierarchy and assign the three Transforms near the top of the inspector:
      1. Head Anchor should be `OVRPlayerController / OVECameraRig / TrackingSpace / CenterEyeAnchor`
      2. Left Hand Anchor should be `OVRPlayerController / OVECameraRig / TrackingSpace / LeftHandAnchor / LeftControllerAnchor`
      3. Right Hand Anchor should be `OVRPlayerController / OVECameraRig / TrackingSpace / RightHandAnchor / RightControllerAnchor`
   5. Press Play in Unity (assuming you already have your Meta Quest Link set up and running).
   6. Select --QuVR-- in the Hierarchy. You should be able to see the controls moving in the Inspector. 

### Recommended Packages
1. **[Naughty Attributes by Denis Rizov](https://assetstore.unity.com/packages/tools/utilities/naughtyattributes-129996)** (*com.dbrizov.naughtyattributes*) - An excellent little project that makes it easy to set up Editor UI using compiler attributes (e.g., `[Required]` above a field declaration). It is used extensively in the QuVR class to make the Editor Inspector for QuVR easier to read. If you import this, please uncomment the `#define USE_NAUGHTY_ATTRIBUTES` at the top of QuVR.cs.

### Other Packages to Consider
2. **[Meta XR Interaction SDK](https://assetstore.unity.com/packages/tools/integration/meta-xr-interaction-sdk-265014)** (*com.meta.xr.sdk.interaction.ovr*) - QuickVR does not rely on this package, but it can give you some good examples of interactions you can build on. You can check them out by installing the optional Sample files that come with this Package.

### How to fix / remove the CONSTANT MetaXR PopUp message about telemetry tracking
When I imported the MetaXR library, it began to ***constantly*** ask me to enable telemetry and then did not accept the choice when I finally did accept it. I believe that the issue is the MetaXR code being completely locked and immutable when it is imported into Unity as a UPM package (via the Package Manager). By making it a "Custom"package, we can edit the code and remove the message about it. If you update Meta XR Core to a new version, it _will_ overwrite these changes, so you will need to make them again.

To get rid of this pop-up permanently (or at least until you update MetaCR again), follow these steps:

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

