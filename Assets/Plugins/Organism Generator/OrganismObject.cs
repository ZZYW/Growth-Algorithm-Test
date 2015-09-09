using UnityEngine;
using System.Collections;

public class OrganismObject : MonoBehaviour
{

	public int myIndex{ get; set; }

	public bool growingCompleted { get; set; }


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
		Organism organismScript = gameObject.transform.parent.transform.parent.GetComponent<Organism> ();
		growingSpeed = organismScript.growingSpeed;
		startScale = new Vector3 (organismScript.newBornScale, organismScript.newBornScale, organismScript.newBornScale);


		gameObject.tag = "organismObject";
		growingVelocity = new Vector3 (growingSpeed, growingSpeed, growingSpeed);
		gameObject.transform.localScale = startScale;
		gameObject.transform.rotation = Random.rotation;
		float randomSize = Random.Range (0.7f, 1.0f);
		targetScale = new Vector3 (randomSize, randomSize, randomSize);
	}
	
	void Update ()
	{
		//Invisible before it sticks to anything
		if (gameObject.GetComponent<FixedJoint> () != null) {
			foreach (FixedJoint fj in gameObject.GetComponents<FixedJoint>()) {
				fj.enablePreprocessing = false;
			}
			gameObject.GetComponent<MeshRenderer> ().enabled = true;
		}
	
		//Growing Size
		if (gameObject.transform.localScale.magnitude < targetScale.magnitude && !growingCompleted) {
			Vector3 temp = gameObject.transform.localScale;
			temp += growingVelocity * Time.deltaTime;
			gameObject.transform.localScale = temp;

		} else {
			growingCompleted = true;
			gameObject.GetComponent<StickyStickStuckPackage.StickyStickStuck> ().infectionProperties.affectInfected = false;
		
		}


		//When The Object is All Set
		if (growingCompleted && gameObject.GetComponent<FixedJoint> () != null && !gravityChanged) {
			gameObject.GetComponent<Rigidbody> ().useGravity = false;
			gravityChanged = true;
		}


		if (growingCompleted && gameObject.GetComponent<FixedJoint> () == null) {
			OrganismBranch parentScript = gameObject.transform.parent.GetComponent<OrganismBranch> ();
			parentScript.objectsData.RemoveAt (parentScript.objectsData.Count - 1);
			Destroy (gameObject);
		}


	}

	void OnCollisionEnter (Collision col)
	{
		if (gameObject.GetComponent<FixedJoint> () == null) {
			if (col.gameObject.tag != "organismObject" && myIndex > 0 ) {
				OrganismBranch parentScript = gameObject.transform.parent.GetComponent<OrganismBranch> ();
				parentScript.objectsData.RemoveAt (parentScript.objectsData.Count - 1);
				Destroy (gameObject);
			}
		}
	}


}