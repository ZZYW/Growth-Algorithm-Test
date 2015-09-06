/*******************************************************************************************
 *       Author: Lane Gresham, AKA LaneMax
 *         Blog: http://lanemax.blogspot.com/
 *      Version: 1.00
 * Created Date: 02/05/14
 * Last Updated: 02/05/14
 *  
 *  Description: 
 *  
 *      Allows the camera to look at the given object.
 * 
 *  Inputs:
 * 
 *      lookAtGameObject: Looks at whatever GameObejct is assigned to it.
 * 
*******************************************************************************************/
using UnityEngine;
using System.Collections;

namespace StickyStickStuckPackage
{
    public class CamLookAtObject : MonoBehaviour
    {
        public GameObject lookAtGameObject;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Camera.main.transform.LookAt(lookAtGameObject.transform);
        }
    }
}