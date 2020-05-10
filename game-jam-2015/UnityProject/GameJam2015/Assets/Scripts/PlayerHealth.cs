using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{	
	void Awake ()
	{
	}


	void OnCollisionEnter2D (Collision2D col)
	{
		// If the colliding gameobject is an Enemy...
		if(col.gameObject.tag == "Enemy")
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
