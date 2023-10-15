using UnityEngine;

public class GameObjective : MonoBehaviour
{
	[SerializeField] float _size = 2;

	private void FixedUpdate()
	{
#if DEBUG
		Debug.DrawRay(transform.position, Vector2.up * _size, Color.red);
		Debug.DrawRay(transform.position, Vector2.down * _size, Color.red);
		Debug.DrawRay(transform.position, Vector2.right * _size, Color.red);
		Debug.DrawRay(transform.position, Vector2.left * _size, Color.red);
#endif

		var collisions = Physics2D.BoxCastAll(transform.position, Vector2.one * _size, 0, Vector2.zero);
		if (collisions.Length > 0)
		{
			foreach (var collision in collisions)
			{
				var player = collision.transform.gameObject.GetComponent<CharacterController2D>();
				if (player is not null)
				{
					GameManager.gameManager.WinGame();
					return;
				}
			}
		}
	}

	
}
