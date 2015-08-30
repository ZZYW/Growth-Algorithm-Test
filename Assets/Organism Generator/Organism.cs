using UnityEngine;
using System.Collections;

public class Organism : MonoBehaviour {
	
	public bool growing = false;

	public bool add_a_branch = false;


	// Use this for initialization
	void Start () {
		addBranch();
	}
	
	// Update is called once per frame
	void Update () {
		if(add_a_branch){
			addBranch();
			add_a_branch = false;
		}
	}


	public void addBranch(){
		GameObject newBranch = new GameObject();
		newBranch.name = "Branch";
		newBranch.AddComponent<OrganismBranch>();
		newBranch.transform.parent = gameObject.transform;
	}

}
