using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrganismBranch : MonoBehaviour
{


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
	public int branchLength = 15;
	public Vector3 direction;
	Vector3 basePosition;
	public float growingTimeGap = 1.0f;
	public Vector3 target;
	public List<ObjectData> objectsData;
	GameObject parent;
	public int branchOutPosition;
	public bool hasSubBranch;

	void Start ()
	{
		parent = gameObject.transform.parent.gameObject;
		objectsData = new List<ObjectData> ();
		basePosition = gameObject.transform.position;
		StartCoroutine ("WaitAndGrow");

		branchOutPosition = (int)Random.Range (0, branchLength);
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void stopGrow ()
	{
		StopCoroutine ("WaitAndGrow");
	}
	
	IEnumerator WaitAndGrow ()
	{
		while (objectsData.Count< 20) {
			if (objectsData.Count == 0) { //If this is the first object in the branch
				addObject ();
			} else {
				if (objectsData [objectsData.Count - 1].myGameObject.GetComponent<OrganismObject> ().growingCompleted || objectsData.Count == 0) {
					addObject ();
				}
			}
			yield return new WaitForSeconds (growingTimeGap);
		}
	}
	
	public void addObject ()
	{
		Organism parentScript = parent.GetComponent<Organism> ();

		Vector3 previousObjectPos = parent.transform.position;// + Vector3.up;   //default pos is branch's base position

		if (objectsData.Count > 0) {
			previousObjectPos = objectsData [objectsData.Count - 1].myGameObject.transform.position;
		}


		Vector3 newPosition = previousObjectPos + direction;
		GameObject newObject = (GameObject)Instantiate (Resources.Load ("corgi_withcollider"), previousObjectPos, Quaternion.identity);
		newObject.name = "Object" + objectsData.Count;
		newObject.transform.parent = gameObject.transform;
		OrganismObject myOrgan = newObject.AddComponent<OrganismObject> ();
		myOrgan.myIndex = objectsData.Count;
		myOrgan.targetLocation = newPosition;

		if(objectsData.Count > 0){
			myOrgan.previousGameObject = objectsData [objectsData.Count - 1].myGameObject;
		}else{
			myOrgan.previousGameObject = parent.transform.FindChild("Base").transform.gameObject;
		}



		//INIT STATE
//		StickyStickStuckPackage.StickyStickStuck newObjectSSS = newObject.AddComponent<StickyStickStuckPackage.StickyStickStuck> ();
		Rigidbody newObjectRigidBody = newObject.GetComponent<Rigidbody> ();

		//set it invisible before it actually sticks to anything
//		newObject.GetComponent<MeshRenderer> ().enabled = false;
	
		//Rigidbody parameters change
		newObjectRigidBody.mass = 0.1f;
		newObjectRigidBody.drag = 0.5f;
		newObjectRigidBody.angularDrag = 0.0f;
		newObjectRigidBody.isKinematic = true;

		Collider[] colliders = newObject.GetComponents<Collider>();


		for (int i=0; i<colliders.Length; i++) {
			colliders [i].isTrigger = true;

		}

//		newObjectSSS.stickProperties.stickNonRigidbodys = false;
//		newObjectSSS.infectionProperties.affectInfected = true;


		objectsData.Add (new ObjectData (objectsData.Count, newObject));


//		if (objectsData.Count == 4 && hasSubBranch) {
//			int newBornBranchID = parentScript.branches.Count - 1;
//
//			parentScript.addBranch (objectsData [objectsData.Count - 1].myGameObject.transform.position,
//			                       Vector3.right,
//			                       9,
//			                       2,
//			                        false
//			);
//		}
//
//
//		if (objectsData.Count == 7 && hasSubBranch) {
//			int newBornBranchID = parentScript.branches.Count - 1;
//			
//			parentScript.addBranch (objectsData [objectsData.Count - 1].myGameObject.transform.position,
//			                        Vector3.left,
//			                        7,
//			                        2,	
//			                       false);
//		}
//


	}


}
