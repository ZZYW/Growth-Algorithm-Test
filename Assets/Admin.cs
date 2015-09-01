using UnityEngine;
using System.Collections;

public class Admin : MonoBehaviour {

	GameObject testObject;
	// Use this for initialization
	void Start () {
		testObject = GameObject.Find("John");
	}
	
	// Update is called once per frame
	void Update () {
		float a = testObject.GetComponent<Male>().height;
		Debug.Log(a);

	}
}
