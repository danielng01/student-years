using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	//private Animator anim;					// Reference to the Animator component.


	void Awake()
	{
		// Setting up the references.
	//	anim = transform.root.gameObject.GetComponent<Animator>();

	}


	void Update ()
	{
		// If the fire button is pressed...
		if(Input.GetButtonDown("Fire1"))
		{
			// ... set the animator Shoot trigger parameter and play the audioclip.
			//anim.SetTrigger("Shoot");
//			audio.Play();

			// If the player is facing right...

		}
	}
}
