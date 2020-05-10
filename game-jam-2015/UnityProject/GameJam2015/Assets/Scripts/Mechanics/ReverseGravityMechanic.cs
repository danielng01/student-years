using UnityEngine;
using System.Collections;

public class ReverseGravityMechanic : IMechanic {

	[HideInInspector]
	public bool changeGravity = false;				// Condition for whether the player should jump.
	[HideInInspector]
	public bool antigravity = false;				// Condition for whether the player should jump.
	
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
//	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public AudioClip[] taunts;				// Array of clips for when the player taunts.
	public float tauntProbability = 50f;	// Chance of a taunt happening.
	public float tauntDelay = 1f;			// Delay for when the taunt should happen.
	
	
	private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
	private Animator anim;					// Reference to the player's animator component.

	private Rigidbody2D rocket;				// Prefab of the rocket.
	public float speed = 20f;				// The speed the rocket will fire at.
	
	
	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.

	override public void Start()
	{
		anim = GetComponent<Animator>();

		playerCtrl = transform.root.GetComponent<PlayerControl>();
		GameObject rocketObject = Resources.Load ("rocket", typeof(GameObject)) as GameObject;
		rocket = rocketObject.GetComponent<Rigidbody2D> ();
	}
	
	override public void Update ()
	{

		if (Input.GetKeyDown(KeyCode.Space)) {
			if(playerCtrl.mechanic.facingRight)
			{
				// ... instantiate the rocket facing right and set it's velocity to the right. 
				Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(speed, 0);
			}
			else
			{
				// Otherwise instantiate the rocket facing left and set it's velocity to the left.
				Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(-speed, 0);
			}
		}
	}
	
	override public void FixedUpdate () 
	{
			// Cache the horizontal input.
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis( "Vertical" );
			
			// The Speed animator parameter is set to the absolute value of the horizontal input.
			anim.SetFloat("Speed", Mathf.Abs(h));
			
			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
			if(h * rigidbody2D.velocity.x < maxSpeed)
				// ... add a force to the player.
				rigidbody2D.AddForce(Vector2.right * h * moveForce);
			// If the player's horizontal velocity is greater than the maxSpeed...
			if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
				// ... set the player's velocity to the maxSpeed in the x axis.
				rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
			
			if(h > 0 && !facingRight)
				Flip();
			else if(h < 0 && facingRight)
				Flip();
			
			//Getting input up/down and changing gravity
			if (v > 0 && !antigravity)
				ChangeGravity ();
			
			if (v < 0 && antigravity)
				ChangeGravity ();
			
	}
	void ChangeGravity()
	{
//		int i = Random.Range(0, jumpClips.Length);
//		AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);
		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;
		rigidbody2D.gravityScale *= -1;
		antigravity = !antigravity;
	}
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	
	public IEnumerator Taunt()
	{
		// Check the random chance of taunting.
		float tauntChance = Random.Range(0f, 100f);
		if(tauntChance > tauntProbability)
		{
			// Wait for tauntDelay number of seconds.
			yield return new WaitForSeconds(tauntDelay);
			
			// If there is no clip currently playing.
			if(!audio.isPlaying)
			{
				// Choose a random, but different taunt.
				tauntIndex = TauntRandom();
				
				// Play the new taunt.
				audio.clip = taunts[tauntIndex];
				audio.Play();
			}
		}
	}
	
	
	int TauntRandom()
	{
		// Choose a random index of the taunts array.
		int i = Random.Range(0, taunts.Length);
		
		// If it's the same as the previous taunt...
		if(i == tauntIndex)
			// ... try another random taunt.
			return TauntRandom();
		else
			// Otherwise return this index.
			return i;
	}

}
