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
	public float growingTimeGap;
	public Vector3 target;
	public List<ObjectData> objectsData;
	GameObject parentGameObject;


	void Start ()
	{
		parentGameObject = gameObject.transform.parent.gameObject;
		objectsData = new List<ObjectData> ();
		basePosition = gameObject.transform.position;
		StartCoroutine ("WaitAndGrow");

		Organism myOrganismClass = parentGameObject.GetComponent<Organism>();
		growingTimeGap = myOrganismClass.minimalGeneratingTimeGap;

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
		while (objectsData.Count<branchLength-1) {
			if (objectsData.Count == 0) {
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
		Organism parentScript = parentGameObject.GetComponent<Organism> ();

		Vector3 previousObjectPos = parentScript.baseObject.transform.position + Vector3.up * 2; //default pos is branch's base position
		if (objectsData.Count > 0)
//			previousObjectPos = objectsData [objectsData.Count - 1].myGameObject.GetComponent<MeshRenderer> ().bounds.center;
			previousObjectPos = objectsData[objectsData.Count - 1].myGameObject.transform.position;

		Vector3 newPosition = previousObjectPos + Vector3.up * parentGameObject.GetComponent<Organism> ().objectDropingDistance + direction;
		GameObject newObject = (GameObject)Instantiate (Resources.Load (parentScript.modelName), newPosition, Quaternion.identity);
		StickyStickStuckPackage.StickyStickStuck newObjectSSS = newObject.AddComponent<StickyStickStuckPackage.StickyStickStuck> ();
		Rigidbody newObjectRigidBody = newObject.GetComponent<Rigidbody> ();

		//set it invisible before it actually sticks to anything
		newObject.GetComponent<MeshRenderer> ().enabled = false;
	
		//Rigidbody parameters change
		newObjectRigidBody.mass = 0.1f;
		newObjectRigidBody.drag = 0.5f;
		newObjectRigidBody.angularDrag = 0.0f;
		newObjectSSS.stickProperties.stickNonRigidbodys = false;
		newObjectSSS.infectionProperties.affectInfected = true;
		OrganismObject myOrgan = newObject.AddComponent<OrganismObject> ();
		myOrgan.myIndex = objectsData.Count;
		newObject.name = "Object" + objectsData.Count;
		newObject.transform.parent = gameObject.transform;
		objectsData.Add (new ObjectData (objectsData.Count, newObject));

//
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



	}


}
