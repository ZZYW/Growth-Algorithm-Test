using UnityEngine;
using System.Collections;

public class DeKinematicizeAfterDuration : MonoBehaviour 
{
	public float delay = 2.0f;//TODO: auto calc this later on eventually!
	private float elapsed = 0.0f;
	private bool bTriggered=false;
	// Use this for initialization
	void Start () 
	{
	
	}

	void onTrigger()
	{
		Rigidbody rigid = this.GetComponent<Rigidbody>();
		if (rigid == null)
		{
			Debug.Log ("DeKinematicize object: null rigidbody!");
		}
		else
		{
			rigid.isKinematic = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!bTriggered)
		{
			elapsed += Time.deltaTime;
			if (elapsed >= delay)
			{
				bTriggered= true;
				onTrigger();
			}
		}
	}
}
