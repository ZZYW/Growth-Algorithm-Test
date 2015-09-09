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


	float modelBoundSize;

	//given value when class is init
	public int index;
	public int branchLength;
	public Vector3 direction;
	public bool isCluster;



	//inherit value from parent
	float growingTimeGap;
	Organism myOrganismClass;




	//self use
	public List<ObjectData> objectsData;
	GameObject parentGameObject;

	void Start ()
	{
		parentGameObject = gameObject.transform.parent.gameObject;
		myOrganismClass = parentGameObject.GetComponent<Organism> ();
		objectsData = new List<ObjectData> ();
		growingTimeGap = myOrganismClass.minimalGeneratingTimeGap;

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
		Organism myOrganismClass = parentGameObject.GetComponent<Organism> ();



		Vector3 previousObjectPos = myOrganismClass.baseObject.transform.position + Vector3.up * 2; //default pos is branch's base position
		if (objectsData.Count > 0) {
//			previousObjectPos = objectsData [objectsData.Count - 1].myGameObject.GetComponent<MeshRenderer> ().bounds.center;
			previousObjectPos = objectsData [objectsData.Count - 1].myGameObject.transform.position;
		}
		Vector3 newPosition;
		if(isCluster && myOrganismClass.objectSum < branchLength){
			Debug.Log("is cluster!");
			float randomRange = modelBoundSize;
			Vector3 randomOffset = new Vector3(Random.Range(-randomRange,randomRange),
			                                   0,
			                                   Random.Range(-randomRange,randomRange));
			newPosition = gameObject.transform.position + randomOffset + Vector3.up/2 * myOrganismClass.objectDropingDistance + new Vector3(0,previousObjectPos.y,0);
		}else{
			newPosition = previousObjectPos + Vector3.up * myOrganismClass.objectDropingDistance + direction;

		}


		GameObject newObject = (GameObject)Instantiate (Resources.Load (myOrganismClass.modelName), newPosition, Quaternion.identity);
		modelBoundSize = newObject.GetComponent<MeshRenderer> ().bounds.extents.magnitude;
		StickyStickStuckPackage.StickyStickStuck newObjectSSS = newObject.AddComponent<StickyStickStuckPackage.StickyStickStuck> ();
		Rigidbody newObjectRigidBody = newObject.GetComponent<Rigidbody> ();
		newObject.GetComponent<MeshRenderer> ().enabled = false;
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

		myOrganismClass.objectSum++;



	}


}
