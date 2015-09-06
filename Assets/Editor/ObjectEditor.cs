using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(OrganismObject))]
public class ObjectEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		OrganismObject myScript = (OrganismObject)target;
		if (GUILayout.Button ("Generate A Root Branch")) {
			myScript.generateBranchBasedOnMe("Root");
		}
		if (GUILayout.Button ("Generate A Sprout Branch")) {
			myScript.generateBranchBasedOnMe("Sprout");
		}
		if (GUILayout.Button ("Generate A Cluster Branch")) {
			myScript.generateBranchBasedOnMe("Cluster");
		}
	}
		
}

