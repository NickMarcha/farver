using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "LevelController/level")]
public class LevelSO : ScriptableObject
{
    
	private TileBase[,,] tiles;

	private Vector3Int startPos;


	public void Save(TileBase[,,] tiles, Vector3Int startPos)
	{
		this.tiles = tiles;
		this.startPos = startPos;
	}

	public TileBase[,,]GetTiles()
	{
		return tiles;
	}

	public Vector3Int GetStartPos()
	{
		return startPos;
	}
}
