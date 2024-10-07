#define USE_CUSTOM_PROPERTY_DRAWER

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace QuickVR {
    public enum eQuVR_ProgressOn { update, fixedUpdate };
    
    /// <summary>
    /// I have not completed moving everything that I should from QuVR_ButtonState to this class - JGB 2024-10-02 
    /// </summary>
    [System.Serializable]
    public class QuVR_State {
        static public event System.Action PROGRESS_STATES;
        static public void PROGRESS() {
            PROGRESS_STATES?.Invoke();
        }
        
        public string name;
        [HideInInspector]
        public System.Guid id;

        public virtual void Progress() { }
    }


    /// <summary>
    /// <para>Allows a single enum to store all phases of a button press.</para>
    /// <para>- bools for just being pressed or released this frame are .down and .up.</para>
    /// <para>- bools for status are isHeld and isFree.</para>
    /// <para>- Button .isHeld the same frame as .down and .isFree the same frame as .up.</para>
    /// <para>- Evaluating as bool returns value of .isHeld</para>
    /// </summary>
    [System.Serializable]
    public class QuVR_ButtonState : QuVR_State {
        
        internal const int showDownUpEditorFrames = 1;


        public enum eInputButtonState {
            free, held, up,
            down
        };


        [SerializeField]
        private eInputButtonState _state;
        public eInputButtonState state {
            get { return _state; }
            set {
                _state = value;
                stateString = stat;
                stateChar = Char;
            }
        }
        [HideInInspector]
        public string stateString;
        [HideInInspector]
        public char stateChar;
        internal int showDown = 0, showUp = 0;

        public QuVR_ButtonState( string _name = "" ) {
            if ( _name != "" ) name = _name;
            // This calls the state.set() method as part of this constructor.
            state = eInputButtonState.free;
        }

        // This won't work because the QuVR_ButtonStates are created by QuVR in the Unity Editor via serialization.
        // // This destructor removes the Progress method from the PROGRESS_STATES event
        // ~QuVR_ButtonState() {
        //     PROGRESS_STATES -= Progress;
        // }
        
        /// <summary>
        /// Is this button currently NOT held (i.e., free)?
        /// </summary>
        public bool isFree {
            get { return( state == eInputButtonState.free || state == eInputButtonState.up ); }
        }

        /// <summary>
        /// Is this button currently held.
        /// </summary>
        public bool isHeld {
            get { return( state == eInputButtonState.held || state == eInputButtonState.down ); }
        }

        /// <summary>
        /// Was this button released this frame (more exactly since the last Progress() was called)?
        /// </summary>
        public bool up {
            get { return( state == eInputButtonState.up ); }
        }

        /// <summary>
        /// Was this button pressed this frame (more exactly since the last Progress() was called)?
        /// </summary>
        public bool down {
            get { return( state == eInputButtonState.down ); }
        }

        public override string ToString() {
            return stat;
        }

        public string stat {
            get {
                if ( state == eInputButtonState.free ) return"____";
                if ( state == eInputButtonState.held ) return"HELD";
                if ( state == eInputButtonState.down ) return"DOWN";
                if ( state == eInputButtonState.up ) return"_UP_";
                return"_REL";
            }
        }

        public char Char {
            get {
                if ( state == eInputButtonState.down || ( showDown > 0 ) ) return'v';
                if ( state == eInputButtonState.up || ( showUp > 0 ) ) return'^';
                if ( state == eInputButtonState.free ) return'¯';
                if ( state == eInputButtonState.held ) return'_';
                return' ';
            }
        }

        /// <summary>
        /// Converts eIBS.pressed to eIBS.down
        /// Converts eIBS.released to eIBS.up
        /// </summary>
        override public void Progress() {
            if ( state == eInputButtonState.down ) {
                state = eInputButtonState.held;
            }
            if ( state == eInputButtonState.up ) {
                state = eInputButtonState.free;
            }
        }

        /// <summary>
        /// This assigns the value of the ButtonState based on a bool.
        /// If the state is up or released, and buttonVal==true, state will become pressed
        /// If the state is down or pressed, and buttonVal==false, state will become released
        /// This does NOT automatically Progress() the ButtonState!!!
        /// </summary>
        /// <param name="buttonVal"></param>
        public void Set( bool buttonVal ) {
            if ( buttonVal && isFree ) {
                state = eInputButtonState.down;
                showDown = showDownUpEditorFrames;
            }
            if ( !buttonVal && isHeld ) {
                state = eInputButtonState.up;
                showUp = showDownUpEditorFrames;
            }
        }

        public static implicit operator bool( QuVR_ButtonState bs ) {
            return bs.isHeld;
        }

        public static implicit operator QuVR_ButtonState( bool b ) {
            QuVR_ButtonState bs = new QuVR_ButtonState();
            bs.Set( b );
            return bs;
        }

        public static implicit operator string( QuVR_ButtonState bs ) {
            return bs.stat;
        }
    }



#if UNITY_EDITOR && USE_CUSTOM_PROPERTY_DRAWER
    [CustomPropertyDrawer( typeof(QuVR_ButtonState) )]
    public class ButtonState_Drawer : PropertyDrawer {

        // Draw the property inside the given rect
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
            // Init the SerializedProperty fields
            // SerializedProperty m_name, m_stateString, m_stateChar, m_showDown, m_showUp;
            QuVR_ButtonState bs = property.boxedValue as QuVR_ButtonState;


            // Start property drawing
            EditorGUI.BeginProperty( position, label, property );

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUI.LabelField( position, label );// bs.name );
            EditorGUI.indentLevel = 4;
            EditorGUI.LabelField( position, $"[{bs.stat}]" );
            EditorGUI.indentLevel = 8;
            if ( bs.showDown-- > 0 ) {
                EditorGUI.LabelField( position, $"Down" );
                //EditorUtility.SetDirty( property.serializedObject.targetObject ); // Repaint
            } else if ( bs.showUp-- > 0 ) {
                EditorGUI.LabelField( position, $"Up" );
                //EditorUtility.SetDirty( property.serializedObject.targetObject ); // Repaint
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            // End property drawing
            EditorGUI.EndProperty();

            /*
            SerializedProperty mName = property.FindPropertyRelative( "name" );
            SerializedProperty mStateString = property.FindPropertyRelative( "stateString" );
            SerializedProperty mStateChar = property.FindPropertyRelative( "stateChar" );
            SerializedProperty mShowDown = property.FindPropertyRelative( "showDown" );
            SerializedProperty mShowUp = property.FindPropertyRelative( "showUp" );
            
            // Much of the next several lines was from a conversation with Bing
            
            // Start property drawing
            EditorGUI.BeginProperty(position, label, property);
            
            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
    
            EditorGUI.LabelField( position, mName.stringValue );
            EditorGUI.indentLevel = 4;
            EditorGUI.LabelField( position, $"[{mStateString.stringValue}]" );
            EditorGUI.indentLevel = 8;
            if ( bs.showDown-- > 0 ) {
                EditorGUI.LabelField( position, $"Down" );
                //EditorUtility.SetDirty( property.serializedObject.targetObject ); // Repaint
            } else if ( bs.showUp-- > 0 ) {
                EditorGUI.LabelField( position, $"Up" );
                //EditorUtility.SetDirty( property.serializedObject.targetObject ); // Repaint
            }
    
            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
            
            
            // Calculate rects
            Rect nameRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            Rect valueRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);
            
            // Draw fields
            // EditorGUI.PropertyField(nameRect, m_name, new GUIContent("Name"));
            EditorGUI.PropertyField(nameRect, mStateString, new GUIContent(mName.stringValue));
    
            // End property drawing
            EditorGUI.EndProperty();
            */

            //
            // // Don't make child fields be indented
            // var indent = EditorGUI.indentLevel;
            // EditorGUI.indentLevel = 0;
            //
            // EditorGUI.LabelField( position, property.displayName );
            // EditorGUI.indentLevel = 4;
            // EditorGUI.LabelField( position, $"[{bs}]" );
            // EditorGUI.indentLevel = 8;
            // if ( bs.showDown-- > 0 ) {
            //     EditorGUI.LabelField( position, $"Down" );
            //     //EditorUtility.SetDirty( property.serializedObject.targetObject ); // Repaint
            // } else if ( bs.showUp-- > 0 ) {
            //     EditorGUI.LabelField( position, $"Up" );
            //     //EditorUtility.SetDirty( property.serializedObject.targetObject ); // Repaint
            // }
            //
            // // Set indent back to what it was
            // EditorGUI.indentLevel = indent;
            //
            // EditorGUI.EndProperty();

            if ( Application.isPlaying ) {
                EditorUtility.SetDirty( property.serializedObject.targetObject ); // Repaint
            }
        }

        // Much of the next several lines was from a conversation with Bing
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
            // Calculate height needed for the property
            return EditorGUIUtility.singleLineHeight; // * 2 + 4;
        }
    }


