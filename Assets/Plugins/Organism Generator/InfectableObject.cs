using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class InfectableObject : MonoBehaviour {



	public GrowthAlgorithm newGrowthAlgo;

	private bool rooted;
	private GrowthAlgorithm originalGrowthAlgo;
	private GameObject organismGameObject;


	[Range(4,40)]
	public int generatedBranchLength = 30;

	
	// Use this for initialization
	void Start () {
		newGrowthAlgo = GetRandomEnum<GrowthAlgorithm>();
	}

	// Update is called once per frame
	void Update () {

	}
	
	void OnCollisionEnter (Collision c)
	{
		if(c.gameObject.GetComponent<OrganismObject>() != null && !rooted){

			//put this object under Organism
			gameObject.transform.parent = c.gameObject.transform.parent.transform.parent;

			organismGameObject = c.gameObject.transform.parent.transform.parent.gameObject;

			foreach(GameObject b in organismGameObject.GetComponent<Organism>().allBranchGameObjects){
				b.GetComponent<OrganismBranch>().StopCoroutine("WaitAndGrow");
			}

			Organism organismClass = organismGameObject.GetComponent<Organism>();
			organismClass.ChangeModel(gameObject);


			switch (newGrowthAlgo) {
			case GrowthAlgorithm.StraightUp:
				GameObject newbranch = organismClass.AddBranch (gameObject.transform.position, Vector3.up, generatedBranchLength);			
				break;
			case GrowthAlgorithm.RoundCluster:
				GameObject newborn = organismClass.AddBranch (gameObject.transform.position, new Vector3 (0, 0, 0), generatedBranchLength);
				newborn.GetComponent<OrganismBranch> ().isCluster = true;
				break;
			case GrowthAlgorithm.LeftLeaning:
				organismClass.AddBranch (gameObject.transform.position, Vector3.left * 	organismClass.modelSize / 10, generatedBranchLength);
				break;
			case GrowthAlgorithm.RightLeaning:
				organismClass.AddBranch (gameObject.transform.position, Vector3.right * organismClass.modelSize / 10, generatedBranchLength);
				break;
			case GrowthAlgorithm.Balanced:
				organismClass.AddBranch (gameObject.transform.position, Vector3.up, generatedBranchLength);
				break;
			}
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
