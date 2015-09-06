using UnityEngine;
using System.Collections;

public class OrganismObject : MonoBehaviour
{
	
	public int myIndex;
	public bool growingCompleted = false;
	Vector3 startScale = new Vector3 (0, 0, 0);
	float acrePare = 0.5f;
	Vector3 acre;
	Vector3 targetScale;
	public Vector3 targetLocation;
//	public Vector3 previousObjectLocation;
	public float shiftSpeed = 0.4f;
	string parentType;
	bool shiftCompleted = false;
	
	// Use this for initialization
	void Start ()
	{
		acre = new Vector3 (acrePare, acrePare, acrePare);
		gameObject.transform.localScale = startScale;

		if(myIndex==0){
			gameObject.transform.rotation = Quaternion.identity;
		}else{
			gameObject.transform.rotation = Random.rotation;
		}

		float randomSize = Random.Range (0.8f, 1.2f);
		targetScale = new Vector3 (randomSize, randomSize, randomSize);
		parentType = gameObject.transform.parent.name; 
	}

	static string vec2String(Vector3 v)
	{
		string ret="("+v.x+", "+v.y+", "+v.z+")";
		return ret;
	}
	// Update is called once per frame
	void Update ()
	{

		if(gameObject.GetComponent<FixedJoint>()!=null || myIndex == 0){
			gameObject.GetComponent<MeshRenderer>().enabled = true;
		}
		if (gameObject.transform.localScale.magnitude < targetScale.magnitude  ) {
			Vector3 temp = gameObject.transform.localScale;
			temp += acre * Time.deltaTime;
			gameObject.transform.localScale = temp;
		} else {
			growingCompleted = true;

//			if(myIndex!=0)gameObject.GetComponent<Rigidbody>().isKinematic = false;
//			gameObject.GetComponent<StickyStickStuckPackage 
//			gameObject.GetComponent<Rigidbody>().mass = 0.1f;
//			StickyStickStuckPackage.StickyStickStuck stickyStickStuck = gameObject.GetComponent<StickyStickStuckPackage.StickyStickStuck> ();
//			stickyStickStuck.stickProperties.stickNonRigidbodys = false;
//			stickyStickStuck.enabled = true;
		}

		if (targetLocation != gameObject.transform.position) {
			if (!shiftCompleted) {

				Vector3 temp = gameObject.transform.position;
				Vector3 direction = Vector3.Normalize(targetLocation - temp);
				Vector3 moveSpeed = direction * (shiftSpeed * Time.deltaTime);
				moveSpeed *= 2.0f;//10.0f;
				gameObject.transform.position = temp + moveSpeed;
			}
			else
			{
				//Debug.Log ("shift completed!");
			}

		} else {
			//Debug.Log ("Completing shift?!");
			shiftCompleted = true;
		}


	}

	public void generateBranchBasedOnMe (string type)
	{
		gameObject.transform.parent.transform.parent.GetComponent<Organism> ().addBranch (gameObject.transform.position, type);
	}


}