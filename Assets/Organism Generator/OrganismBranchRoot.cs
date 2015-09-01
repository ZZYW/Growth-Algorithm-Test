using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrganismBranchRoot : MonoBehaviour
{
	
	Vector3 basePosition;

	float yIncrement = Random.Range(-0.01f, -0.1f);
	float xIncrement = Random.Range(-0.1f, 0.1f);
	float zIncrement = Random.Range(-0.1f, 0.1f);

	public float growingTimeGap = 3.0f;
	public struct ObjectStr
	{
		public GameObject myGameObject;
		public int i;

		public ObjectStr (int _i, GameObject _g)
		{
			i = _i;
			myGameObject = _g;
		}
	}
	public List<ObjectStr> objectsStr;

	// Use this for initialization
	void Start ()
	{
		objectsStr = new List<ObjectStr> ();
		basePosition = gameObject.transform.position;
		StartCoroutine ("WaitAndGrow");
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	IEnumerator WaitAndGrow ()
	{
//		while (true) {
		for(int i=0;i<10;i++){
			addObject ();
			yield return new WaitForSeconds (growingTimeGap);
		}
	}

	public void addObject ()
	{

		Vector3 previousObjectPos = basePosition; //default pos is branch's base position

		if(objectsStr.Count>0){
			previousObjectPos = objectsStr [objectsStr.Count - 1].myGameObject.transform.position;
		}

		//Small chance to generate a new Branch.
		if (Random.Range (0, 100) > 86) {
			Debug.Log ("We got a new Branch!");
			gameObject.transform.parent.GetComponent<Organism> ().addBranch(previousObjectPos,"Root");
		}
		//end

		GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		if (objectsStr.Count == 0) { //if this is the first object in this branch, then place it on the base position of this branch
			newObject.transform.position = basePosition;
		} else {
			//Core Algorithm
			Vector3 increment = new Vector3 (xIncrement, yIncrement, zIncrement);
			newObject.transform.position = previousObjectPos + increment;
		}

		newObject.AddComponent<Rigidbody>();
		newObject.AddComponent<StickyStickStuckPackage.StickyStickStuck>();
		newObject.AddComponent<OrganismObject>();
		newObject.GetComponent<Rigidbody>().mass = 10;
		newObject.GetComponent<OrganismObject> ().MyIndex = objectsStr.Count;
		newObject.transform.parent = gameObject.transform;
		newObject.name = "Object" + objectsStr.Count;
		//Add new born object to the List
		objectsStr.Add (new ObjectStr (objectsStr.Count, newObject));
	}
	
}
