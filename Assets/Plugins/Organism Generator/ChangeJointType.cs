using UnityEngine;
using System.Collections;

public class ChangeJointType : MonoBehaviour
{


	public Organism myOrganismClass;


	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (myOrganismClass != null) {
			if (gameObject.GetComponent<FixedJoint> () != null) {
				FixedJoint fixedjoint = gameObject.GetComponent<FixedJoint> ();
				Rigidbody connectedbody = fixedjoint.connectedBody;
				Destroy (fixedjoint);
				ConfigurableJoint conjoint = gameObject.AddComponent<ConfigurableJoint> ();
				conjoint.connectedBody = connectedbody;
			
			
			
				//Configurable
				conjoint.xMotion = myOrganismClass.xmotion;
				conjoint.yMotion = myOrganismClass.ymotion;
				conjoint.zMotion = myOrganismClass.zmotion;
				conjoint.angularXMotion = myOrganismClass.xmotionAngular;
				conjoint.angularYMotion = myOrganismClass.ymotionAngular;
				conjoint.angularZMotion = myOrganismClass.zmotionAngular;
			
				SoftJointLimit softjointlimit = conjoint.linearLimit;
				softjointlimit.limit = myOrganismClass.jointLimit;
				//				softjointlimit.bounciness = 1.0f;
				conjoint.linearLimit = softjointlimit;
			
			
				SoftJointLimitSpring limitspring = conjoint.linearLimitSpring;
				limitspring.spring = 20.0f;
				conjoint.linearLimitSpring = limitspring;


//				//				conjoint.limi
//				SoftJointLimit limits = conjoint.linearLimit;
//				//				limits.
//				conjoint.linearLimit = limits;
			}
		}
	}
}