/* Broken old version that doesn't work with the new buttonStates List<ButtonState>
 [CustomPropertyDrawer( typeof(ButtonState) )]
   public class ButtonState_Drawer : PropertyDrawer {
       SerializedProperty m_stat;

       // Draw the property inside the given rect
       public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
           // Init the SerializedProperty fields
           //if ( m_show == null ) m_show = property.FindPropertyRelative( "show" );
           //if ( m_recNum == null ) m_recNum = property.FindPropertyRelative( "recNum" );
           //if ( m_playerName == null ) m_playerName = property.FindPropertyRelative( "playerName" );
           //if ( m_dateTime == null ) m_dateTime = property.FindPropertyRelative( "dateTime" );

           ButtonState bs = fieldInfo.GetValue( property.serializedObject.targetObject ) as ButtonState;


           // Using BeginProperty / EndProperty on the parent property means that
           // prefab override logic works on the entire property.
           EditorGUI.BeginProperty( position, label, property );

           // Draw label
           //position = EditorGUI.PrefixLabel( position, GUIUtility.GetControlID( FocusType.Passive ), GUIContent.none );// label );


           // Don't make child fields be indented
           var indent = EditorGUI.indentLevel;
           EditorGUI.indentLevel = 0;

           EditorGUI.LabelField( position, property.displayName );
           EditorGUI.indentLevel = 4;
           EditorGUI.LabelField( position, $"[{bs}]" );
           EditorGUI.indentLevel = 8;
           if ( bs.showDown-- > 0 ) {
               EditorGUI.LabelField( position, $"Down" );
               //EditorUtility.SetDirty( property.serializedObject.targetObject ); // Repaint
           } else if ( bs.showUp-- > 0 ) {
               EditorGUI.LabelField( position, $"Up" );
               //EditorUtility.SetDirty( property.serializedObject.targetObject ); // Repaint
           }

           // Set indent back to what it was
           EditorGUI.indentLevel = indent;

           EditorGUI.EndProperty();
           if ( Application.isPlaying ) {
               EditorUtility.SetDirty( property.serializedObject.targetObject ); // Repaint
           }
       }
   }
   */

#endif

}