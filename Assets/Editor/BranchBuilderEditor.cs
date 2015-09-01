using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Organism))]
public class BranchBuilderEditor : Editor {
	public override void OnInspectorGUI(){
		Organism myScript = (Organism)target;
		if(GUILayout.Button("Generate A Branch")){
//			myScript.addBranch();
		}
	}

}
