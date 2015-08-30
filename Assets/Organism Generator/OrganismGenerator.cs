using UnityEngine;
using System.Collections;

public class OrganismGenerator : MonoBehaviour {

	
	// Use this for initialization
	void Start () {
		GameObject newOrganism = new GameObject();
		newOrganism.AddComponent<Organism>();
		newOrganism.name = "Organism";


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
