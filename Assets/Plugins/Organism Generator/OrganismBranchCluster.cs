using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrganismBranchCluster : MonoBehaviour {
	
	Vector3 basePosition;

	public float growingTimeGap = 3.0f;
	public struct ObjectData
	{
		public GameObject myGameObject;
		public int i;
		
		public ObjectData (int _i, GameObject _g)
		{
			i = _i;
			myGameObject = _g;
		}
	}
	public List<ObjectData> objectsData;
	
	// Use this for initialization
	void Start ()
	{
		objectsData = new List<ObjectData> ();
		basePosition = gameObject.transform.position;
		StartCoroutine ("WaitAndGrow");
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	IEnumerator WaitAndGrow ()
	{
		for (int i=0; i<10; i++) {
			addObject ();
			yield return new WaitForSeconds (growingTimeGap);
		}
	}
	
	public void addObject ()
	{
		Vector3 previousObjectPos = basePosition; //default pos is branch's base position
		Vector3 targetLocation = new Vector3();
		
		if (objectsData.Count > 0) {
			previousObjectPos = objectsData [objectsData.Count - 1].myGameObject.transform.position;
		}
		
		//Small chance to generate a new Branch.
		//		if (Random.Range (0, 100) > 86) {
		//			Debug.Log ("We got a new Branch!");
		//			gameObject.transform.parent.GetComponent<Organism> ().addBranch (previousObjectPos, "Root");
		//		}
		//end
		
		//		GameObject newObject = GameObject.CreatePrimitive (PrimitiveType.Cube);
		
		
		GameObject newObject = (GameObject)Instantiate(Resources.Load("corgi"));
		newObject.AddComponent<SphereCollider>();
		newObject.GetComponent<SphereCollider>().radius = 0.5f ;
		
		if (objectsData.Count == 0) { //if this is the first object in this branch, then place it on the base position of this branch
			newObject.transform.position = basePosition;
			targetLocation = basePosition;
		} else {
			newObject.transform.position = previousObjectPos;
			float yIncrement = Random.Range (-0.5f, 0.5f);
			float xIncrement = Random.Range (-0.5f, 0.5f);
			float zIncrement = Random.Range (-0.5f, 0.5f);
			Vector3 increment = new Vector3 (xIncrement, yIncrement, zIncrement);
			//			increment.
			targetLocation = previousObjectPos + increment;//increment;
		}
		
				newObject.AddComponent<Rigidbody>();
		newObject.AddComponent<StickyStickStuckPackage.StickyStickStuck> ();
		//		newObject.GetComponent<Rigidbody>().isKinematic = true;
		StickyStickStuckPackage.StickyStickStuck stickyStickStuck = newObject.GetComponent<StickyStickStuckPackage.StickyStickStuck>();
		stickyStickStuck.stickProperties.stickNonRigidbodys = true;
		stickyStickStuck.infectionProperties.affectInfected = true;
//		stickyStickStuck.onColliderStickProperties.isTrigger = true;
		stickyStickStuck.stickProperties.fixedProperties.breakTorque = 30.0f;
		//		newObject.GetComponent<Rigidbody>().mass = 0.1f;
		newObject.AddComponent<OrganismObject>();
		newObject.GetComponent<OrganismObject>().targetLocation = targetLocation;
		newObject.GetComponent<OrganismObject>().myIndex = objectsData.Count;
		
		//		newObject.GetComponent<Rigidbody>().mass = 3.0f;
		newObject.transform.parent = gameObject.transform;
		newObject.name = "Object" + objectsData.Count;
		
		//Add new born object to the List
		objectsData.Add (new ObjectData (objectsData.Count, newObject));
	}

}
