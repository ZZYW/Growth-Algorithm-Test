using UnityEngine;
using System.Collections;

public class OrganismObject : MonoBehaviour
{
	
	public int myIndex;
	public bool growingCompleted = false;
	Vector3 startScale = new Vector3 (0, 0, 0);
	float growingSpeed = 0.35f;
	Vector3 growingVelocity;
	Vector3 targetScale;
	string parentType;

	// Use this for initialization
	void Start ()
	{
		growingVelocity = new Vector3 (growingSpeed, growingSpeed, growingSpeed);
		gameObject.transform.localScale = startScale;
//		gameObject.transform.rotation = Random.rotation;
		float randomSize = Random.Range (0.8f, 1.2f);
		targetScale = new Vector3 (randomSize, randomSize, randomSize);
//		targetScale = Vector3.one;
		parentType = gameObject.transform.parent.name; 
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Invisible before it sticks to anything
		if (gameObject.GetComponent<FixedJoint> () != null) {
			gameObject.GetComponent<MeshRenderer> ().enabled = true;
		}

		//Growing Size
		if (gameObject.transform.localScale.magnitude < targetScale.magnitude) {
			Vector3 temp = gameObject.transform.localScale;
			temp += growingVelocity * Time.deltaTime;
			gameObject.transform.localScale = temp;
		} else {
			growingCompleted = true;
		}


		//When The Object is All Set
		if (growingCompleted && gameObject.GetComponent<FixedJoint> () != null) {
			gameObject.GetComponent<Rigidbody> ().useGravity = false;
		}


		if(growingCompleted && gameObject.GetComponent<FixedJoint>() == null){
			OrganismBranch parentScript = gameObject.transform.parent.GetComponent<OrganismBranch>();
			parentScript.objectsData.RemoveAt(parentScript.objectsData.Count-1);
			Destroy(gameObject);
		}


	}

	public void generateBranchBasedOnMe (string type)
	{
		gameObject.transform.parent.transform.parent.GetComponent<Organism> ().addBranch (gameObject.transform.position, type);
	}


}