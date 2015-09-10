using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class InfectableObject : MonoBehaviour {



	public GrowthAlgorithm newGrowthAlgo;

	private string myModelName;
	private bool rooted;
	private GrowthAlgorithm originalGrowthAlgo;
	private GameObject organismGameObject;


	[Range(4,40)]
	public int generatedBranchLength = 30;

	
	// Use this for initialization
	void Start () {
		myModelName = gameObject.name;
//		newGrowthAlgo = GetRandomEnum<GrowthAlgorithm>();
		newGrowthAlgo = GrowthAlgorithm.StraightUp;
	}

	// Update is called once per frame
	void Update () {

	}


	void OnCollisionEnter (Collision c)
	{
		if(c.gameObject.GetComponent<OrganismObject>() != null && !rooted){

			gameObject.transform.parent = c.gameObject.transform.parent.transform.parent;
			organismGameObject = c.gameObject.transform.parent.transform.parent.gameObject;
			organismGameObject.GetComponent<Organism>().changeModel(myModelName);

			foreach(GameObject b in organismGameObject.GetComponent<Organism>().allBranchGameObjects){
				b.GetComponent<OrganismBranch>().StopCoroutine("WaitAndGrow");
			}

			Organism organismClass = organismGameObject.GetComponent<Organism>();
			organismClass.changeModel(myModelName);


			switch (newGrowthAlgo) {
			case GrowthAlgorithm.StraightUp:
				GameObject newbranch = organismClass.addBranch (gameObject.transform.position, Vector3.up, generatedBranchLength);			
				break;
			case GrowthAlgorithm.RoundCluster:
				GameObject newborn = organismClass.addBranch (gameObject.transform.position, new Vector3 (0, 0, 0), generatedBranchLength);
				newborn.GetComponent<OrganismBranch> ().isCluster = true;
				break;
			case GrowthAlgorithm.LeftLeaning:
				organismClass.addBranch (gameObject.transform.position, Vector3.left * 	organismClass.modelSize / 10, generatedBranchLength);
				break;
			case GrowthAlgorithm.RightLeaning:
				organismClass.addBranch (gameObject.transform.position, Vector3.right * organismClass.modelSize / 10, generatedBranchLength);
				break;
			case GrowthAlgorithm.Balanced:
				organismClass.addBranch (gameObject.transform.position, Vector3.up, generatedBranchLength);
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
