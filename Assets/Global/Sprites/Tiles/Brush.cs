using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor.Tilemaps
{
	[CustomGridBrush(true, false, false, "Checkered Brush")]
	public class CheckeredBrush : GridBrushBase
	{
		public static TileBase tileA;
		public static TileBase tileB;

		public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
		{
			Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
			if (tilemap != null)
			{
				SetCheckeredTile(tilemap, position);
			}
		}

		private static void SetCheckeredTile(Tilemap tilemap, Vector3Int position)
		{
			bool evenX = position.x % 2 == 0;
			bool evenY = position.y % 2 == 0;

			// below untested and I'm very sleepy so this could be wrong but you should get the idea to fix
			TileBase choice;

			if (evenX && evenY) choice = tileA;
			else if (evenX && !evenY) choice = tileB;
			else if (!evenX && evenY) choice = tileB;
			else choice = tileA;

			tilemap.SetTile(position, choice);
		}

		// there are many other methods you can override
		// to get an idea how to implement, see the examples brushes in that github link
	}

	[CustomEditor(typeof(CheckeredBrush))]
	public class CheckeredBrushEditor : GridBrushEditorBase
	{
		/// <summary>Returns all valid targets that the brush can edit.</summary>
		/// <remarks>Valid targets for the CheckeredBrush are any GameObjects with a Tilemap component.</remarks>
		public override GameObject[] validTargets
		{
			get
			{
				return GameObject.FindObjectsOfType<Tilemap>().Select(x => x.gameObject).ToArray();
			}
		}
	}
}