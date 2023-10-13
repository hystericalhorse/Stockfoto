using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEngine.InputSystem;

/// <summary>
/// Handles movement and collision of a 2D character object.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(PlayerInput))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerInput input;
    [SerializeField] CapsuleCollider2D col;
    [SerializeField] SpriteRenderer spr;
	[SerializeField] Animator anim;
	
	[SerializeField] LayerMask controllerLayer;

	Vector2 _input = Vector2.zero;
	public Vector2 _translation = Vector2.zero;

	bool onGround { get; set; } = true;
	bool jumping = false;

	#region MonoBehavior

	private void Start()
	{
		if (rb is null) rb = gameObject.GetComponent<Rigidbody2D>();
		if (input is null) input = gameObject.GetComponent<PlayerInput>();
		if (col is null) col = gameObject.GetComponent<CapsuleCollider2D>();

		rb.freezeRotation = true;
		rb.gravityScale = 0;

		
	}

	private void Update()
	{
		
	}

	private void FixedUpdate()
	{
		GroundCheck(); // First

		// TODO: Literally the rest of the character controller haha.
		CalculateVerticalMotion();
		CalculateHorizontalMotion();

		anim.SetFloat("vel", Mathf.Abs(_translation.x));

		ApplyMotion(); // Last
	}

	private void LateUpdate() { }

	#endregion

	#region Controller Methods

	/// <summary>
	/// Check for collision beneath the controller.
	/// </summary>
	private void GroundCheck()
	{
		anim.SetBool("grounded", onGround);

		bool hitOnGround = Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0, Vector2.down, 0.02f, ~controllerLayer);
		Debug.DrawRay(col.bounds.center, Vector2.down * 0.02f, Color.red);

		//
		// Additional code may be put here to handle coyote time, double jumping, and other things.
		//

		if (_translation.y > 0 && jumping)
		{
			onGround = false;
			return;
		}

		onGround = hitOnGround;

		if (onGround) jumping = false;

	}

	private void CalculateVerticalMotion()
	{
		//TODO
		if (!onGround) _translation.y += -10 * Time.fixedDeltaTime;
		else _translation.y = 0;
	}

	private void CalculateHorizontalMotion()
	{
		//TODO
		_translation.x = _input.x;
	}

	private void ApplyMotion() => rb.velocity = _translation * 10;

	#endregion

	#region Input Methods

	public void Move(InputAction.CallbackContext context)
	{
		_input = context.ReadValue<Vector2>();

		if (_input.x > 0 && spr.flipX) spr.flipX = false;
		if (_input.x < 0 && !spr.flipX) spr.flipX = true;
	}

	public void Jump(InputAction.CallbackContext context)
	{
		//TODO
		if (jumping) return;
		jumping = true;
		anim.SetTrigger("onJump");
		_translation.y = 3;
	}

	public void Pause(InputAction.CallbackContext context)
	{
		GameManager.gameManager.SetPause();
	}

	#endregion
}
