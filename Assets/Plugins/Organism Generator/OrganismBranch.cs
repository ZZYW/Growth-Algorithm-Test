using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrganismBranch : MonoBehaviour
{


	bool branched = false;
	public Vector3 displacement = Vector3.right / 5;
	Vector3 basePosition;
	public float growingTimeGap = 5.0f;
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
	Transform myParent;

	void Start ()
	{
		myParent = gameObject.transform.parent;
		objectsData = new List<ObjectData> ();
		basePosition = gameObject.transform.position;
		StartCoroutine ("WaitAndGrow");
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
		for (int i=0; i<10; i++) {
			addObject ();
			yield return new WaitForSeconds (growingTimeGap);
		}
	}
	
	public void addObject ()
	{
		Vector3 previousObjectPos = gameObject.transform.parent.GetComponent<Organism> ().baseObject.transform.position + Vector3.up; //default pos is branch's base position
		if (objectsData.Count > 0)
			previousObjectPos = objectsData [objectsData.Count - 1].myGameObject.GetComponent<MeshRenderer> ().bounds.center;
		

		Vector3 newPosition = previousObjectPos + Vector3.up * myParent.GetComponent<Organism> ().objectDropingDistance + Vector3.right / 3;

		GameObject newObject = (GameObject)Instantiate (Resources.Load ("corgi_withcollider"), newPosition, Quaternion.identity);
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
	}
}
