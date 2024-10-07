using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace QuickVR {
    [DefaultExecutionOrder( -99 )] // This should happen before other scripts
    public class QuVR : MonoBehaviour {
        static public QuVR      S;
        static public Transform L_TRANS;
        static public Transform R_TRANS;
        static public Transform H_TRANS;

        [Required( "Assign the CenterEyeAnchor to this" )]
        public Transform headAnchor;

        [BoxGroup( "Left Hand" )] public Transform           leftHandAnchor;
        [BoxGroup( "Left Hand" )] public OVRInput.Controller leftController;
        [Header( "Dynamic Controller State" )]
        [BoxGroup( "Left Hand" )] public QuVR_ButtonState x;
        [BoxGroup( "Left Hand" )] public QuVR_ButtonState y;
        [BoxGroup( "Left Hand" )] public QuVR_ButtonState menu;
        [Range( 0, 1 )]
        [BoxGroup( "Left Hand" )] public float lTrigger, lGrip;
        [Range( -1, 1 )]
        [BoxGroup( "Left Hand" )] public float lThumbX, lThumbY;
        [BoxGroup( "Left Hand" )] public Vector2 lThumbStick;


        [BoxGroup( "Right Hand" )] public Transform           rightHandAnchor;
        [BoxGroup( "Right Hand" )] public OVRInput.Controller rightController;
        [Header( "Dynamic Controller State" )]
        [BoxGroup( "Right Hand" )] public QuVR_ButtonState a;
        [BoxGroup( "Right Hand" )] public QuVR_ButtonState b;
        // [BoxGroup( "Right Hand" )] public QuVR_ButtonState meta;
        [Range( 0, 1 )]
        [BoxGroup( "Right Hand" )] public float rTrigger, rGrip;
        [Range( -1, 1 )]
        [BoxGroup( "Right Hand" )] public float rThumbX, rThumbY;
        [BoxGroup( "Right Hand" )] public Vector2 rThumbStick;

        public eQuVR_ProgressOn progressButtonsOn    = eQuVR_ProgressOn.update;
        public bool             debugButtonDownAndUp = false;
        
        void Start() {
            S = this;
            L_TRANS = leftHandAnchor;
            R_TRANS = rightHandAnchor;
            H_TRANS = headAnchor;

            RegisterQuVR_StateProgression();
        }

        private void OnDestroy() {
            DeRegisterQuVR_StateProgression();
        }

        void RegisterQuVR_StateProgression() {
            QuVR_State.PROGRESS_STATES += x.Progress;
            QuVR_State.PROGRESS_STATES += y.Progress;
            QuVR_State.PROGRESS_STATES += menu.Progress;
            QuVR_State.PROGRESS_STATES += a.Progress;
            QuVR_State.PROGRESS_STATES += b.Progress;
            // QuVR_State.PROGRESS_STATES += meta.Progress;
        }

        void DeRegisterQuVR_StateProgression() {
            QuVR_State.PROGRESS_STATES -= x.Progress;
            QuVR_State.PROGRESS_STATES -= y.Progress;
            QuVR_State.PROGRESS_STATES -= menu.Progress;
            QuVR_State.PROGRESS_STATES -= a.Progress;
            QuVR_State.PROGRESS_STATES -= b.Progress;
            // QuVR_State.PROGRESS_STATES -= meta.Progress;
        }

        
        
        void Update() {
            if ( progressButtonsOn == eQuVR_ProgressOn.update ) QuVR_State.PROGRESS();
            
            // Left Hand
            x.Set( OVRInput.Get( OVRInput.Button.One, leftController ) );
            y.Set( OVRInput.Get( OVRInput.Button.Two, leftController ) );
            menu.Set( OVRInput.Get( OVRInput.Button.Three, leftController ) );

            lTrigger = OVRInput.Get( OVRInput.Axis1D.PrimaryIndexTrigger, leftController );
            lGrip = OVRInput.Get( OVRInput.Axis1D.PrimaryHandTrigger, leftController );

            lThumbStick = OVRInput.Get( OVRInput.Axis2D.PrimaryThumbstick, leftController );
            lThumbX = lThumbStick.x;
            lThumbY = lThumbStick.y;


            // Right Hand
            a.Set( OVRInput.Get( OVRInput.Button.One, rightController ) );
            b.Set( OVRInput.Get( OVRInput.Button.Two, rightController ) );
            // meta.Set( OVRInput.Get( OVRInput.Button.Three, rightController ) );

            rTrigger = OVRInput.Get( OVRInput.Axis1D.PrimaryIndexTrigger, rightController );
            rGrip = OVRInput.Get( OVRInput.Axis1D.PrimaryHandTrigger, rightController );

            rThumbStick = OVRInput.Get( OVRInput.Axis2D.PrimaryThumbstick, rightController );
            rThumbX = rThumbStick.x;
            rThumbY = rThumbStick.y;

            if ( debugButtonDownAndUp ) {
                // Button Down
                if (a.down) Debug.LogWarning("A Down"  );
                if (b.down) Debug.LogWarning("B Down"  );
                // if (meta.down) Debug.LogWarning("Meta Down"  );
                if (x.down) Debug.LogWarning("X Down"  );
                if (y.down) Debug.LogWarning("Y Down"  );
                if (menu.down) Debug.LogWarning("Menu Down"  );
                
                
                // Button Up
                if (a.up) Debug.LogWarning("A Up"  );
                if (b.up) Debug.LogWarning("B Up"  );
                // if (meta.up) Debug.LogWarning("Meta Up"  );
                if (x.up) Debug.LogWarning("X Up"  );
                if (y.up) Debug.LogWarning("Y Up"  );
                if (menu.up) Debug.LogWarning("Menu Up"  );
            }
        }

        private void FixedUpdate() {
            if ( progressButtonsOn == eQuVR_ProgressOn.fixedUpdate ) QuVR_State.PROGRESS();
        }

        // Static read-only accessors for all contols
        public static QuVR_ButtonState X => S.x;
        public static QuVR_ButtonState Y => S.y;
        public static QuVR_ButtonState A => S.a;
        public static QuVR_ButtonState B => S.b;
        public static QuVR_ButtonState Menu => S.menu;
        // public static QuVR_ButtonState Meta => S.meta;

        public static float LTrigger => S.lTrigger;
        public static float LGrip => S.lGrip;
        public static float LThumbX => S.lThumbX;
        public static float LThumbY => S.lThumbY;
        public static Vector2 LThumbStick => S.lThumbStick;

        public static float RTrigger => S.rTrigger;
        public static float RGrip => S.rGrip;
        public static float RThumbX => S.rThumbX;
        public static float RThumbY => S.rThumbY;
        public static Vector2 RThumbStick => S.rThumbStick;

        // Static read-only accessors for transform information
        public static Vector3 LPos => L_TRANS.position;
        public static Vector3 RPos => R_TRANS.position;
        public static Vector3 HPos => H_TRANS.position;
        public static Quaternion LRot => L_TRANS.rotation;
        public static Quaternion RRot => R_TRANS.rotation;
        public static Quaternion HRot => H_TRANS.rotation;
        public static Vector3 LEuler => L_TRANS.eulerAngles;
        public static Vector3 REuler => R_TRANS.eulerAngles;
        public static Vector3 HEuler => H_TRANS.eulerAngles;

    }
}