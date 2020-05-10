using UnityEngine;
using System.Collections;

public class ZeroGravityMechanic : IMechanic {
	private Animator anim;
	private GameObject hero;
	public GameObject chainSaw;
	public float movementSpeed = 10f;
	public float maxSpeed = 8f;
	public bool alive = true;
	public float weaponCDTime = 1f; // on every N seconds you can use the chainsaw
	private float coolDownTimer = 0f; // chainsaw cooldown timer
	public float velocityDownCoef = 1f; // coeficent for movement slowing
	public LineRenderer lineRenderer;
	public float rayActiveTime;
	override public void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		hero =  GameObject.FindWithTag("Player");
		hero.rigidbody2D.gravityScale = 0f; // gravity is set to zero
		anim = GetComponent<Animator>();
	}
	
	override public void Update ()
	{

		/*************MOVEMENT*************/
		if (alive) // if the player is alive he can move and do things when he is not alive he can't
		{ 
		float h = Input.GetAxis ("Horizontal") * movementSpeed; // Horisontal speed
		float v = Input.GetAxis ("Vertical") * movementSpeed; //  Vertical speed
		Vector2 movementDirection = new Vector2 (h, v);     // The direction of movement

		rigidbody2D.AddForce (movementDirection, ForceMode2D.Force); // moving the character in movementDirection
		rigidbody2D.velocity = Vector2.ClampMagnitude (rigidbody2D.velocity, maxSpeed); // Max speed
		rigidbody2D.velocity -= Vector2.one*Time.deltaTime*velocityDownCoef;//slowly gets the volocyty down
		/***************Weapon*****************/
			if(facingRight == true)
			{
				if (Input.GetKeyDown(KeyCode.Space)&& coolDownTimer>weaponCDTime)
				{
					RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + 2, transform.position.y),Vector2.right);
					lineRenderer.enabled = true;         
					lineRenderer.SetPosition(0, transform.position); 
					lineRenderer.SetPosition(1, hit.point);
					rayActiveTime = 0;
					if(hit.collider.gameObject.tag == "Enemy")
					{
						hit.collider.GetComponent<Enemy>().Death(); // kills the enemy 
					}
					coolDownTimer = 0f;
				}
			}
			else
				if (Input.GetKeyDown(KeyCode.Space)&& coolDownTimer>weaponCDTime)
			{
				RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - 2, transform.position.y) ,-Vector2.right);
				lineRenderer.enabled = true;         
				lineRenderer.SetPosition(0, transform.position); 
				lineRenderer.SetPosition(1, hit.point);
				rayActiveTime = 0;

				if(hit.collider.gameObject.tag == "Enemy")
				{
					hit.collider.gameObject.GetComponent<Enemy>().Death(); // kills the enemy 
				}
				coolDownTimer = 0f;
			}
		}
		coolDownTimer += Time.deltaTime;
		rayActiveTime += Time.deltaTime;
		if (lineRenderer.enabled && rayActiveTime > 0.1f)
						lineRenderer.enabled = false;

		Debug.Log (coolDownTimer);
	}
	
	override public void FixedUpdate () {
		float h = Input.GetAxis ("Horizontal");
		anim.SetFloat("Speed", Mathf.Abs(h));
		if(h > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && facingRight)
			// ... flip the player.
			Flip();
		
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
