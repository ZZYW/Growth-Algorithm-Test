using UnityEngine;
using System.Collections;

public class OrganismObject : MonoBehaviour
{

	public int myIndex;
	public bool growingCompleted = false;
	Vector3 startScale = new Vector3 (0, 0, 0);
	float scaleGrowingSpeed = 0.8f;
	Vector3 scaleGrowingVelocity;
	Vector3 targetScale;

	public Vector3 targetLocation;
	public Vector3 bornLocation;
	public GameObject previousGameObject;
	bool shiftingCompleted = false;


	// Use this for initialization
	void Start ()
	{
		scaleGrowingVelocity = new Vector3 (scaleGrowingSpeed, scaleGrowingSpeed, scaleGrowingSpeed);
		gameObject.transform.localScale = startScale;
		gameObject.transform.rotation = Random.rotation;
		float randomSize = Random.Range (1.2f, 1.5f);
		targetScale = new Vector3 (randomSize, randomSize, randomSize);


	

	}




	// Update is called once per frame
	void Update ()
	{

		if( Vector3.Distance(gameObject.transform.position,targetLocation) > 0.04f && !shiftingCompleted){
			float shiftSpeed = 1.0f;
			Vector3 dire = targetLocation - gameObject.transform.position;
			Vector3 vel = dire.normalized * shiftSpeed * Time.deltaTime;
			Vector3 temp = gameObject.transform.position + vel;
			gameObject.transform.position = temp;
		}else{
			shiftingCompleted = true;
			if(gameObject.GetComponent<FixedJoint>() == null){

				FixedJoint joint =  gameObject.AddComponent<FixedJoint>();
				joint.connectedBody = previousGameObject.GetComponent<Rigidbody>();
				gameObject.GetComponent<Rigidbody>().isKinematic = false;
				gameObject.GetComponent<Rigidbody>().useGravity = true;

				Collider[] colliders = gameObject.GetComponents<Collider>();
				for (int i=0; i<colliders.Length; i++) {
					colliders[i].isTrigger = false;
				}

				gameObject.GetComponent<Rigidbody>().useGravity = false;

			}


		}






		//Invisible before it sticks to anything
//		if (gameObject.GetComponent<FixedJoint> () != null) {
//			gameObject.GetComponent<MeshRenderer> ().enabled = true;
//		}

		//Growing Size
		if (gameObject.transform.localScale.magnitude < targetScale.magnitude) {
			Vector3 temp = gameObject.transform.localScale;
			temp += scaleGrowingVelocity * Time.deltaTime;
			gameObject.transform.localScale = temp;
		} else {
			growingCompleted = true;
		}

		//When The Object is All Set
//		if (growingCompleted && gameObject.GetComponent<FixedJoint> () != null) {
//			gameObject.GetComponent<Rigidbody> ().useGravity = false;
//		}


//		if (growingCompleted && gameObject.GetComponent<FixedJoint> () == null) {
//			OrganismBranch parentScript = gameObject.transform.parent.GetComponent<OrganismBranch> ();
//			parentScript.objectsData.RemoveAt (parentScript.objectsData.Count - 1);
//			Destroy (gameObject);
//		}


	}

//	public void generateBranchBasedOnMe (string type)
//	{
//		gameObject.transform.parent.transform.parent.GetComponent<Organism> ().addBranch (gameObject.transform.position);
//	}


}