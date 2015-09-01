using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(OrganismBranchRoot))]
public class ObjectBuilderEditor : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		OrganismBranchRoot generateObjectScript = (OrganismBranchRoot)target;
		if(GUILayout.Button("Build An Object")){
			generateObjectScript.addObject();
		}
		if(GUILayout.Button("Start Grow")){
			generateObjectScript.StartCoroutine("WaitAndGrow");
		}

		if(GUILayout.Button("Stop Grow")){
			generateObjectScript.StopCoroutine("WaitAndGrow");
		}
		EditorGUILayout.LabelField("Has Objects:",generateObjectScript.objectsStr.Count.ToString());
	}

}
