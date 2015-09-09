using UnityEngine;
using System.Collections;

public class OrganismObjectStabilizer : MonoBehaviour {


	[Range(1.0f,7.0f)]
	public float period = 5.0f;

	// Use this for initialization
	void Start () {
		StartCoroutine("cancelMomentum");
	}

	IEnumerator cancelMomentum ()
	{
		
		while(true){
			GameObject[] allOrganismObjects = GameObject.FindGameObjectsWithTag("organismObject");
			for(int i=0;i<allOrganismObjects.Length;i++){
				allOrganismObjects[i].GetComponent<Rigidbody>().isKinematic=true;
				allOrganismObjects[i].GetComponent<Rigidbody>().isKinematic=false;
			}
			yield return new WaitForSeconds (period);
		}
		
	}

}
