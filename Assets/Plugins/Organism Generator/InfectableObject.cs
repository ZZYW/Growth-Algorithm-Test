using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class InfectableObject : MonoBehaviour {



	public GrowthAlgorithm newGrowthAlgo;

	private string myModelName;
	private bool rooted;
	private GrowthAlgorithm originalGrowthAlgo;
	private GameObject organismGameObject;



	
	// Use this for initialization
	void Start () {
		myModelName = gameObject.name;
		newGrowthAlgo = GetRandomEnum<GrowthAlgorithm>();
	}
	
	// Update is called once per frame
	void Update () {

	}


	void OnCollisionEnter (Collision c)
	{
		if(c.gameObject.GetComponent<OrganismObject>() != null && !rooted){
			organismGameObject = c.gameObject.transform.parent.transform.parent.gameObject;
			organismGameObject.GetComponent<Organism>().changeModel(myModelName);
			rooted = true;
		}
	}

	static T GetRandomEnum<T>()
	{
		System.Array A = System.Enum.GetValues(typeof(T));
		T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
		return V;
	}

}
