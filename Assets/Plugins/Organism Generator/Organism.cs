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

	public GameObject ground;

	public GameObject brokenOffContainer;
	public GrowthAlgorithm growthAlgorithmPresets;

	public GameObject[] models;
	public int useModel;
	public bool randomSelectModel;


	[Range(3.5f,5.5f)]
	public float
		modelBoundSize = 5.0f;
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
	public GameObject
		modelGameObject;
	[HideInInspector]
	public float
		modelSizeCorrection;

	

	//organism shape
	[Range(1,100)]
	public int
		mainTrunkLength = 4;
	[HideInInspector]
	public List<GameObject>
		allBranchGameObjects;

	[System.Serializable]
	public class AlgorithmProperties {
		[System.Serializable]
		public class StraightUpProperties {
			public int trunkLength = 24;
		}
		public StraightUpProperties strightUpProperties; 

		[System.Serializable]
		public class BalanceProperties {
			public int trunkLength = 7;
			public int forkStartLocation = 3; //Notice:  has to be smaller than trunkLength
			[HideInInspector]
			public Vector3 b1Direction = new Vector3(0.6f,0.5f,0.6f);
			[HideInInspector]
			public Vector3 b2Direction = new Vector3(-0.6f,0.5f,0.6f);
			[HideInInspector]
			public Vector3 b3Direction = new Vector3(0.6f,0.5f,-0.6f);
			[HideInInspector]
			public float leanAngleDiminishRate = 1.4f;
			public int branchLength = 15;
		}
		public BalanceProperties balanceProperties;
		[System.Serializable]
		public class LeftLeaningProperties {
			public int trunkLength = 30;
			[HideInInspector]
			public float leanAngleDiminishRate = 1.4f;
			public int secondBranchOutLocation = 4;
			public int secondBranchLength = 20;
			[HideInInspector]
			public Vector3 leanAngle;
		}
		public LeftLeaningProperties leftLeaningProperties;

		[System.Serializable]
		public class RightLeaningProperties{
			public int trunkLength = 30;
			[HideInInspector]
			public float leanAngleDiminishRate = 1.4f;
			public int secondBranchOutLocation = 4;
			public int secondBranchLength = 20;
			[HideInInspector]
			public Vector3 leanAngle;
		}
		public RightLeaningProperties rightLeaningProperties;

		[System.Serializable]
		public class ClusterProperties{
			public int trunkLength;
		}
		public ClusterProperties clusterProperties;
	}

	public AlgorithmProperties algorithmProperties;

	//leaning related
//	Vector3 trunkLeftLeanAngle;
//	Vector3 trunkRightLeanAngle;
//	float leanAngleDiminishRate = 1.4f;
//	int leaningSecondBranchOutLocation = 4; //from the ?th object of the first branch


	//balance related
