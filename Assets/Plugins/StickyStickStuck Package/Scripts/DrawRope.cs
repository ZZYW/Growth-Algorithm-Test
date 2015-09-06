/*******************************************************************************************
 *       Author: Lane Gresham, AKA LaneMax
 *         Blog: http://lanemax.blogspot.com/
 *      Version: 1.00
 * Created Date: 02/05/14
 * Last Updated: 02/05/14
 *  
 *  Description: 
 *      Creates a line, and draws it based on the points GameObject array.
 * 
 *  How To Use:
 *      Drag and drop the scripted on anything, would recommend the putting the scripted on
 *      the first object within the points list.
 * 
 *  Inputs:
 *      points: Connects the LineRenderer to these points.
 *      
 *      StartSize: Size of the beginning LineRenderer.
 *      
 *      EndSize: Size of the ending LineRenderer.
 *      
 *      material: Material of the LineRenderer.
 * 
 *******************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StickyStickStuckPackage
{
    public class DrawRope : MonoBehaviour
    {
        #region Properties

        public GameObject[] points;

        public float StartSize = .1f;
        public float EndSize = .1f;

        public Material material;

        private LineRenderer line;

        #endregion

        #region Unity Functions

        void Start()
        {
            CreateLine();
        }

        void Update()
        {
            RenderRope();
        }

        #endregion

        #region Functions

        //Renders the line positions based on the points
        void RenderRope()
        {
            if (line != null)
            {
                int counter = 0;
                foreach (var item in points)
                {
                    line.SetPosition(counter, item.transform.position);
                    counter++;
                }
            }
        }

        //Creates the LineRenderer
        private void CreateLine()
        {
            if (line == null)
            {
                line = points[0].AddComponent<LineRenderer>();
                line.SetVertexCount(points.Length);
                line.SetWidth(StartSize, EndSize);

                if (material != null)
                {
                    line.material = material;
                }
            }
        }

        #endregion
    }
}