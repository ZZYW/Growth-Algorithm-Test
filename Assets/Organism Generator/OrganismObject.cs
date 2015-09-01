using UnityEngine;
using System.Collections;

public class OrganismObject : MonoBehaviour {

	int myIndex;

	public int MyIndex 
	{
		get { return myIndex; }
		set { myIndex = value; }
	}

	public bool growingCompleted = false;
	Vector3 startScale = new Vector3(0,0,0);
	Vector3 acre = new Vector3(0.2f, 0.2f, 0.2f);
	Vector3 targetScale = new Vector3(1,1,1);

	// Use this for initialization
	void Start () {
		gameObject.transform.localScale = startScale;
		gameObject.transform.rotation = Random.rotation;
	}
	
	// Update is called once per frame
	void Update () {

		if(gameObject.transform.localScale.magnitude < targetScale.magnitude){
			Vector3 temp = gameObject.transform.localScale;
			temp += acre * Time.deltaTime;
			gameObject.transform.localScale = temp;
		}else{
			growingCompleted = true;
		}
	}


}