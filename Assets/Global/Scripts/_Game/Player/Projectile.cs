using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifetime = 5;
    [SerializeField] SpriteRenderer spr;
    public LayerMask levelLayer;
    public Vector2 direction;
    BoxCollider2D col;

	[SerializeField] Rigidbody2D rb;

    public void Start()
    {
        if (direction == Vector2.left) spr.flipX = true;
		col = gameObject.GetComponent<BoxCollider2D>();
	}

	public void FixedUpdate()
	{
        var hit = Physics2D.BoxCast(transform.position, col.size, 0, Vector2.zero, 0, levelLayer);
        if (hit)
        {
			if (hit.transform.gameObject.tag != "Player") Destroy(this.gameObject);
		}

        lifetime -= Time.fixedDeltaTime;
        if (lifetime < 0) Destroy(this.gameObject);
        rb.velocity = direction * speed;
        //transform.Translate(direction * speed * Time.fixedDeltaTime);
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
        var enemy = collision.gameObject.GetComponent<WaypointAI>();
        if (enemy is not null)
        {
            enemy.StartCoroutine(enemy.IDieNow());
			Destroy(this.gameObject);
            return;
		}
	}
}
