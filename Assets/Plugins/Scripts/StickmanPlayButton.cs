/*******************************************************************************************
 *       Author: Lane Gresham, AKA LaneMax
 *         Blog: http://lanemax.blogspot.com/
 *      Version: 1.00
 * Created Date: 02/05/14
 * Last Updated: 02/05/14
 *  
 *  Description: 
 *      Makes a GUI play button in the middle of the screen.
 * 
 *  How To Use:
 *      Just drag and drop the script anywhere in the scene, note that you do need
 *      a GameObject named 'GUI Visualizer' that contains the 
 *      StickyStickStuckVisualizer in order for this to work. Also when the play button is 
 *      clicked, it sets the SceneTimeScale to 1 and destroys itself.
 *      
 *  Inputs:
 *      None
 * 
 *******************************************************************************************/
using UnityEngine;
using System.Collections;

namespace StickyStickStuckPackage
{
    public class StickmanPlayButton : MonoBehaviour
    {
        #region Properties

        private bool togglePlay;

        #endregion

        #region Unity Functions

        void Start()
        {
            togglePlay = false;
        }

        void Update()
        {
            if (togglePlay)
            {
                GameObject.Find("GUI Visualizer").GetComponent<StickyStickStuckVisualizer>().hSceneTimeScale = 1;
            }

            if (GameObject.Find("GUI Visualizer").GetComponent<StickyStickStuckVisualizer>().hSceneTimeScale > 0)
            {
                Destroy(this);
            }
        }

        void OnGUI()
        {
            togglePlay = GUI.Button(new Rect((Screen.width / 2) - 70f, (Screen.height / 2) - 20f, 140, 40), "PLAY");
        }

        #endregion
    }
}