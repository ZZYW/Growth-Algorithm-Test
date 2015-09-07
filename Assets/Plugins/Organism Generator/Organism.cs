using UnityEngine;
using System.Collections;

public class Organism : MonoBehaviour {

	public float objectDropingDistance = 1.5f;
	public GameObject baseObject;
	public string modelName;


	void Start () {
		modelName = "corgi_withcollider";
		baseObject = (GameObject)Instantiate(Resources.Load(modelName));
		baseObject.transform.position = gameObject.transform.position;
		baseObject.transform.parent = gameObject.transform;
		baseObject.name = "Base";
		baseObject.AddComponent<StickyStickStuckPackage.StickyStickStuck> ();
		baseObject.GetComponent<Renderer>().enabled = false;
		Rigidbody rigid = baseObject.GetComponent<Rigidbody>();
		rigid.isKinematic = true;
		rigid.constraints = RigidbodyConstraints.FreezeRotation;
		baseObject.transform.rotation = Quaternion.identity;
		addBranch(baseObject.transform.position, "Sprout");
	}
	
	// Update is called once per frame
	void Update (){
	} 

	
	public void addBranch(Vector3 basePosition, string type){
		GameObject newBranch = new GameObject();
		newBranch.name = "Branch" + " " + type;
		newBranch.transform.position = basePosition;

		switch (type)
		{
		case "Cluster":
			newBranch.AddComponent<OrganismBranchCluster>();
			break;
		case "Root":
			newBranch.AddComponent<OrganismBranchRoot>();
			break;
		case "Sprout":
			newBranch.AddComponent<OrganismBranch>();
			break;
		default:
			Debug.LogError("Didn't find any matched branch type.");
			break;
		}

		newBranch.transform.parent = gameObject.transform;
	}


//	public void addBranch(Vecto)



}
