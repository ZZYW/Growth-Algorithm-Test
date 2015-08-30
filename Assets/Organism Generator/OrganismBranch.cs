using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrganismBranch : MonoBehaviour {
	
	int index;
	Vector3 basePosition;
	public bool generate_one_object = false;

	// Use this for initialization
	void Start () {
		basePosition = new Vector3(0,0,0);
		index = 0;
		addObject();
	}
	
	// Update is called once per frame
	void Update () {
		if(generate_one_object){
			addObject();
			generate_one_object = false;
		}
	}

	public void addObject(){


		Object loadModel = Resources.Load("corgi");
		GameObject newObject = (GameObject)Instantiate(loadModel);

		if(index==0){
			newObject.transform.position = basePosition;
		}else{
			string targetObjectName = "Object"+(index-1);
			Debug.Log(targetObjectName);
			GameObject previousObject =  transform.Find(targetObjectName).gameObject;
			Vector3 yIncre = new Vector3(0,0.5f,0);
			newObject.transform.position = previousObject.transform.position + yIncre;
		}



		newObject.AddComponent<OrganismObject>();
		newObject.GetComponent<OrganismObject>().MyIndex = index;
		newObject.transform.parent = gameObject.transform;
		newObject.name = "Object"+ newObject.GetComponent<OrganismObject>().MyIndex;

		index++;
	}
}
