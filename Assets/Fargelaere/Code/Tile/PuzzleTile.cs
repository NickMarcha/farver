using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Puzzle Tile")]
/// <summary>
/// Represents a tile used in this game
/// </summary>
public class PuzzleTile : Tile
{
	/// <summary>
	/// Is this tile solid
	/// </summary>
	[HideInSubClass]
	public bool Solid;

	public virtual bool CanPass(Direction4 dir) {
		return Solid;
	}

	public virtual void OnPaintSlide(PaintBlob blob, Vector3Int pos, Direction4 dir) {
		return;
	}
}
