using UnityEngine;
using System.Collections;

public class SizeChangeMechanic : IMechanic {
	bool isBig = false;
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
	private Animator anim;					// Reference to the player's animator component.
	private GameObject hero;
	private	float isBigTimer = 4f;
	private float isBigMaxTime = 2f;
	private float isBigCooldown = 2f;
	private float isSmallTimer = 4f;
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "EnemyLevel3") {
			if(isBig)
			{
				collision.gameObject.GetComponent<Enemy>().Death();
			}
			else
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		}

	}



	override public void Start()
	{
		hero =  GameObject.FindWithTag("Player");
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
	}
	
	override public void Update () 
	{
		//*********Movement*******//
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(hero.transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
		
		// If the jump button is pressed and the player is grounded then the player should jump.
		if(Input.GetKeyDown(KeyCode.UpArrow) && grounded)
			jump = true;



		/*******Size change transitions******/
		if(Input.GetKeyDown(KeyCode.Space)&&isBigCooldown<isSmallTimer&&!isBig) 
		{
			isBig=!isBig;
			if(facingRight)
			{
				transform.localScale += new Vector3(0.1f,0.1f,0); // character gets bigger
			}
			else
			{
				transform.localScale += new Vector3(-0.1f,0.1f,0); // character gets bigger
			}
			isBigTimer = 0;
		}
		if (isBigTimer > isBigMaxTime && isBig) 
		{
			isBig=!isBig;
			if(facingRight)
			{
				transform.localScale -= new Vector3(0.1f,0.1f,0); // character gets bigger
			}
			else
			{
				transform.localScale -= new Vector3(-0.1f,0.1f,0); // character gets bigger
			}
			isSmallTimer = 0;
		}
			isBigTimer += Time.deltaTime;
			isSmallTimer += Time.deltaTime;

	}

	override public void FixedUpdate () 
	{
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");
		
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
		
		// If the input is moving the player right and the player is facing left...
		if(h > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && facingRight)
			// ... flip the player.
			Flip();
		
		// If the player should jump...
		if(jump)
		{
			
			// Add a vertical force to the player.
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
			
			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
		}
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
	}

