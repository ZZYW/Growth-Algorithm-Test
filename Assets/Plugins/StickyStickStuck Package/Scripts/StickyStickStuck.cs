/*******************************************************************************************
 *       Author: Lane Gresham, AKA LaneMax
 *         Blog: http://lanemax.blogspot.com/
 *      Version: 1.00
 * Created Date: 02/05/14
 * Last Updated: 02/05/14
 *  
 *  Description: 
 *      
 * 
 *  How To Use:
 *      Simply drag and drop or dynamically add the script on whatever gameobject you want, 
 *      remember that you must have a collider on the gameobject in order for it to work.
 * 
 *  Inputs:
 *      enable: Enables or disables the StickyStickStuck effect.
 *      
 *      stickProperties->
 *          stickType: Lets you pick between three different stick types Fixed, Character, 
 *                     and spring.
 *                     
 *          stickNonRigidbodys: Allows you to pick whether you want to stick to objects
 *                              that don't have rigidbodys or not.
 *          
 *          fixedProperties->
 *              breakForce: Break force for fixed joint.
 *              breakTorque: Break torque for fixed joint.
 *              
 *          characterProperties->
 *              anchor: Character anchor point.
 *              axis: Character axis point.
 *              swingAxis: Character swing axis point.
 *              
 *              lowTwistLimit->
 *                  limit: Character low twist limit.
 *                  bounciness: Character low twist limit bounciness.
 *                  spring: Character low twist limit spring.
 *                  damper: Character low twist limit damper.
 *                  
 *              highTwistLimit->
 *                  limit: Character high twist limit.
 *                  bounciness: Character high twist limit bounciness.
 *                  spring: Character high twist limit spring.
 *                  damper: Character high twist limit damper.
 *                  
 *              swing1Limit->
 *                  limit: Character swing 1 limit.
 *                  bounciness: Character swing 1 limit bounciness.
 *                  spring: Character swing 1 limit spring.
 *                  damper: Character swing 1 damper.
 *                  
 *              swing2Limit->
 *                  limit: Character swing 2 limit.
 *                  bounciness: Character swing 2 limit bounciness.
 *                  spring: Character swing 2 limit spring.
 *                  damper: Character swing 2 damper.
 *                  
 *              breakForce: Break force for character joint.
 *              breakTorque: Break force for character joint.
 *              
 *          maxStuck: The maximum number of objects that can stick to the GameObject.
 * 
 *      onColliderStickProperties->
 *          isTrigger: Turns the collision to a trigger, this is very useful when dealing
 *                     with fast moving objects. (Note: This property will not cary over 
 *                     to other child objects that get effected through the recursive 
 *                     infection options.)
 *          
 *      specialEffectsProperties->
 *          specialEffect: GameObject that gets created for the special effects.
 *          
 *          AffectNonRigidbodys: If you want the effect to be added to non rigidbodys.
 *          
 *      infectionProperties->
 *          recursiveInfection: Recursively goes through and adds the StickyStickStuck
 *                              to whatever GameObject it touches.
 *                              
 *          affectInfected: Allows you to effect a GameObject that already as the 
 *                          StickyStickStuck on it.
 *                          
 *      taggingProperties->
 *          useTagging: When enabled, will rename tag to whatever the TagNameOnStick is set 
 *                      to when the object gets stuck.
 *          TagNameOnStick: Name of the tag you what to use.
 *          
 *          BackupTag: Backs up the original tag so that when cleanup happens it resets the
 *                     tag to its original name.
 * 
 *******************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StickyStickStuckPackage
{
    public class StickyStickStuck : MonoBehaviour
    {
        #region GameObject Constructor/Destructor

        //Destructor, this gets fired when the object is destroyed or inactive
        void OnDisable()
        {
            CleanUp();
        }

        #endregion

        #region Properties

        public bool enable = true;

        [System.Serializable]
        public class StickProperties
        {
            public enum StickType
            {
                Fixed,
                Character
            }
            public StickType stickType = StickType.Fixed;

            public bool stickNonRigidbodys = false;

            [System.Serializable]
            public class FixedProperties
            {
                public float breakForce = Mathf.Infinity;
                public float breakTorque = Mathf.Infinity;
            }
            public FixedProperties fixedProperties;

            [System.Serializable]
            public class CharacterProperties
            {
                public Vector3 anchor = new Vector3(0f, 0.5f, 0f);
                public Vector3 axis = new Vector3(1f, 0f, 0f);
                public Vector3 swingAxis = new Vector3(0f, 1f, 0f);

                [System.Serializable]
                public class LowTwistLimit
                {
                    public float limit = -20f;
                    public float bounciness = 0f;
                    public float spring = 0f;
                    public float damper = 0f;
                }
                public LowTwistLimit lowTwistLimit;

                [System.Serializable]
                public class HighTwistLimit
                {
                    public float limit = 70f;
                    public float bounciness = 0f;
                    public float spring = 0f;
                    public float damper = 0f;
                }
                public HighTwistLimit highTwistLimit;

                [System.Serializable]
                public class Swing1Limit
                {
                    public float limit = 40f;
                    public float bounciness = 0f;
                    public float spring = 0f;
                    public float damper = 0f;
                }
                public Swing1Limit swing1Limit;

                [System.Serializable]
                public class Swing2Limit
                {
                    public float limit = 0f;
                    public float bounciness = 0f;
                    public float spring = 0f;
                    public float damper = 0f;
                }
                public Swing2Limit swing2Limit;

                public float breakForce = Mathf.Infinity;
                public float breakTorque = Mathf.Infinity;
            }
            public CharacterProperties characterProperties;

            public float maxStuck = 10f;
        }
        public StickProperties stickProperties;

        [System.Serializable]
        public class OnColliderStickProperties
        {
            public bool isTrigger = false;
        }
        public OnColliderStickProperties onColliderStickProperties;

        [System.Serializable]
        public class SpecialEffectsProperties
        {
            public GameObject specialEffect;
            public bool AffectNonRigidbodys = false;
        }
        public SpecialEffectsProperties specialEffectsProperties;

        [System.Serializable]
        public class InfectionProperties
        {
            public bool recursiveInfection = false;
            public bool affectInfected = false;
        }
        public InfectionProperties infectionProperties;

		[System.Serializable]
		public class TaggingProperties
		{
            public bool useTagging = false;

			public string TagNameOnStick;

            [HideInInspector]
            public string BackupTag;
		}
		public TaggingProperties taggingProperties;

        #endregion

        #region private Properties

        //Name of the GameObject when getting created by the special effects
        private static string specialEffectName = "StickYStickStuck_SpecialEffect";

        //Used for telling which StickYStickStuck object is the parent
        private bool isInfectParent = true;

        //List of connected objects
        private List<ConnectedObjectData> connectedObjectDataList { get; set; }

        //Data structure for the stored connected objects
        private class ConnectedObjectData
        {
            public GameObject gameObject { get; set; }

            public FixedJoint fixedJoint { get; set; }
            public CharacterJoint characterJoint { get; set; }
        }

        #endregion

        #region Unity Functions

        void Awake()
        {
            //Used when dynamicly created the StickyStickStuck object
            if (stickProperties == null)
            {
                stickProperties = new StickProperties();

                if (stickProperties.fixedProperties == null)
                {
                    stickProperties.fixedProperties = new StickProperties.FixedProperties();
                }

                if (stickProperties.characterProperties == null)
                {
                    stickProperties.characterProperties = new StickProperties.CharacterProperties();

                    stickProperties.characterProperties.lowTwistLimit = new StickProperties.CharacterProperties.LowTwistLimit();
                    stickProperties.characterProperties.highTwistLimit = new StickProperties.CharacterProperties.HighTwistLimit();
                    stickProperties.characterProperties.swing1Limit = new StickProperties.CharacterProperties.Swing1Limit();
                    stickProperties.characterProperties.swing2Limit = new StickProperties.CharacterProperties.Swing2Limit();
                }
            }

            if (onColliderStickProperties == null)
            {
                onColliderStickProperties = new OnColliderStickProperties();
            }

            if (specialEffectsProperties == null)
            {
                specialEffectsProperties = new SpecialEffectsProperties();
            }

            if (infectionProperties == null)
            {
                infectionProperties = new InfectionProperties();
            }

            if (connectedObjectDataList == null)
            {
                connectedObjectDataList = new List<ConnectedObjectData>();
            }

			if(taggingProperties == null)
			{
				taggingProperties = new TaggingProperties();
			}
        }

        void Start()
        {
            StickyStickStuckValidation();
        }

        void Update()
        {
        }

        void FixedUpdate()
        {
            //Cleans up the mess when enabled equals false
            if (!enable)
            {
                CleanUp();
            }
        }

        void OnCollisionStay(Collision collision)
        {
            //Used to add the joint when collision happens
            if (enable)
            {
                AddStickyJoint(this.gameObject, collision);
            }
        }

        /*
         * Note: There are two diffrent collision functions you can use
         *       OnCollisionStay, and OnCollisionEnter. Feel free to try
         *       them both and see which one you like better, OnCollisionStay
         *       for me seems to work better, for a fast stick.
         * 
        void OnCollisionEnter(Collision collision)
        {
            //Used to add the joint when collision happens
            if (enable)
            {
                AddStickyJoint(this.gameObject, collision);
            }
        }
        */

        //For when the joint breaks
        void OnJointBreak(float breakForce)
        {
            CleanUp();
        }

        #endregion

        #region Physics / Collision

        private void AddStickyJoint(GameObject parent, Collision whatThisHit)
        {
            if (CheckIfOkToStick(whatThisHit, parent))
            {
                ConnectedObjectData newConnectedObject = new ConnectedObjectData();
                newConnectedObject.gameObject = whatThisHit.gameObject;

                //If the isTrigger is checked
                if (onColliderStickProperties.isTrigger)
                {
                    parent.GetComponent<Collider>().isTrigger = true;
                }

                //Finds out what Stick Type was used
                switch (stickProperties.stickType)
                {
                    case StickProperties.StickType.Fixed:

                        //Sets up the Fixed joint properties
                        FixedJoint fixedJoint = new FixedJoint();
                        fixedJoint = parent.AddComponent<FixedJoint>();
                        newConnectedObject.fixedJoint = fixedJoint;

                        newConnectedObject.fixedJoint.connectedBody = newConnectedObject.gameObject.GetComponent<Rigidbody>();

                        newConnectedObject.fixedJoint.breakForce = this.stickProperties.fixedProperties.breakForce;
                        newConnectedObject.fixedJoint.breakTorque = this.stickProperties.fixedProperties.breakTorque;

                        break;
                    case StickProperties.StickType.Character:

                        //Sets up the Character joint properties
                        CharacterJoint characterJoint = new CharacterJoint();
                        characterJoint = parent.AddComponent<CharacterJoint>();
                        newConnectedObject.characterJoint = characterJoint;

                        newConnectedObject.characterJoint.connectedBody = newConnectedObject.gameObject.GetComponent<Rigidbody>();

                        newConnectedObject.characterJoint.anchor = this.stickProperties.characterProperties.anchor;
                        newConnectedObject.characterJoint.axis = this.stickProperties.characterProperties.axis;
                        newConnectedObject.characterJoint.swingAxis = this.stickProperties.characterProperties.swingAxis;
                        SoftJointLimit lowTwistLimit = new SoftJointLimit();
                        lowTwistLimit.limit = this.stickProperties.characterProperties.lowTwistLimit.limit;
                        lowTwistLimit.bounciness = this.stickProperties.characterProperties.lowTwistLimit.bounciness;
//                        lowTwistLimit.spring = this.stickProperties.characterProperties.lowTwistLimit.spring;
//                        lowTwistLimit.damper = this.stickProperties.characterProperties.lowTwistLimit.damper;
                        newConnectedObject.characterJoint.lowTwistLimit = lowTwistLimit;
                        SoftJointLimit highTwistLimit = new SoftJointLimit();
                        highTwistLimit.limit = this.stickProperties.characterProperties.highTwistLimit.limit;
                        highTwistLimit.bounciness = this.stickProperties.characterProperties.highTwistLimit.bounciness;
//                        highTwistLimit.spring = this.stickProperties.characterProperties.highTwistLimit.spring;
//                        highTwistLimit.damper = this.stickProperties.characterProperties.highTwistLimit.damper;
                        newConnectedObject.characterJoint.highTwistLimit = highTwistLimit;
                        SoftJointLimit swing1Limit = new SoftJointLimit();
                        swing1Limit.limit = this.stickProperties.characterProperties.swing1Limit.limit;
                        swing1Limit.bounciness = this.stickProperties.characterProperties.swing1Limit.bounciness;
//                        swing1Limit.spring = this.stickProperties.characterProperties.swing1Limit.spring;
//                        swing1Limit.damper = this.stickProperties.characterProperties.swing1Limit.damper;
                        newConnectedObject.characterJoint.swing1Limit = swing1Limit;
                        SoftJointLimit swing2Limit = new SoftJointLimit();
                        swing2Limit.limit = this.stickProperties.characterProperties.swing2Limit.limit;
                        swing2Limit.bounciness = this.stickProperties.characterProperties.swing2Limit.bounciness;
//                        swing2Limit.spring = this.stickProperties.characterProperties.swing2Limit.spring;
//                        swing2Limit.damper = this.stickProperties.characterProperties.swing2Limit.damper;
                        newConnectedObject.characterJoint.swing2Limit = swing2Limit;
                        newConnectedObject.characterJoint.breakForce = this.stickProperties.characterProperties.breakForce;
                        newConnectedObject.characterJoint.breakTorque = this.stickProperties.characterProperties.breakTorque;

                        break;
                }

                //Creates the special effect if setup
                if (specialEffectsProperties.specialEffect != null)
                {
                    CreateAttachedGameObject(newConnectedObject.gameObject);
                }

                //Used for when Recursive Infection is checked
                if (infectionProperties.recursiveInfection)
                {
                    if (newConnectedObject.gameObject.GetComponent<StickyStickStuck>() == null)
                    {
                        StickyStickStuck stickyStickStuck = newConnectedObject.gameObject.AddComponent<StickyStickStuck>();

                        stickyStickStuck.enable = this.enable;

                        stickyStickStuck.stickProperties.stickType = this.stickProperties.stickType;
                        stickyStickStuck.stickProperties.stickNonRigidbodys = this.stickProperties.stickNonRigidbodys;

                        //Fixed Properties
                        stickyStickStuck.stickProperties.fixedProperties.breakForce = this.stickProperties.fixedProperties.breakForce;
                        stickyStickStuck.stickProperties.fixedProperties.breakTorque = this.stickProperties.fixedProperties.breakTorque;

                        //Character Properties
                        stickyStickStuck.stickProperties.characterProperties.anchor = this.stickProperties.characterProperties.anchor;
                        stickyStickStuck.stickProperties.characterProperties.axis = this.stickProperties.characterProperties.axis;
                        stickyStickStuck.stickProperties.characterProperties.swingAxis = this.stickProperties.characterProperties.swingAxis;
                        stickyStickStuck.stickProperties.characterProperties.lowTwistLimit.limit = this.stickProperties.characterProperties.lowTwistLimit.limit;
                        stickyStickStuck.stickProperties.characterProperties.lowTwistLimit.bounciness = this.stickProperties.characterProperties.lowTwistLimit.bounciness;
                        stickyStickStuck.stickProperties.characterProperties.lowTwistLimit.spring = this.stickProperties.characterProperties.lowTwistLimit.spring;
                        stickyStickStuck.stickProperties.characterProperties.lowTwistLimit.damper = this.stickProperties.characterProperties.lowTwistLimit.damper;
                        stickyStickStuck.stickProperties.characterProperties.highTwistLimit.limit = this.stickProperties.characterProperties.highTwistLimit.limit;
                        stickyStickStuck.stickProperties.characterProperties.highTwistLimit.bounciness = this.stickProperties.characterProperties.highTwistLimit.bounciness;
                        stickyStickStuck.stickProperties.characterProperties.highTwistLimit.spring = this.stickProperties.characterProperties.highTwistLimit.spring;
                        stickyStickStuck.stickProperties.characterProperties.highTwistLimit.damper = this.stickProperties.characterProperties.highTwistLimit.damper;
                        stickyStickStuck.stickProperties.characterProperties.swing1Limit.limit = this.stickProperties.characterProperties.swing1Limit.limit;
                        stickyStickStuck.stickProperties.characterProperties.swing1Limit.bounciness = this.stickProperties.characterProperties.swing1Limit.bounciness;
                        stickyStickStuck.stickProperties.characterProperties.swing1Limit.spring = this.stickProperties.characterProperties.swing1Limit.spring;
                        stickyStickStuck.stickProperties.characterProperties.swing1Limit.damper = this.stickProperties.characterProperties.swing1Limit.damper;
                        stickyStickStuck.stickProperties.characterProperties.swing2Limit.limit = this.stickProperties.characterProperties.swing2Limit.limit;
                        stickyStickStuck.stickProperties.characterProperties.swing2Limit.bounciness = this.stickProperties.characterProperties.swing2Limit.bounciness;
                        stickyStickStuck.stickProperties.characterProperties.swing2Limit.spring = this.stickProperties.characterProperties.swing2Limit.spring;
                        stickyStickStuck.stickProperties.characterProperties.swing2Limit.damper = this.stickProperties.characterProperties.swing2Limit.damper;
                        stickyStickStuck.stickProperties.characterProperties.breakForce = this.stickProperties.characterProperties.breakForce;
                        stickyStickStuck.stickProperties.characterProperties.breakTorque = this.stickProperties.characterProperties.breakTorque;

                        stickyStickStuck.stickProperties.maxStuck = this.stickProperties.maxStuck;

                        stickyStickStuck.specialEffectsProperties.specialEffect = this.specialEffectsProperties.specialEffect;
                        stickyStickStuck.specialEffectsProperties.AffectNonRigidbodys = this.specialEffectsProperties.AffectNonRigidbodys;

                        stickyStickStuck.infectionProperties.recursiveInfection = this.infectionProperties.recursiveInfection;
                        stickyStickStuck.infectionProperties.affectInfected = this.infectionProperties.affectInfected;

                        stickyStickStuck.taggingProperties.useTagging = this.taggingProperties.useTagging;
                        stickyStickStuck.taggingProperties.TagNameOnStick = this.taggingProperties.TagNameOnStick;

                        //Sets the children isInfectParent to false
                        stickyStickStuck.isInfectParent = false;
                    }
                }

				if(taggingProperties.useTagging)
				{
					taggingProperties.BackupTag = newConnectedObject.gameObject.tag;
					newConnectedObject.gameObject.tag = taggingProperties.TagNameOnStick;
				}

                connectedObjectDataList.Add(newConnectedObject);
            }
        }

        //Check to see if the joint exists for the given GameObject
        private bool CheckIfJointExistsForGameObject(GameObject gameObject)
        {
            foreach (var item in connectedObjectDataList)
            {
                if (item.gameObject == gameObject)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Functions

        private void StickyStickStuckValidation()
        {
            if (this.GetComponent<Collider>() == null)
            {
                Debug.LogError(string.Format("Object {0} must have a collider for StickyStickStuck to work!", this.name));
            }
            else if (!this.GetComponent<Collider>().enabled)
            {
                Debug.LogError(string.Format("Object {0} collider must be enabled for StickyStickStuck to work!", this.name));
            }
        }

        //Creates the special effect gameobject and adds it to the touched object
        private void CreateAttachedGameObject(GameObject touchedObject)
        {
            if (touchedObject.transform.FindChild(specialEffectName) == null)
            {
                if (CheckIfOkToAddSpecialEffect(touchedObject.gameObject))
                {
                    GameObject newAttachGameObject = Instantiate
                    (
                        specialEffectsProperties.specialEffect,
                        touchedObject.gameObject.transform.position,
                        touchedObject.gameObject.transform.rotation
                    ) as GameObject;

                    newAttachGameObject.name = specialEffectName;
                    newAttachGameObject.transform.parent = touchedObject.gameObject.transform;
                }
            }
        }

        //This is where it check to see if its ok to add a sticky joint
        private bool CheckIfOkToStick(Collision whatThisHit, GameObject parent)
        {
            //if whatthishit equals null then fail
            if (whatThisHit == null)
            {
                return false;
            }

            //if whatthishit collider equals the parents collider then fail
            if (whatThisHit.gameObject.GetComponent<Collider>() == parent.GetComponent<Collider>())
            {
                return false;
            }

            //If stickNonRigidbodys is false
            if (!stickProperties.stickNonRigidbodys)
            {
                //If the whatThisHit rigidbody equals null then fail
                if (whatThisHit.rigidbody == null)
                {
                    return false;
                }
            }

            //If the connected amount of objects is greater than the max sticked size then fail
            if (connectedObjectDataList.Count >= stickProperties.maxStuck)
            {
                return false;
            }

            //Fail if joint exists
            if (CheckIfJointExistsForGameObject(whatThisHit.gameObject))
            {
                return false;
            }

            //If affect infected is checked
            if (!infectionProperties.affectInfected)
            {
                //If StickyStickStuck exists then fail
                if (whatThisHit.gameObject.GetComponent<StickyStickStuck>() != null)
                {
                    return false;
                }
            }

            return true;
        }

        //Checks to see if its ok to add the special effect
        private bool CheckIfOkToAddSpecialEffect(GameObject gameObject)
        {
            if (!specialEffectsProperties.AffectNonRigidbodys)
            {
                if (gameObject.GetComponent<Rigidbody>() == null)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region CleanUp

        //Cleans up the mess
        private void CleanUp()
        {
            //Checks to see of any of the connected objects exist
            if (connectedObjectDataList.Count > 0)
            {
                //Resets the object for a trigger to non-trigger
                if (onColliderStickProperties.isTrigger == true)
                {
                    CleanupIsTriggerGameObjects();
                }

                //Cleans up the special effect attached GameObjects
                CleanupAttachedGameObjects();

                //Cleans up joins
                CleanupJoints();

                //Cleans up all objects intected by the StickyStickStuck scripted
                InfectCleanUp();

				TaggingCleanup();
            }

            //Cleans the connected object data list
            connectedObjectDataList.Clear();

            //If not the parent destroy the StickyStickStuck object
            if (!isInfectParent)
            {
                Destroy(this.GetComponent<StickyStickStuck>());
            }
        }

        //Cleans up all the joints
        private void CleanupJoints()
        {
            foreach (var item in connectedObjectDataList)
            {
                if (item.fixedJoint != null)
                {
                    Destroy(item.fixedJoint);
                }

                if (item.characterJoint != null)
                {
                    Destroy(item.characterJoint);
                }
            }
        }

        //Cleans up the triggers
        private void CleanupIsTriggerGameObjects()
        {
            this.gameObject.GetComponent<Collider>().isTrigger = false;
        }

        //Cleans up the special effect created GameObjects
        private void CleanupAttachedGameObjects()
        {
            foreach (var item in connectedObjectDataList)
            {
                if (item.gameObject != null)
                {
                    Transform getAttachedGameObject = item.gameObject.transform.FindChild(specialEffectName);

                    if (getAttachedGameObject != null)
                    {
                        Destroy(getAttachedGameObject.gameObject);
                    }
                }
            }
        }

        //Cleans up the infected
        private void InfectCleanUp()
        {
            foreach (var item in connectedObjectDataList)
            {
                if (item.gameObject != null)
                {
                    StickyStickStuck stickyStickStuck = item.gameObject.GetComponent<StickyStickStuck>();

                    if (stickyStickStuck != null)
                    {
                        if (stickyStickStuck.isInfectParent == false)
                        {
                            stickyStickStuck.enable = false;
                        }
                    }
                }
            }
        }

		private void TaggingCleanup ()
		{
			foreach (var item in connectedObjectDataList)
			{
				if (item.gameObject != null)
				{
					StickyStickStuck stickyStickStuck = item.gameObject.GetComponent<StickyStickStuck>();

					if (stickyStickStuck != null && stickyStickStuck.taggingProperties.useTagging)
					{
						item.gameObject.tag = taggingProperties.BackupTag;
					}
				}
			}
		}

        #endregion
    }
}