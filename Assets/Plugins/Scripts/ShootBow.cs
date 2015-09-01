/*******************************************************************************************
 *       Author: Lane Gresham, AKA LaneMax
 *         Blog: http://lanemax.blogspot.com/
 *      Version: 1.00
 * Created Date: 02/05/14
 * Last Updated: 02/05/14
 *  
 *  Description: 
 *      Used to spawn an arrow GameObject.
 * 
 *  How To Use:
 *      Draw and drop the script on what object you want it to shoot from. Would recommend a
 *      empty game object.
 * 
 *  Inputs:
 *      inputControl: Input control to fire the arrow GameObject.
 *      
 *      arrow: The GameObject that shoots.
 *      
 *      bulletPower: Force fired from the bow.
 *      
 *      arrowLife: Time before the object gets destroyed.
 * 
 *******************************************************************************************/
using UnityEngine;
using System.Collections;

namespace StickyStickStuckPackage
{
    public class ShootBow : MonoBehaviour
    {
        #region Properties

        //Input to shoot
        public string inputControl = "Jump";

        //GameObject to shoot
        public GameObject arrow;

        //Bullet Speed
        public float bulletPower = 1000f;

        //Bullet life
        public float arrowLife = 30f;

        #endregion

        #region Unity Functions

        //Use this for initialization
        void Start()
        {
        }

        //Update is called once per frame
        void Update()
        {

        }

        void FixedUpdate()
        {
            if (Input.GetButtonDown(inputControl))
            {
                //Gets bow anaimation
                Animation bowAnimation = this.transform.parent.GetComponent<Animation>();

                if (bowAnimation.isPlaying != true)
                {
                    //Plays bow animation
                    bowAnimation.Play();

                    //Creates Arrow
                    GameObject instance = Instantiate(arrow, transform.position, this.transform.rotation) as GameObject;

                    //Shoots Arrow
                    Rigidbody[] ArrayRigs = instance.GetComponentsInChildren<Rigidbody>();
                    foreach (var item in ArrayRigs)
                    {
                        item.GetComponent<Rigidbody>().AddForce(this.transform.forward * bulletPower);
                    }

                    //Destroy arrow after arrowLife
                    DestroyObject(instance, arrowLife);
                }
            }
        }

        #endregion
    }
}