using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(OrganismBranch))]
public class ObjectBuilderEditor : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		OrganismBranch generateObjectScript = (OrganismBranch)target;
		if(GUILayout.Button("Build An Object")){
			generateObjectScript.addObject();
		}


	}

}
