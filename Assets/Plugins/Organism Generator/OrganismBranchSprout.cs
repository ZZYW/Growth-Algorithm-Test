using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrganismBranchSprout : MonoBehaviour {
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
			previousObjectPos = objectsData [objectsData.Count - 1].myGameObject.GetComponent<MeshRenderer>().bounds.center;
		}

		GameObject newObject = (GameObject)Instantiate(Resources.Load("corgi_withcollider"));
		newObject.AddComponent<StickyStickStuckPackage.StickyStickStuck> ();
		Rigidbody newObjectRigidBody = newObject.GetComponent<Rigidbody>();

		StickyStickStuckPackage.StickyStickStuck newObjectSSS = newObject.GetComponent<StickyStickStuckPackage.StickyStickStuck>();
		newObjectSSS.enabled = false;
		newObject.transform.parent = gameObject.transform;//moved up here

		
		if (objectsData.Count == 0) { //if this is the first object in this branch, then place it on the base position of this branch
			newObject.transform.position = basePosition;
			targetLocation = basePosition;
		} else {
			newObject.transform.position = previousObjectPos + Vector3.up * 4;
			float yIncrement = Random.Range (-0.3f, 0.3f);
			float xIncrement = Random.Range (-0.3f, 0.3f);
			float zIncrement = Random.Range (-0.3f, 0.3f);
			Vector3 increment = new Vector3 (xIncrement, yIncrement, zIncrement);
//			targetLocation = previousObjectPos + Vector3.up * objectsData[objectsData.Count-1].myGameObject.GetComponent<MeshRenderer>().bounds.extents.magnitude;//2.5f;//5.0f;//increment;
			targetLocation = previousObjectPos + Vector3.up * 4;
		}
		newObjectRigidBody.mass = 0.1f;
		newObjectRigidBody.drag = 0.3f;
		newObjectRigidBody.angularDrag = 0.0f;
		newObjectRigidBody.interpolation = RigidbodyInterpolation.Interpolate;
		newObjectSSS.stickProperties.stickNonRigidbodys = false;
		newObjectSSS.infectionProperties.affectInfected = true;
		OrganismObject myOrgan = newObject.AddComponent<OrganismObject>();
		myOrgan.targetLocation = targetLocation;
		myOrgan.myIndex = objectsData.Count;
		if(objectsData.Count==0){
			newObject.name = "Base Object";
		}else{
			newObject.name = "Object" + objectsData.Count;
		}
		if(objectsData.Count == 0){
			newObjectRigidBody.constraints = RigidbodyConstraints.FreezeAll;
			newObjectRigidBody.isKinematic = true;
		}
		objectsData.Add (new ObjectData (objectsData.Count, newObject));
	}
}
