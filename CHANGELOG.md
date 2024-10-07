# QuickVR - Change Log

## Version 1.0.0 - 2024-10-07
Initial version. Includes files:
- **QuVR.cs** - Core QuVR script
- **QuVR_ButtonState.cs** - Used by QuVR to track the states of the A, B, X, Y, and Menu buttons. Can be read as a bool or read with something like QuVR.X.down to see if X was pressed down this frame.
- **--QuVR-- Prefab** - A prefab to drop into the root level of your Scene Hierarchy that enables QuVR to track controllers and the headset. 