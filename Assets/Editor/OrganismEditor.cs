using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Organism))]
public class OrganismEditor : Editor {
	public override void OnInspectorGUI(){
		Organism myScript = (Organism)target;
		if(GUILayout.Button("Generate A Root Branch")){
			myScript.addBranch(myScript.transform.position, "Root");
		}
		if(GUILayout.Button("Generate A Sprout Branch")){
			myScript.addBranch(myScript.transform.position, "Sprout");
		}
		if(GUILayout.Button("Generate A Cluster Branch")){
			myScript.addBranch(myScript.transform.position, "Cluster");
		}
	}

}
