using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllerScript: MonoBehaviour {
	
	[HideInInspector] public bool facingRight = true;
	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float speed = 0.1F;
	public Transform groundCheck;

	private Dictionary<int, Vector3> gravityList = new Dictionary<int, Vector3>();
	private int gravity = 0;
	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;
	
	
	// Use this for initialization
	void Awake () 
	{
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		gravityList.Add(0, new Vector3(0, -30));
		gravityList.Add(1, new Vector3(30, 0));
		gravityList.Add(2, new Vector3(0, 30));
		gravityList.Add(3, new Vector3(-30, 0));
	}
	
	// Update is called once per frame
	void Update () 
	{
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
	}
	
	void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");
		
		anim.SetFloat("Speed", Mathf.Abs(h));
		
		if (h * rb2d.velocity.x < maxSpeed && (gravity == 0 || gravity == 2)) {
			if (gravity == 0)
				rb2d.AddForce (Vector2.right * h * moveForce);
			if (gravity == 2)
				rb2d.AddForce (Vector2.left * h * moveForce);
		}

		if (h * rb2d.velocity.y < maxSpeed && (gravity == 1 || gravity == 3)) {
			if (gravity == 1)
				rb2d.AddForce (Vector2.up * h * moveForce);
			if (gravity == 3)
				rb2d.AddForce (Vector2.down * h * moveForce);
		}

		if (Mathf.Abs (rb2d.velocity.x) > maxSpeed && (gravity == 0 || gravity == 2))
			rb2d.velocity = new Vector2(Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

		if (Mathf.Abs (rb2d.velocity.y) > maxSpeed && (gravity == 1 || gravity == 3))
			rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Sign (rb2d.velocity.y) * maxSpeed);
		
		if (h > 0 && !facingRight)
			Flip ();
		else if (h < 0 && facingRight)
			Flip ();

		if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.DownArrow)) {
			CycleGravity();
			SetGravity ();
		}

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			// Get movement of the finger since last frame
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			// Move object across XY plane
			transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
		}

		/*if (Input.touchCount == 1 && Input.GetTouch(0).position.x <= Screen.currentResolution.width/2) {
			this.gameObject.transform.position.Set(Input.GetTouch(0).position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
			rb2d.AddForce (Vector2.left * h * moveForce);
		}
		else if (Input.touchCount == 1 && Input.GetTouch(0).position.x > Screen.currentResolution.width/2) {
			this.gameObject.transform.position.x = Input.GetTouch(0).position.x;
			rb2d.AddForce (Vector2.right * h * moveForce);
		}*/

	}

	void CycleGravity()
	{
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			gravity += 1;
			if (gravity > 3)
				gravity = 0;
		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			gravity -= 1;
			if (gravity < 0)
				gravity = 3;
		}
	}

	void SetGravity()
	{
		Vector3 temp;
		gravityList.TryGetValue(gravity, out temp);
		Physics2D.gravity = temp;
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}