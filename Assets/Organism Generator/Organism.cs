using UnityEngine;
using System.Collections;

public class Organism : MonoBehaviour {
	
	public bool growing = false;
	
	// Use this for initialization
	void Start () {
		addBranch(new Vector3(0,0,0), "Sprout");
	}
	
	// Update is called once per frame
	void Update () {

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



}
