using UnityEngine;
using System.Collections;

public class Human : MonoBehaviour {


	public float height = 173.0f;
	public float weight = 45.0f;


	public virtual float getRate(){
		return height/weight;
	}


	
}
