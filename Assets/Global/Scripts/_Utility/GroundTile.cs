using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEngine.UI;

public class GroundTile : Tile
{
	[SerializeField] private Sprite[] tiles;
	[SerializeField] private Sprite ground;

	public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
	{
		if (tilemap.GetTile(position + Vector3Int.up))
			tileData.sprite = ground;
		else 
			tileData.sprite = tiles[(uint)position.x % tiles.Length];
		tileData.colliderType = colliderType;
	}

#if UNITY_EDITOR
	[MenuItem("Assets/Create/2D/Tiles/Ground Tile")]
	public static void CreateAlternatingTile()
	{

		string path = EditorUtility.SaveFilePanelInProject(
			"Ground Tile",
			"New Ground Tile",
			"Asset",
			"Please enter a name for the new Ground tile",
			"Assets");

		AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GroundTile>(), path);
	}
#endif
}