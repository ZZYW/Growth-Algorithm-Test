using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Organism : MonoBehaviour
{

	public enum GrowthAlgorithm
	{
		Balanced,
		RoundCluster,
		RightLeaning,
		LeftLeaning,
		StraightUp
	}

	public enum ModelNames
	{
		Corgi,
		OfficeChair
	}


	public ModelNames model;
	public GrowthAlgorithm growthAlgorithmPresets;
	[HideInInspector]
	public float
		objectDropingDistance = 2.0f;
	[HideInInspector]
	public GameObject
		baseObject;
	public string modelName;
	
	//organism shape
	[Range(1,5)]
	public int
		branchNumber = 2;
	[Range(1,100)]
	public int
		mainTrunkLength = 4;
	public int branchLenghMin = 3;
	public int branchLenghMax = 8;

	[HideInInspector]
	public List<GameObject>
		branches;

	//object
	[Range(0.1f, 1.0f)]
	public float
		growingSpeed = 0.3f;
	[Range(0.0f, 0.5f)]
	public float
		newBornScale = 0.2f;
	[Range(1.0f,20.0f)]
	public float
		minimalGeneratingTimeGap = 1.0f;

	public int objectSum;

	void Start ()
	{
		updateModel ();
		branches = new List<GameObject> ();
		addBaseObject ();

		switch (growthAlgorithmPresets) {
		case GrowthAlgorithm.StraightUp:
			//nothing fancy, main trunk growing upward
			GameObject newbranch = addBranch (baseObject.transform.position, Vector3.up, mainTrunkLength);
			break;
		case GrowthAlgorithm.RoundCluster:
			//one branch with a random direction
			GameObject newborn =  addBranch(baseObject.transform.position, new Vector3(0,0,0), mainTrunkLength);
			newborn.GetComponent<OrganismBranch>().isCluster = true;
			break;
		case GrowthAlgorithm.LeftLeaning:
			//trunk with a left growing direction
			//branch at early position, new branch with a slightly smaller left direction 
			//branch at early position based on the second branch, new branch with a slight smaller left direction than the 2rd branch
			addBranch(baseObject.transform.position, Vector3.left/4,mainTrunkLength);
			break;
		case GrowthAlgorithm.RightLeaning:
			//same as the leftleaning but with right direction
			addBranch(baseObject.transform.position, Vector3.right/4,mainTrunkLength);
			break;
		case GrowthAlgorithm.Balanced:
			//main trunk grows upward, branch out base on the same object in trunk and each growing towards opposite direction, with similar length
			addBranch(baseObject.transform.position, new Vector3(0,0,0),(mainTrunkLength));
			break;
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
		updateModel ();
	}

	public GameObject addBranch (Vector3 _basePosition, Vector3 _dir, int length)
	{
		GameObject newBranchGameobject = new GameObject ();
		newBranchGameobject.name = "Branch";
		newBranchGameobject.transform.position = _basePosition;
		OrganismBranch newOB =	newBranchGameobject.AddComponent<OrganismBranch> ();
		newOB.direction = _dir;
		newBranchGameobject.transform.parent = gameObject.transform;
		newOB.index = branches.Count; //0,1,2,3....
		newOB.branchLength = length;
		branches.Add(newBranchGameobject);
		return newBranchGameobject;
	}

	public void changeModel (string _modelName)
	{
		modelName = _modelName;
	}

	private void addBaseObject ()
	{
		baseObject = (GameObject)Instantiate (Resources.Load (modelName));
		baseObject.transform.position = gameObject.transform.position;//reset base object's position as Organism's position
		baseObject.transform.parent = gameObject.transform;
		baseObject.name = "Base";
		baseObject.AddComponent<StickyStickStuckPackage.StickyStickStuck> ();
		Rigidbody rigid = baseObject.GetComponent<Rigidbody> ();
		rigid.isKinematic = true;
		rigid.constraints = RigidbodyConstraints.FreezeRotation;//avoid falling down
		baseObject.transform.rotation = Quaternion.identity;//no rotation
	}

	private void updateModel ()
	{
		switch (model) {
		case ModelNames.Corgi:
			changeModel ("corgi_withcollider");
			break;
		case ModelNames.OfficeChair:
			changeModel ("officechair_collider");
			break;
		}
	}







	
}
