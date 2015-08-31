using UnityEngine;
using System.Collections;

public class Organism : MonoBehaviour {
	
	public bool growing = false;

	public bool add_a_branch = false;


	// Use this for initialization
	void Start () {
		addBranch(new Vector3(0,0,0));
	}
	
	// Update is called once per frame
	void Update () {

	}


	public void addBranch(Vector3 basePosition){
		GameObject newBranch = new GameObject();
		newBranch.name = "Branch";
		newBranch.transform.position = basePosition;
		newBranch.AddComponent<OrganismBranch>();
		newBranch.transform.parent = gameObject.transform;
	}

}
