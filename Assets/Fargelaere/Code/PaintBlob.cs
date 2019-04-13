using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Represents the code for a paint blob
/// </summary>
public class PaintBlob : MonoBehaviour
{
	/// <summary>
	/// The tilemap this blob is attatched to
	/// </summary>
	private Tilemap TileMap;

	//This variable makes sure that you cant repush blobs while any blob is moving
	private static int _blobsInAnimation;

	/// <summary>
	/// Gets or sets the position of the tile the blob is over
	/// </summary>
	public Vector3Int TilePosition
	{
		//TODO: this needs to be handled, changed because of bug after loading map
		get => TileMap ? TileMap.LocalToCell(transform.position) : GetComponentInParent<Tilemap>().LocalToCell(transform.position);
		set => TileMap.CellToLocal(value);
	}

	private const float SLIDE_TIME = 0.01f;

	private void Awake()
	{
		TileMap = GetComponentInParent<Tilemap>();
	}

	private void Update()
	{
		TouchInputController.AddListeners(Push);
	}

	/// <summary>
	/// Slides the blob in the given direction
	/// </summary>
	/// <param name="direction"></param>
	public void Push(Direction4 direction)
	{
		if (_blobsInAnimation != 0)
		{
			//Do not push if any blob is moving
			return;
		}

		StartCoroutine(nameof(CoPush), direction);

		LevelController.SaveNewState();

	}

	/// <summary>
	/// Changes the slide direction of the blob to the given direction
	/// </summary>
	/// <param name="direction"></param>
	public void ChangeDirection(Direction4 direction)
	{

		StopCoroutine(nameof(CoPush));
		_blobsInAnimation--;
		StartCoroutine(nameof(CoPush), direction);
	}
	/// <summary>
	/// Pushes the blob in the given direction until it hits a solid tile
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	private IEnumerator CoPush(Direction4 direction)
	{
		//Mark as in motion
		_blobsInAnimation++;

		//Push one-by-one tile until the next tile is solid
		//TODO: Check if next tile exist?
		while (!(TileMap.GetTile(TilePosition + Vector3Int.RoundToInt(direction.ToVector2())) as PuzzleTile).CanPass(direction))
		{
			yield return CoSlideOneTile(direction);
		}

		//Mark as standing still
		_blobsInAnimation--;
	}

	/// <summary>
	/// Slides the blob by one tile
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	private IEnumerator CoSlideOneTile(Direction4 direction)
	{
		const int GRANULARITY = 4;

		for (int i = 0; i < GRANULARITY; i++)
		{
			transform.Translate(direction.ToVector2() / GRANULARITY);
			yield return new WaitForSeconds(SLIDE_TIME / GRANULARITY);
		}

		(TileMap.GetTile(TilePosition) as PuzzleTile)?.OnPaintSlide(this, TilePosition, direction);

	}
}