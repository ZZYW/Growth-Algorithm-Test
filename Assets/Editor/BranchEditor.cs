using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(OrganismBranch))]
public class BranchEditor : Editor {

	public override void OnInspectorGUI()
	{
		OrganismBranch mytarget = (OrganismBranch)target;

		if(GUILayout.Button("Break This Branch"))
		{
			mytarget.breakBranch();
		}
	}
}
