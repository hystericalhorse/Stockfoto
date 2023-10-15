using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class NewCustomRuleTile : RuleTile<NewCustomRuleTile.Neighbor>
{
	public bool customField;

	public class Neighbor : RuleTile.TilingRule.Neighbor
	{
		public const int _0 = 3;
		public const int _1 = 4;
		public const int _2 = 5;
		public const int _3 = 6;
	}

	public override bool RuleMatch(int neighbor, TileBase tile)
	{
		switch (neighbor)
		{
			case Neighbor.This: return tile == null;
			case Neighbor.NotThis: return tile != null;
		}
		return base.RuleMatch(neighbor, tile);
	}
}