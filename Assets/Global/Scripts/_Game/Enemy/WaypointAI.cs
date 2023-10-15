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

    bool alive = true;

    void Start() {
        RB = GetComponent<Rigidbody2D>();
        spriterenderer = GetComponent<SpriteRenderer>();
        alive = true;
    }

    void Update() {
		if (!alive) return;
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

    public IEnumerator IDieNow()
    {
        animator.SetTrigger("onDeath");
        alive = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        RB.simulated = false;
        yield return null;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);

        Destroy(this.gameObject);
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (!alive) return;
        var player = collision.gameObject.GetComponent<CharacterController2D>();
        if (player is not null)
        {
            player.TakeDamage((int) Random.Range(1, 3));
        }

		//var projectile = collision.gameObject.GetComponent<Projectile>();
        //if (projectile is not null)
        //{
        //    StartCoroutine(IDieNow());
        //    Destroy(projectile.gameObject);
        //}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (!alive) return;
		var player = collision.gameObject.GetComponent<CharacterController2D>();
		if (player is not null)
		{
			player.TakeDamage((int)Random.Range(1, 3));
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var projectile = collision.gameObject.GetComponent<Projectile>();
		if (projectile is not null) StartCoroutine(IDieNow());
		Destroy(projectile.gameObject);
	}
}