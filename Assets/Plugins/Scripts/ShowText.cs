/*******************************************************************************************
 *       Author: Lane Gresham, AKA LaneMax
 *         Blog: http://lanemax.blogspot.com/
 *      Version: 1.00
 * Created Date: 02/05/14
 * Last Updated: 02/05/14
 *  
 *  Description: 
 *  
 *      Allows to display text on the screen.
  * 
 *  Inputs:
 * 
 *      helpInfo: Displays strings onto screen.
 * 
*******************************************************************************************/
using UnityEngine;
using System.Collections;

namespace StickyStickStuckPackage
{
    public class ShowText : MonoBehaviour
    {
        public string[] text;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnGUI()
        {
            float row = 0;
            foreach (var str in text)
            {
                GUI.Label(new Rect(5, (row * 20) + 5, 1000, 22), str);

                row = row + 1f;
            }
        }
    }
}