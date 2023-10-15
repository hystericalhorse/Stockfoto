using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WaypointAI : MonoBehaviour {
    [SerializeField] float Speed;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriterenderer;

    [Header("AI")]
    [SerializeField] Transform[] Waypoints;

    Rigidbody2D RB;
    Vector2 Velocity = Vector2.zero;
    bool FaceRight = true;
    Transform TargetWaypoint = null;
    float Timer = .01f;

    void Start() {
        RB = GetComponent<Rigidbody2D>();
        spriterenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        Vector2 Direction = Vector2.zero;
        if (TargetWaypoint == null) SetNewWaypointTarget();

        Direction.x = Mathf.Sign(TargetWaypoint.position.x - transform.position.x);
        float DX = Mathf.Abs(TargetWaypoint.position.x - transform.position.x);
        if (DX <= 0.25f) {
            Timer -= Time.deltaTime;
            if (Timer <= 0) SetNewWaypointTarget();
        }

        Velocity.x = Direction.x * Speed;

        // move character
        RB.velocity = Velocity;

        // Rotate character to face direction of movement
        if (Velocity.x > 0 && !FaceRight) Flip();
        if (Velocity.x < 0 && FaceRight) Flip();
    }

    private void Flip() {
        Vector3 CurrentScale = gameObject.transform.localScale;
        CurrentScale.x *= -1;
        gameObject.transform.localScale = CurrentScale;

        FaceRight = !FaceRight;
    }

    private void SetNewWaypointTarget() {
        Transform Waypoint = null;
        do {
            Waypoint = Waypoints[Random.Range(0, Waypoints.Length)];
        } while (Waypoint == TargetWaypoint);
        TargetWaypoint = Waypoint;
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        var player = collision.gameObject.GetComponent<CharacterController2D>();
        if (player is not null)
        {
            player.TakeDamage((int) Random.Range(1, 3));
        }
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		var player = collision.gameObject.GetComponent<CharacterController2D>();
		if (player is not null)
		{
			player.TakeDamage((int)Random.Range(1, 3));
		}
	}
}