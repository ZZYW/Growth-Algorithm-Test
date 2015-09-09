using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Organism))]
//[CanEditMultipleObjects]
public class OrganismEditor : Editor {

//
//	SerializedProperty _growingSpeed;
//	SerializedProperty _newBornScale;
//
//	void onEnable(){
//		_growingSpeed = serializedObject.FindProperty("growingSpeed");
//		_newBornScale = serializedObject.FindProperty("newBornScale");
//	}
//
//
	public override void OnInspectorGUI()
	{

		DrawDefaultInspector();

//		//get current values
//		serializedObject.Update();
//
//		//show values
//		EditorGUILayout.Slider(_growingSpeed,0.1f,1.0f);
//
//		//update values
//		serializedObject.ApplyModifiedProperties();
	}
}

