using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrganismBranchSprout : MonoBehaviour
{
	Vector3 basePosition;
	public float growingTimeGap = 4.0f;
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

		if (objectsData.Count > 0) {
			previousObjectPos = objectsData [objectsData.Count - 1].myGameObject.GetComponent<MeshRenderer> ().bounds.center;
		}

		GameObject newObject = (GameObject)Instantiate (Resources.Load ("corgi_withcollider"));
		StickyStickStuckPackage.StickyStickStuck newObjectSSS = newObject.AddComponent<StickyStickStuckPackage.StickyStickStuck> ();
		Rigidbody newObjectRigidBody = newObject.GetComponent<Rigidbody> ();
		newObject.transform.position = previousObjectPos + Vector3.up * myParent.GetComponent<Organism>().objectDropingDistance;
		newObject.GetComponent<MeshRenderer>().enabled = false;
	
		//Rigidbody parameters change
		newObjectRigidBody.mass = 0.1f;
		newObjectRigidBody.drag = 3.0f;
		newObjectRigidBody.angularDrag = 0.0f;
		newObjectRigidBody.interpolation = RigidbodyInterpolation.Interpolate;
		newObjectSSS.stickProperties.stickNonRigidbodys = false;
		newObjectSSS.infectionProperties.affectInfected = true;
		OrganismObject myOrgan = newObject.AddComponent<OrganismObject> ();
		myOrgan.myIndex = objectsData.Count;
		newObject.name = "Object" + objectsData.Count;
		newObject.transform.parent = gameObject.transform;
		objectsData.Add (new ObjectData (objectsData.Count, newObject));
	}
}
