using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelController : MonoBehaviour
{
	public Tilemap tmap;

	public LevelSO levelSO;


	public void SaveLevel()
	{
		if (!levelSO)
		{
			Debug.LogWarning("No LevelSO found on levelcontroller");
			return;
		}

		TileBase[,,] level = new TileBase[tmap.cellBounds.size.x, tmap.cellBounds.size.y, tmap.cellBounds.size.z];

		for (int i = 0; i < level.GetLength(0); i++)
		{
			for (int u = 0; u < level.GetLength(1); u++)
			{
				for (int o = 0; o < level.GetLength(2); o++)
				{
					level[i, u, o] = tmap.GetTile(new Vector3Int(i, u, o) + tmap.cellBounds.position);
				}
			}
		}

		levelSO.Save(level, tmap.cellBounds.position);

		Debug.Log("Saved Level to SO");
	}

	public void LoadLevel()
	{
		tmap.ClearAllTiles();
		if (!levelSO)
		{
			Debug.LogWarning("No LevelSO found on levelcontroller");
			return;
		}
		TileBase[,,] level = levelSO.GetTiles();

		Vector3Int startPos = levelSO.GetStartPos();

		for (int i = 0; i < level.GetLength(0); i++)
		{
			for (int u = 0; u < level.GetLength(1); u++)
			{
				for (int o = 0; o < level.GetLength(2); o++)
				{
					tmap.SetTile(new Vector3Int(i, u, o) + startPos, level[i, u, o]);
				}
			}
		}
		Debug.Log("Loaded Level from SO");
	}
}
