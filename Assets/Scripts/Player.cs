using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float speed;
	public float jumpForce = 10;

	public float footOffset = .4f;
	public float groundDistance = .2f;
	public LayerMask groundLayer;

	private Rigidbody2D rb;
	private bool jump;
	private bool facingRight = true;
	private Animator anim;
	private bool onGround;

	// Start is called before the first frame update
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetButtonDown("Jump") && onGround)
		{
			jump = true;
		}
    }

	private void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");
		rb.velocity = new Vector2(h * speed, rb.velocity.y);

		anim.SetFloat("Speed", Mathf.Abs(h));

		onGround = false;

		RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance);
		RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance);

		if(leftCheck || rightCheck)
		{
			onGround = true;
		}

		anim.SetBool("OnGround", onGround);

		if (jump)
		{
			jump = false;
			rb.velocity = Vector2.zero;
			rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		}

		if((!facingRight && h > 0) || (facingRight && h < 0))
		{
			Flip();
		}
	}

	void Flip()
	{
		facingRight = !facingRight;
		transform.Rotate(new Vector3(0, 180, 0));
	}

	RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
	{
		//Call the overloaded Raycast() method using the ground layermask and return 
		//the results
		return Raycast(offset, rayDirection, length, groundLayer);
	}

	RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
	{
		//Record the player's position
		Vector2 pos = transform.position;

		//Send out the desired raycasr and record the result
		RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

		//If we want to show debug raycasts in the scene...

		//...determine the color based on if the raycast hit...
		Color color = hit ? Color.red : Color.green;
		//...and draw the ray in the scene view
		Debug.DrawRay(pos + offset, rayDirection * length, color);


		//Return the results of the raycast
		return hit;
	}
}
