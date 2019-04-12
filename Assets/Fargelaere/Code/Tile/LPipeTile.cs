using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tiles/L Pipe Tile")]
/// <summary>
/// Represents a tile used in this game
/// </summary>
public class LPipeTile : PuzzleTile
{
	/// <summary>
	/// The direction of the "top" of the "L"
	/// </summary>
	[Header("The direction of the 'top' of the 'L'")]
	public Direction4 Facing;

	
	public override bool CanPass(Direction4 dir)
	{
		return (dir == Facing || dir == Facing.Rotate(1));
	}
	public override void OnPaintSlide(PaintBlob blob, Vector3Int pos, Direction4 dir)
	{
		if(dir == Facing.Flip())
		{
			dir = Facing.Rotate(1);
		} else if (dir == Facing.Rotate(1).Flip())
		{
			dir = Facing;
		} else
		{
			Debug.LogError("Unexpectied Behaviour in LPipeTile: " + dir);
		}

		blob.ChangeDirection(dir);
	}
}
