using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GrowthAlgorithm
{
	Balanced,
	RoundCluster,
	RightLeaning,
	LeftLeaning,
	StraightUp
}

public class Organism : MonoBehaviour
{



	public enum ModelNames
	{
		Corgi,
		OfficeChair,
		GiantBone
	}


	public bool isSubOrganism;
	public GameObject ground;
	public ModelNames model;
	public GrowthAlgorithm growthAlgorithmPresets;
	[HideInInspector]
	public float
		objectDropingDistance = 1.0f;
	[HideInInspector]
	public GameObject
		baseObject;
	[HideInInspector]
	public string
		modelName;
	[HideInInspector]
	public float
		modelSizeCorrection;
	
	//organism shape

	[Range(1,100)]
	public int
		mainTrunkLength = 4;
	[HideInInspector]
	public List<GameObject>
		branches;

	//leaning related
	Vector3 trunkLeftLeanAngle;
	Vector3 trunkRightLeanAngle;
	float leanAngleDiminishRate = 1.4f;
	int leaningSecondBranchOutLocation = 4; //from the ?th object of the first branch


	//balance related
	int forkPosition = 3;
	List<Vector3> balanceBranchDirections;


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
	[HideInInspector]
	public int
		objectSum;
	[HideInInspector]
	public float
		modelSize;
	[HideInInspector]
	public string
		objectTagName = "organismObject";

	void Start ()
	{
		updateModel ();


		branches = new List<GameObject> ();

		if (!isSubOrganism) {
			addBaseObject ();
		}

		trunkLeftLeanAngle = Vector3.left * modelSize / 10;
		trunkRightLeanAngle = Vector3.right * modelSize / 10;

		balanceBranchDirections = new List<Vector3> ();
		balanceBranchDirections.Add (new Vector3 (1, 0, 1));
		balanceBranchDirections.Add (new Vector3 (-1, 0, 1));
		balanceBranchDirections.Add (new Vector3 (1, 0, -1));

		switch (growthAlgorithmPresets) {
		case GrowthAlgorithm.StraightUp:
			//nothing fancy, main trunk growing upward
			GameObject newbranch = addBranch (baseObject.transform.position, Vector3.up, mainTrunkLength);
			break;
		case GrowthAlgorithm.RoundCluster:
			//one branch with a random direction
			GameObject newborn = addBranch (baseObject.transform.position, new Vector3 (0, 0, 0), mainTrunkLength);
			newborn.GetComponent<OrganismBranch> ().isCluster = true;
			break;
		case GrowthAlgorithm.LeftLeaning:
			//trunk with a left growing direction
			//branch at early position, new branch with a slightly smaller left direction 
			//branch at early position based on the second branch, new branch with a slight smaller left direction than the 2rd branch
			addBranch (baseObject.transform.position, Vector3.left * modelSize / 10, mainTrunkLength);
			break;
		case GrowthAlgorithm.RightLeaning:
			//same as the leftleaning but with right direction
			addBranch (baseObject.transform.position, Vector3.right * modelSize / 10, mainTrunkLength);
			break;
		case GrowthAlgorithm.Balanced:
			//main trunk grows upward, branch out base on the same object in trunk and each growing towards opposite direction, with similar length
			addBranch (baseObject.transform.position, Vector3.up, forkPosition + 2);
			break;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Not allow to change model in running time manually.
//		updateModel ();

		switch (growthAlgorithmPresets) {
		case GrowthAlgorithm.StraightUp:
			break;
		case GrowthAlgorithm.RoundCluster:
			break;
		case GrowthAlgorithm.LeftLeaning:
			if (branches.Count == 1) {
				if (branches [0].GetComponent<OrganismBranch> ().objectsData.Count > leaningSecondBranchOutLocation) {
					addBranch (branches [0].GetComponent<OrganismBranch> ().objectsData [leaningSecondBranchOutLocation - 1].myGameObject.transform.position,
					           trunkLeftLeanAngle / leanAngleDiminishRate, mainTrunkLength - 3);
				}
			}
			break;
		case GrowthAlgorithm.RightLeaning:
			if (branches.Count == 1) {
				if (branches [0].GetComponent<OrganismBranch> ().objectsData.Count > leaningSecondBranchOutLocation) {
					addBranch (branches [0].GetComponent<OrganismBranch> ().objectsData [leaningSecondBranchOutLocation - 1].myGameObject.transform.position,
					           trunkRightLeanAngle / leanAngleDiminishRate, mainTrunkLength - 3);

				}
			}
			break;
		case GrowthAlgorithm.Balanced:

			if (branches.Count == 1) {
				if (branches [0].GetComponent<OrganismBranch> ().objectsData.Count > forkPosition) {

					addBranch (branches [0].GetComponent<OrganismBranch> ().objectsData [forkPosition - 1].myGameObject.transform.position,
					           balanceBranchDirections [0] / leanAngleDiminishRate, mainTrunkLength);

					addBranch (branches [0].GetComponent<OrganismBranch> ().objectsData [forkPosition - 1].myGameObject.transform.position,
					           balanceBranchDirections [1] / leanAngleDiminishRate, mainTrunkLength);

					addBranch (branches [0].GetComponent<OrganismBranch> ().objectsData [forkPosition - 1].myGameObject.transform.position,
					           balanceBranchDirections [2] / leanAngleDiminishRate, mainTrunkLength);
				}
			}
			break;
		}



	}

	public GameObject addBranch (Vector3 _basePosition, Vector3 _dir, int length)
	{
		GameObject newBranchGameobject = new GameObject ();
		newBranchGameobject.name = "Branch";
		newBranchGameobject.transform.position = _basePosition;
		OrganismBranch newOB = newBranchGameobject.AddComponent<OrganismBranch> ();
		newOB.direction = _dir;
		newBranchGameobject.transform.parent = gameObject.transform;
		newOB.index = branches.Count; //0,1,2,3....
		newOB.branchLength = length;
		branches.Add (newBranchGameobject);
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
		case ModelNames.GiantBone:
			changeModel ("giantbone_withcollider");
			break;
		}

		modelSize = ModelSize ();
	}

	private float ModelSize ()
	{
		GameObject newObject = (GameObject)Resources.Load (modelName);
		float size = newObject.GetComponent<MeshRenderer> ().bounds.size.magnitude;
		modelSizeCorrection = 4.45f / size;
		return size;
	}

}
