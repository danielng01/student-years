using UnityEngine;
using System.Collections;

public class EnemyMutant : MonoBehaviour 
{
	public float moveSpeed = 50f;		// The speed the enemy moves at.
	private SpriteRenderer ren;			// Reference to the sprite renderer.
	private Transform frontCheck;		// Reference to the position of the gameobject used for checking if something is in front.

	public bool antigravity;


	 void Awake()
	{
		// Setting up the references.
		ren = transform.Find("body").GetComponent<SpriteRenderer>();
		frontCheck = transform.Find("frontCheck").transform;
	
		rigidbody2D.gravityScale =  antigravity ? -1 : 1;
		Vector3 enemyScale = transform.localScale;
		enemyScale.y *= antigravity ? -1 : 1;
		transform.localScale = enemyScale;

	}
	
	 void FixedUpdate ()
	{
		// Create an array of all the colliders in front of the enemy.
		Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position);
		
		// Check each of the colliders.
		foreach(Collider2D c in frontHits)
		{
			// If any of the colliders is an Obstacle...
			if(c.tag == "Obstacle")
			{
				// ... Flip the enemy and stop checking the other colliders.
				Flip ();
				break;
			}
		}
		
		// Set the enemy's velocity to moveSpeed in the x direction.
		rigidbody2D.velocity = new Vector2(transform.localScale.x * moveSpeed, rigidbody2D.velocity.y);	

	}

	public void Flip()
	{
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}
}