//	int forkPosition = 3;

	[Range(0.1f, 1.0f)]
	public float
		growingSpeed = 0.3f;
	[Range(0.0f, 0.5f)]
	public float
		newBornScale = 0.2f;
	[Range(1.0f,20.0f)]
	public float
		generatingTimeGap = 1.0f;
	[HideInInspector]
	public int
		objectSum;
	[HideInInspector]
	public float
		modelSize;
	[HideInInspector]
	public string
		objectTagName = "organismObject";


	public ConfigurableJointMotion xmotion= ConfigurableJointMotion.Limited;
	public ConfigurableJointMotion ymotion = ConfigurableJointMotion.Limited;
	public ConfigurableJointMotion zmotion = ConfigurableJointMotion.Limited;
	public ConfigurableJointMotion xmotionAngular = ConfigurableJointMotion.Locked;
	public ConfigurableJointMotion ymotionAngular = ConfigurableJointMotion.Locked;
	public ConfigurableJointMotion zmotionAngular = ConfigurableJointMotion.Locked;
	public float jointLimit = 0.6f;

	
	void Awake(){
		algorithmProperties = new AlgorithmProperties();
		algorithmProperties.strightUpProperties = new AlgorithmProperties.StraightUpProperties();
		algorithmProperties.balanceProperties = new AlgorithmProperties.BalanceProperties();
		algorithmProperties.clusterProperties = new AlgorithmProperties.ClusterProperties();
		algorithmProperties.leftLeaningProperties = new AlgorithmProperties.LeftLeaningProperties();
		algorithmProperties.rightLeaningProperties = new AlgorithmProperties.RightLeaningProperties();
	}

	void Start ()
	{
		if(useModel>models.Length-1){
			useModel = models.Length-1;
		}else if(useModel<0){
			useModel = 0;
		}

		CheckModelList ();
		AddBaseObject ();

		allBranchGameObjects = new List<GameObject> ();
		algorithmProperties.leftLeaningProperties.leanAngle = Vector3.left * modelSize / 10;
		algorithmProperties.rightLeaningProperties.leanAngle = Vector3.right * modelSize / 10;



		switch (growthAlgorithmPresets) {
		case GrowthAlgorithm.StraightUp:
			//nothing fancy, main trunk growing upward
			AddBranch (baseObject.transform.position, Vector3.up, algorithmProperties.strightUpProperties.trunkLength);
			break;
		case GrowthAlgorithm.RoundCluster:
			//one branch with a random direction
			GameObject newborn = AddBranch (baseObject.transform.position, new Vector3 (0, 0, 0), algorithmProperties.clusterProperties.trunkLength);
			newborn.GetComponent<OrganismBranch> ().isCluster = true;
			break;
		case GrowthAlgorithm.LeftLeaning:
			//trunk with a left growing direction
			//branch at early position, new branch with a slightly smaller left direction 
			//branch at early position based on the second branch, new branch with a slight smaller left direction than the 2rd branch
			AddBranch (baseObject.transform.position, Vector3.left * modelSize / 10, algorithmProperties.leftLeaningProperties.trunkLength);
			break;
		case GrowthAlgorithm.RightLeaning:
			//same as the leftleaning but with right direction
			AddBranch (baseObject.transform.position, Vector3.right * modelSize / 10, algorithmProperties.rightLeaningProperties.trunkLength);
			break;
		case GrowthAlgorithm.Balanced:
			//main trunk grows upward, branch out base on the same object in trunk and each growing towards opposite direction, with similar length
			AddBranch (baseObject.transform.position, Vector3.up, algorithmProperties.balanceProperties.trunkLength);
			break;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{

		switch (growthAlgorithmPresets) {
		case GrowthAlgorithm.StraightUp:
			break;
		case GrowthAlgorithm.RoundCluster:
			break;
		case GrowthAlgorithm.LeftLeaning:
			if (allBranchGameObjects.Count == 1) {
				if (allBranchGameObjects [0].GetComponent<OrganismBranch> ().objectsData.Count > algorithmProperties.leftLeaningProperties.secondBranchOutLocation) {
					AddBranch (allBranchGameObjects [0].GetComponent<OrganismBranch> ().objectsData [algorithmProperties.leftLeaningProperties.secondBranchOutLocation - 1].myGameObject.transform.position,
					           algorithmProperties.leftLeaningProperties.leanAngle / algorithmProperties.leftLeaningProperties.leanAngleDiminishRate, algorithmProperties.leftLeaningProperties.secondBranchLength);
				}
			}
			break;

		case GrowthAlgorithm.RightLeaning:
			if (allBranchGameObjects.Count == 1) {
				if (allBranchGameObjects [0].GetComponent<OrganismBranch> ().objectsData.Count > algorithmProperties.rightLeaningProperties.secondBranchOutLocation) {
					AddBranch (allBranchGameObjects [0].GetComponent<OrganismBranch> ().objectsData [algorithmProperties.rightLeaningProperties.secondBranchOutLocation - 1].myGameObject.transform.position,
					           algorithmProperties.rightLeaningProperties.leanAngle / algorithmProperties.rightLeaningProperties.leanAngleDiminishRate, algorithmProperties.rightLeaningProperties.secondBranchLength);
				}
			}
			break;

		case GrowthAlgorithm.Balanced:
			if (allBranchGameObjects.Count == 1) {
				if (allBranchGameObjects [0].GetComponent<OrganismBranch> ().objectsData.Count > algorithmProperties.balanceProperties.forkStartLocation) {

					AddBranch (allBranchGameObjects [0].GetComponent<OrganismBranch> ().objectsData [algorithmProperties.balanceProperties.forkStartLocation - 1].myGameObject.transform.position,
					           algorithmProperties.balanceProperties.b1Direction / algorithmProperties.balanceProperties.leanAngleDiminishRate, algorithmProperties.balanceProperties.branchLength);

					AddBranch (allBranchGameObjects [0].GetComponent<OrganismBranch> ().objectsData [algorithmProperties.balanceProperties.forkStartLocation - 1].myGameObject.transform.position,
					           algorithmProperties.balanceProperties.b2Direction / algorithmProperties.balanceProperties.leanAngleDiminishRate, algorithmProperties.balanceProperties.branchLength);

					AddBranch (allBranchGameObjects [0].GetComponent<OrganismBranch> ().objectsData [algorithmProperties.balanceProperties.forkStartLocation - 1].myGameObject.transform.position,
					           algorithmProperties.balanceProperties.b3Direction / algorithmProperties.balanceProperties.leanAngleDiminishRate, algorithmProperties.balanceProperties.branchLength);
				}
			}
			break;
		}
	}

	public GameObject AddBranch (Vector3 _basePosition, Vector3 _dir, int length)
	{
		GameObject newBranchGameobject = new GameObject ();
		newBranchGameobject.name = "Branch";
		newBranchGameobject.transform.position = _basePosition;
		OrganismBranch newOB = newBranchGameobject.AddComponent<OrganismBranch> ();
		newOB.direction = _dir;
		newBranchGameobject.transform.parent = gameObject.transform;
		newOB.index = allBranchGameObjects.Count; //0,1,2,3....
		newOB.branchLength = length;
		allBranchGameObjects.Add (newBranchGameobject);
		return newBranchGameobject;
	}

	public void ChangeModel (GameObject m)
	{
		modelGameObject = m;
		modelSize = ModelSize ();
	}

	private void AddBaseObject ()
	{
		baseObject = (GameObject)Instantiate (modelGameObject);
		baseObject.transform.position = gameObject.transform.position;//reset base object's position as Organism's position
		baseObject.transform.parent = gameObject.transform;
		baseObject.name = "Base";
		baseObject.AddComponent<StickyStickStuckPackage.StickyStickStuck> ();
		Rigidbody rigid = baseObject.GetComponent<Rigidbody> ();
		rigid.isKinematic = true;
		rigid.constraints = RigidbodyConstraints.FreezeRotation;//avoid falling down
		baseObject.transform.rotation = Quaternion.identity;//no rotation
	}

	private void CheckModelList ()
	{
		if(!randomSelectModel){
			ChangeModel(models[useModel]);
		}else{
			int randomIndex = Mathf.FloorToInt(Random.Range(0,models.Length));
			ChangeModel(models[randomIndex]);
		}
	}

	private float ModelSize ()
	{
		float size = modelGameObject.GetComponent<MeshRenderer> ().bounds.size.magnitude;
	
		modelSizeCorrection = modelBoundSize / size;
		return size;
	}

}
