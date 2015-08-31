using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrganismBranch : MonoBehaviour {
	
	int index; //for label all objects in this branch
	Vector3 basePosition;


	public struct ObjectStr {
		public Vector3 pos; //used to store all objects' locations, for convenience
		public int i;
		public ObjectStr (int _i, Vector3 _p ){
			i = _i;
			pos = _p;
		}
	}

	List<ObjectStr> allObjects;
	// Use this for initialization
	void Start () {
		allObjects = new List<ObjectStr>();
		basePosition = gameObject.transform.position;
		index = 0;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("allObjets.count->" + allObjects.Count + "  index->" + index);
	}

	public void addObject(){

		Vector3 previousObjectPos = basePosition; //default pos is branch's base position


		OrganismObject[] allChildScripts;
		allChildScripts = GetComponentsInChildren<OrganismObject>();
		int hasObjectNumber = 0;
		foreach (OrganismObject script in allChildScripts) {
			if(script.MyIndex == allChildScripts.Length-1){
				previousObjectPos = script.transform.position;
			}
		}

//		if(Random.Range(0,100) > 40){
//			Debug.Log("auto generated a new branch");
//			Vector3 temp = new Vector3(2,0,0);
//			gameObject.transform.parent.GetComponent<Organism>().addBranch(previousObjectPos+temp);
//		}


		Object loadModel = Resources.Load("corgi");
		GameObject newObject = (GameObject)Instantiate(loadModel);

		if(index==0){ //if this is the first object in this branch, then place it on the base position of this branch
			newObject.transform.position = basePosition;
		}else{
			Vector3 yIncre = new Vector3(0,0.2f,0);
			newObject.transform.position = allObjects[index-1].pos + yIncre;
		}

		newObject.AddComponent<OrganismObject>();
		newObject.GetComponent<OrganismObject>().MyIndex = index;
		newObject.transform.parent = gameObject.transform;
		newObject.name = "Object"+ newObject.GetComponent<OrganismObject>().MyIndex;

		allObjects.Add(new ObjectStr(index, newObject.transform.position));
		index++;
	}
}
