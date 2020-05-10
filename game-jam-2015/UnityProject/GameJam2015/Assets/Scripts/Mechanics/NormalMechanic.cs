using UnityEngine;
using System.Collections;

public class NormalMechanic : IMechanic {
	
	public float moveForce = 20f;			// Amount of force added to move the player left and right.
	public float jumpForce = 1200f;			// Amount of force added when the player jumps.
	public float bombCooldown = 1f;
	public float bombTimer = 0f;
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
	private Animator anim;					// Reference to the player's animator component.
	private GameObject hero;

	override public void Start()
	{
		hero =  GameObject.FindWithTag("Player");
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
	}

	override public void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(hero.transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
		
		// If the jump button is pressed and the player is grounded then the player should jump.
		if(Input.GetKeyDown(KeyCode.UpArrow) && grounded)
			jump = true;

		if (Input.GetKeyDown(KeyCode.Space) && bombTimer>bombCooldown) {
			GameObject.Instantiate(Resources.Load("bomb",  typeof(GameObject)), transform.position, transform.rotation);		
			bombTimer = 0;
		}
		bombTimer += Time.deltaTime;
	}

	override public void FixedUpdate()
	{
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");
		
		// The Speed animator parameter is set to the absolute value of the horizontal input.
		anim.SetFloat("Speed", Mathf.Abs(h));

		rigidbody2D.velocity = new Vector2(h * moveForce, rigidbody2D.velocity.y);

		
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
			// Set the Jump animator trigger parameter.
			//anim.SetTrigger("Jump");
			
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
