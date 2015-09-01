using UnityEngine;
using System.Collections;

public class Male : Human {
	
	public override float getRate(){
		return 3.3f;
	}

	void Start(){
		height = 10.0f;
		weight = 23.0f;
		Debug.Log( getRate()  + "{}{}");
	}




}
