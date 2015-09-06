/*******************************************************************************************
 *       Author: Lane Gresham, AKA LaneMax
 *         Blog: http://lanemax.blogspot.com/
 *      Version: 1.00
 * Created Date: 02/05/14
 * Last Updated: 02/05/14
 *  
 *  Description: 
 *      Used to move an object around using the Horizontal, and Vertical controls.
 * 
 *  How To Use:
 *      Draw and drop the script on what object you want to control.
 * 
 *  Inputs:
 *      movementForce: Movement force speed.
 *      
 *      jumpforce: Jump/fly force.
 *      
 * 
 *******************************************************************************************/
using UnityEngine;
using System.Collections;

namespace StickyStickStuckPackage
{
    public class MoveStickyObject : MonoBehaviour
    {
        //Movement force/speed
        public float movementForce = 3.0f;

        //Jump force
        public float jumpforce = 100.0f;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void FixedUpdate()
        {
            //Right Left controls
            float horMovement = movementForce * Input.GetAxis("Horizontal");

            //Forward Backward controls
            float verMovement = movementForce * Input.GetAxis("Vertical");

            if (horMovement != 0)
            {
                this.transform.GetComponent<Rigidbody>().AddForce(new Vector3(horMovement, 0, 0), ForceMode.Impulse);
            }

            if (verMovement != 0)
            {
                this.transform.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, verMovement), ForceMode.Impulse);
            }

            if (Input.GetButton("Jump"))
            {
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpforce);
            }
        }
    }
}