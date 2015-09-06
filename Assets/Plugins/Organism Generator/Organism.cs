using UnityEngine;
using System.Collections;

public class Organism : MonoBehaviour {
	
	public bool growing = false;
	public GameObject baseObject;

	void Start () {

		baseObject = (GameObject)Instantiate(Resources.Load("corgi_withcollider"));
		baseObject.transform.position = gameObject.transform.position;
		baseObject.transform.rotation = Quaternion.identity;
		Rigidbody rigid = baseObject.AddComponent<Rigidbody>();
		rigid.isKinematic = true;
		rigid.constraints = RigidbodyConstraints.FreezeAll;
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
			newBranch.AddComponent<OrganismBranchSprout>();
			break;
		default:
			Debug.LogError("Didn't find any matched branch type.");
			break;
		}

		newBranch.transform.parent = gameObject.transform;
	}


//	public void addBranch(Vecto)



}
