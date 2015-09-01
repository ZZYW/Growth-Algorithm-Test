/*******************************************************************************************
 *       Author: Lane Gresham, AKA LaneMax
 *         Blog: http://lanemax.blogspot.com/
 *      Version: 1.00
 * Created Date: 02/05/14
 * Last Updated: 02/05/14
 *  
 *  Description: 
 *      Allows you to visualize, through a GUI interface, a GameObject that has the 
 *      StickyStickStuck script. This is very useful for allowing you to test objects that 
 *      contain the StickyStickStuck script.
 * 
 *  How To Use:
 *      Drag and drop the script on whatever GameObject you what, would recommend you drag
 *      this script on whatever GameObject contains the StickyStickStuck. Once applied,
 *      drag and drop the GameObject that contains the StickyStickStuck onto the 
 *      stickyStickStuckGameObject property within the StickyStickStuckVisualizer.
 * 
 *  Inputs:
 *      stickyStickStuckGameObject: 
 *      
 *      guiLocation: GUI location.
 *      
 *      breakProperties->
 *          defaultAllowStickyBreak: Defaults whether you want AllowStickyBreak to be set.
 *          defaultBreakForce: Defaults Break Force value.
 *          defaultBreakTorque: Defaults Break Torque value. 
 *          
 *      specialEffectProperties->
 *          toggleSpecialEffect: Whether or not the special effects is checked.
 *          defaultSpecialEffectGameObject: Defaulted GameObject for the special effects.
 *      
 *      maxStickedProperties->
 *          min: Minimum of stuck items on the GameObject.
 *          max: Maximum of stuck items on the GameObject.
 *          
 *      sceneTimeScale->
 *          min: Minimum of the scene time scale.
 *          max: Maximum of the scene time scale.
 *          
 *      defaultSceneTimeScale: Sets the default time scale of the scene.
 * 
 *******************************************************************************************/
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace StickyStickStuckPackage
{
    public class StickyStickStuckVisualizer : MonoBehaviour
    {
        #region Properties

        //Assign the current object that has CircularGravity to display the GUI for thoese properties
        public GameObject stickyStickStuckGameObject;

        //Used to set the GUI location
        public Vector2 guiLocation = new Vector2(0f, 0f);

        //Break properties
        [System.Serializable]
        public class BreakProperties
        {
            public bool defaultAllowStickyBreak = false;
            public float defaultBreakForce = 15f;
            public float defaultBreakTorque = 15f;
        }
        public BreakProperties breakProperties;

        //Special effect properties
        [System.Serializable]
        public class SpecialEffectProperties
        {
            public bool toggleSpecialEffect;
            public GameObject defaultSpecialEffectGameObject;
        }
        public SpecialEffectProperties specialEffectProperties;

        //Min and Max for the hMaxSticked
        [System.Serializable]
        public class MaxStickedProperties
        {
            public int min = 1;
            public int max = 20;
        }
        public MaxStickedProperties maxStickedProperties;

        //Min and Max for the Time Scale
        [System.Serializable]
        public class SceneTimeScale
        {
            public float min = 0f;
            public float max = 1f;
        }
        public SceneTimeScale sceneTimeScale;

        //Default time scale
        public float defaultSceneTimeScale = 1.0f;

        #endregion

        #region Private Properties

        private bool toggleEnable;
        private bool toggleIsTrigger;
        private bool toggleFixed;
        private bool toggleCharacter;
        private string stickyTypeLable;
        private bool toggleAllowStickyBreak;
        private bool toggleRecursiveInfection;
        private bool toggleAffectInfected;
        private bool toggleStickNonRigidbodys;
        private float hMaxSticked;

        [HideInInspector]
        public float hSceneTimeScale { get; set; }

        #endregion

        #region Unity Events

        //Use this for initialization
        void Start()
        {
            try
            {
                StickyStickStuck stickyStickStuck = stickyStickStuckGameObject.GetComponent<StickyStickStuck>();

                toggleEnable = stickyStickStuck.enable;
                toggleFixed = false;
                toggleCharacter = false;
                stickyTypeLable = "Sticky Type: " + stickyStickStuck.stickProperties.stickType.ToString();

                if (breakProperties.defaultAllowStickyBreak)
                {
                    toggleAllowStickyBreak = true;
                }
                else
                {
                    toggleAllowStickyBreak = false;
                }

                toggleIsTrigger = stickyStickStuck.onColliderStickProperties.isTrigger;
                toggleRecursiveInfection = stickyStickStuck.infectionProperties.recursiveInfection;
                toggleAffectInfected = stickyStickStuck.infectionProperties.affectInfected;
                toggleStickNonRigidbodys = stickyStickStuck.stickProperties.stickNonRigidbodys;
                hMaxSticked = stickyStickStuck.stickProperties.maxStuck;
                hSceneTimeScale = defaultSceneTimeScale;
            }
            catch (System.Exception)
            {
                Debug.LogWarning("Error reading StickyStickStuck GameObject");
            }
        }

        void Awake()
        {
        }

        //Update is called once per frame
        void Update()
        {
        }

        //Draws the GUI
        void OnGUI()
        {
            float i = 0;
            float spacerXY = 25.0f;

            GUIStyle labelGUIStyle = new GUIStyle();
            labelGUIStyle.normal.textColor = Color.white;
            labelGUIStyle.alignment = TextAnchor.MiddleLeft;

            GUI.Label(new Rect(guiLocation.x, guiLocation.y + ((spacerXY) * i++), 50, 20), stickyTypeLable, labelGUIStyle);
            toggleFixed = GUI.Button(new Rect((guiLocation.x), guiLocation.y + ((spacerXY) * i), 70, 20), "Fixed");
            toggleCharacter = GUI.Button(new Rect((guiLocation.x + 75f), guiLocation.y + ((spacerXY) * i++), 70, 20), "Character");
            toggleEnable = GUI.Toggle(new Rect(guiLocation.x, guiLocation.y + ((spacerXY) * i++), 300, 20), toggleEnable, "Enable");
            toggleIsTrigger = GUI.Toggle(new Rect(guiLocation.x, guiLocation.y + ((spacerXY) * i++), 300, 20), toggleIsTrigger, "Is Trigger on Collision");
            toggleAllowStickyBreak = GUI.Toggle(new Rect(guiLocation.x, guiLocation.y + ((spacerXY) * i++), 300, 20), toggleAllowStickyBreak, "Allow Sticky Break");
            specialEffectProperties.toggleSpecialEffect = GUI.Toggle(new Rect(guiLocation.x, guiLocation.y + ((spacerXY) * i++), 300, 20), specialEffectProperties.toggleSpecialEffect, "Special Effect");
            toggleRecursiveInfection = GUI.Toggle(new Rect(guiLocation.x, guiLocation.y + ((spacerXY) * i++), 300, 20), toggleRecursiveInfection, "Recursive Infection");
            toggleAffectInfected = GUI.Toggle(new Rect(guiLocation.x, guiLocation.y + ((spacerXY) * i++), 300, 20), toggleAffectInfected, "Affect Infected");

            if (toggleRecursiveInfection)
            {
                toggleStickNonRigidbodys = false;
                GUI.Label(new Rect(guiLocation.x + 15f, guiLocation.y + ((spacerXY) * i++), 300, 20), "Stick NonRigidbodys is Dissabled for Recursive Infection");
            }
            else
            {
                toggleStickNonRigidbodys = GUI.Toggle(new Rect(guiLocation.x, guiLocation.y + ((spacerXY) * i++), 300, 20), toggleStickNonRigidbodys, "Stick NonRigidbodys");
            }

            GUI.Label(new Rect(guiLocation.x, guiLocation.y + ((spacerXY) * i), 300, 20), "Max Stuck: " + Mathf.Round(hMaxSticked).ToString(), labelGUIStyle);
            hMaxSticked = GUI.HorizontalSlider(new Rect(guiLocation.x + 100f, guiLocation.y + ((spacerXY + .5f) * i++), 123, 20), hMaxSticked, maxStickedProperties.min, maxStickedProperties.max);

            GUI.Label(new Rect(guiLocation.x, guiLocation.y + ((spacerXY) * i), 300, 20), "Time Scale: ", labelGUIStyle);
            hSceneTimeScale = GUI.HorizontalSlider(new Rect(guiLocation.x + 100f, guiLocation.y + ((spacerXY + .5f) * i++), 123, 20), hSceneTimeScale, sceneTimeScale.min, sceneTimeScale.max);

            if (GUI.Button(new Rect(guiLocation.x + 160f, guiLocation.y + ((spacerXY) * i++), 63, 20), "Reset"))
            {
                Application.LoadLevel(Application.loadedLevelName);

                hSceneTimeScale = 1f;
            }

            try
            {
                StickyStickStuck stickyStickStuck = stickyStickStuckGameObject.GetComponent<StickyStickStuck>();

                stickyStickStuck.enable = toggleEnable;
                stickyStickStuck.onColliderStickProperties.isTrigger = toggleIsTrigger;

                if (toggleAllowStickyBreak)
                {
                    stickyStickStuck.stickProperties.fixedProperties.breakForce = breakProperties.defaultBreakForce;
                    stickyStickStuck.stickProperties.fixedProperties.breakTorque = breakProperties.defaultBreakTorque;
                    stickyStickStuck.stickProperties.characterProperties.breakForce = breakProperties.defaultBreakForce;
                    stickyStickStuck.stickProperties.characterProperties.breakTorque = breakProperties.defaultBreakTorque;

                }
                else
                {
                    stickyStickStuck.stickProperties.fixedProperties.breakForce = Mathf.Infinity;
                    stickyStickStuck.stickProperties.fixedProperties.breakTorque = Mathf.Infinity;
                    stickyStickStuck.stickProperties.characterProperties.breakForce = Mathf.Infinity;
                    stickyStickStuck.stickProperties.characterProperties.breakTorque = Mathf.Infinity;
                }

                if (specialEffectProperties.toggleSpecialEffect)
                {
                    if (specialEffectProperties.defaultSpecialEffectGameObject != null)
                    {
                        stickyStickStuck.specialEffectsProperties.specialEffect = specialEffectProperties.defaultSpecialEffectGameObject;
                    }
                }
                else
                {
                    stickyStickStuck.specialEffectsProperties.specialEffect = null;
                }

                if (toggleFixed)
                {
                    stickyStickStuck.stickProperties.stickType = StickyStickStuck.StickProperties.StickType.Fixed;
                    toggleFixed = false;
                }
                if (toggleCharacter)
                {
                    stickyStickStuck.stickProperties.stickType = StickyStickStuck.StickProperties.StickType.Character;
                    toggleCharacter = false;
                }

                stickyTypeLable = "Sticky Type: " + stickyStickStuck.stickProperties.stickType.ToString();
                stickyStickStuck.infectionProperties.recursiveInfection = toggleRecursiveInfection;
                stickyStickStuck.stickProperties.stickNonRigidbodys = !toggleRecursiveInfection;
                stickyStickStuck.infectionProperties.affectInfected = toggleAffectInfected;
                stickyStickStuck.stickProperties.stickNonRigidbodys = toggleStickNonRigidbodys;
                stickyStickStuck.stickProperties.maxStuck = Mathf.Round(hMaxSticked);

                Time.timeScale = hSceneTimeScale;
            }
            catch (System.Exception)
            {
                Debug.LogWarning("Error reading StickyStickStuck GameObject");
            }
        }

        #endregion
    }
}