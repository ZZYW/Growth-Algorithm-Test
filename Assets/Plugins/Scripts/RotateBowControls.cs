/*******************************************************************************************
 *       Author: Lane Gresham, AKA LaneMax
 *         Blog: http://lanemax.blogspot.com/
 *      Version: 1.00
 * Created Date: 02/05/14
 * Last Updated: 02/05/14
 *  
 *  Description: 
 *      Controls the horizontal and vertical rotation of the bow, while keeping the z angle at zero.
 * 
 *  How To Use:
 *      Drag and drop on whatever GameObejct you what to control.
 * 
 *  Inputs:
 *      speed: Speed of the rotating controls.
 * 
 *******************************************************************************************/
using UnityEngine;
using System.Collections;

namespace StickyStickStuckPackage
{
    public class RotateBowControls : MonoBehaviour
    {
        #region Properties

        //How fast the turret turns
        public float speed = 50f;

        #endregion

        #region Unity Functions

        //Use this for initialization
        void Start()
        {

        }

        //Update is called once per frame
        void Update()
        {
            this.transform.rotation = this.transform.localRotation;

            float horMovement = Input.GetAxis("Horizontal");
            float verMovement = Input.GetAxis("Vertical");

            if (horMovement != 0)
            {
                //Horizontaly rotates the bow
                this.transform.Rotate(new Vector3(0f, horMovement, 0f) * speed * Time.deltaTime);

                //Sets the z angle to zero
                float x = this.transform.localEulerAngles.x;
                float y = this.transform.localEulerAngles.y;
                this.transform.localEulerAngles = new Vector3(x, y, 0.0f);
            }

            if (verMovement != 0)
            {
                //Vertical rotates the bow
                this.transform.Rotate(new Vector3(-verMovement, 0f, 0f) * speed * Time.deltaTime);
            }
        }

        #endregion
    }
}