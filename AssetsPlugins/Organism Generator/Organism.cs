using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Organism : MonoBehaviour
{

	public float objectDropingDistance = 1.5f;
	public GameObject baseObject;
	public string modelName;
	
	//organism shape
	public int branchNumber = 2;
	public int mainTrunkLength = 4;
	public int branchLenghMin = 3;
	public int branchLenghMax = 8;

	public List<int> branchesLength;
	public List<int> branchoutPosition;
	public List<OrganismBranch> branches;
	public List<Vector3> growDirections;
	
	void Start ()
	{
		branches = new List<OrganismBranch>();
	
//		//TODO: parametrize grow directions!
//		growDirections.Add(Vector3.left);
//		growDirections.Add(Vector3.back);
//
//		for (int i=0; i<branchNumber; i++) {
//			branchesLength.Add ((int)Random.Range (branchLenghMin, branchLenghMax));
//			branchoutPosition.Add((int)Random.Range(0,branchesLength[branchesLength.Count-1]));
//		}

		addBaseObject ();
		addBranch (baseObject.transform.position, new Vector3(0,0,0), mainTrunkLength, 4, true);
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	public void addBranch (Vector3 basePosition, Vector3 growDirection, int _length, int _branchoutPosition, bool _hasSub)
	{
		GameObject newBranchGameobject = new GameObject ();
		newBranchGameobject.name = "Branch" + " " + branches.Count;
		newBranchGameobject.transform.position = basePosition;
		branches.Add (newBranchGameobject.AddComponent<OrganismBranch> ());
		branches [branches.Count - 1].branchLength = _length;
		branches [branches.Count - 1].direction = growDirection;
		branches [branches.Count - 1].branchOutPosition = _branchoutPosition;
		branches [branches.Count - 1].hasSubBranch = _hasSub;
		newBranchGameobject.transform.parent = gameObject.transform;

	}

	public void changeModel (string _modelName)
	{
		modelName = _modelName;
	}

	void addBaseObject ()
	{
		modelName = "officechair_collider";
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

	
}
