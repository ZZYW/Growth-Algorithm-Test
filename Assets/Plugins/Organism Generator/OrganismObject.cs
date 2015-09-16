using UnityEngine;
using System.Collections;

public class OrganismObject : MonoBehaviour
{

	public int myIndex{ get; set; }

	public bool growingCompleted { get; set; }
//	public bool hitSomething {get;set;}

	Organism myOrganismClass;
	OrganismBranch myOrganismBranchClass;

	//value set by Organism
	Vector3 startScale;
	float growingSpeed;
	Vector3 growingVelocity;
	Vector3 targetScale;
	bool gravityChanged = false;

	
	// Use this for initialization
	void Start ()
	{
		//get paremeter value from Organism
		myOrganismClass = gameObject.transform.parent.transform.parent.GetComponent<Organism> ();
		myOrganismBranchClass = gameObject.transform.parent.GetComponent<OrganismBranch> ();

		growingSpeed = myOrganismClass.growingSpeed;
		startScale = new Vector3 (myOrganismClass.newBornScale, myOrganismClass.newBornScale, myOrganismClass.newBornScale);

		gameObject.tag = myOrganismClass.objectTagName;
		growingVelocity = new Vector3 (growingSpeed, growingSpeed, growingSpeed);
		gameObject.transform.localScale = startScale;
		gameObject.transform.rotation = Random.rotation;


		float randomSize = Random.Range (0.7f, 1.4f) * myOrganismClass.modelSizeCorrection;
		targetScale = new Vector3 (randomSize, randomSize, randomSize);
	}
	
	void Update ()
	{

		//When it has already sticked to something.
		if (gameObject.GetComponent<Joint> () != null) {

//			gameObject.GetComponent<StickyStickStuckPackage.StickyStickStuck>().infectionProperties.affectInfected = false;

			gameObject.GetComponent<MeshRenderer> ().enabled = true;

			if (!gravityChanged) {
				gameObject.GetComponent<Rigidbody> ().useGravity = false;
				gravityChanged = true;
			}
		}

		if (gameObject.transform.localScale.magnitude < targetScale.magnitude && !growingCompleted) {
			Vector3 temp = gameObject.transform.localScale;
			temp += growingVelocity * Time.deltaTime;
			gameObject.transform.localScale = temp;
		} else {
			//When the size growing is completed.
			growingCompleted = true;
			gameObject.GetComponent<StickyStickStuckPackage.StickyStickStuck> ().infectionProperties.affectInfected = false;
		}

		if (gameObject.GetComponent<Joint> () != null) {
			if (gameObject.GetComponent<Joint> ().connectedBody == null) {
				myOrganismBranchClass.objectsData.RemoveAt (myOrganismBranchClass.objectsData.Count - 1);
				Destroy (gameObject);
			}
		}

	}

	void OnCollisionEnter (Collision col)
	{
		if (gameObject.GetComponent<Joint> () == null) {
			//If it hits ground first, destroy it
			if (col.gameObject == myOrganismClass.ground && myIndex > 0) {
				myOrganismBranchClass.objectsData.RemoveAt (myOrganismBranchClass.objectsData.Count - 1);
				myOrganismBranchClass.objectMissedNumber++;
				Destroy (gameObject);
			}
		} else {
			myOrganismBranchClass.objectMissedNumber = 0;
		}
	}


}