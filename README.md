# QuickVR
by Jeremy Gibson Bond

### Required Packages
QuickVR relies on the following two packages:
1. **Meta XR Core Library** (*com.meta.xr.sdk.core*) - This is the core Meta library that allows access to OculusVR and other Quest-related code. 
2. **Naughty Attributes** (*com.dbrizov.naughtyattributes*) - An excellent little project that makes it easy to set up Editor UI using compiler attributes (e.g., `[Required]` above a field declaration). It is used extensively in the QuVR class to make the Editor Inspector for QuVR easier to read.

### How to fix / remove the MetaXR PopUp message about telemetry tracking
When I imported the MetaXR library, it began to ***constantly*** ask me to enable telemetry and then did not accept the choice when I finally did accept it. I believe that the issue is the MetaXR code being completely locked and immutable when it is imported into Unity as a UPM package (via the Package Manager). By making it a "Custom"package, we can edit the code and remove the message about it. If you update Meta XR Core to a new version, it _will_ overwrite these changes, so you will need to make them again.

To get rid of this pop-up permanently (or at least until you update MetaCR again), follow these steps:

1. **Move the MetaXR Core Library out of the _Library / PackageCache_ folder and into the _Packages_ folder to make it a Custom Package:**
   1. In the Unity Hierarchy pane, right click on the Assets folder and choose _Show in Explorer_ (macOS: _Show in Finder_).
   2. Quit Unity.
   3. Open the folder _Library / PackageCache_ .
   3. Find the _com.meta.xr.sdk.core@68.0.2_ folder (the digits after `@` will be different if you have a different version installed).
   4. Right click on this folder and choose Cut.
   5. In the Windows File Explorer or macOS Finder, go back up to the root level of your project folder (where you can see the Assets folder) and open the _Packages_ folder. You should see two files inside (manifest.json and packages-lock.json).
   6. Right click and choose Paste. This should move the com.meta.xr... folder into the Packages folder.
   7. Open Unity, and the project should load fine.
   8. To double check that the move worked, open the Package Manager (from _Window > Package Manager_), and you should see the word Custom next to the Meta XR Core SDK package.
2. **Modify the code of OVRTelemetryPopup.cs to make it _never_ pop up**
   1. In the Hierarchy, find the file: `Packages / Meta XR Core SDK / Editor / OVRTelemetry / OVRTelemetryPopup`.
   2. Open it in your code editor.
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

