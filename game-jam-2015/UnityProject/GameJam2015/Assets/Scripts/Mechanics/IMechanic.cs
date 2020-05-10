using UnityEngine;
using System.Collections;

public class IMechanic : MonoBehaviour {
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.

	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.

	virtual public void Start()
	{
	
	}

	virtual public void Update () {

	}

	virtual public void FixedUpdate () {
		
	}
}
