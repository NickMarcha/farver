using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelController : MonoBehaviour
{
	public Tilemap tmap;

	public LevelSO levelSO;

	public GameObject blobPrefab;

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





		PaintBlob[] PBblobs = FindObjectsOfType<PaintBlob>();

		LevelInfo.BlobInfo[] blobs = new LevelInfo.BlobInfo[PBblobs.Length];
		for (int i = 0; i < PBblobs.Length; i++)
		{
			blobs[i] = new LevelInfo.BlobInfo {position = PBblobs[i].transform.position, color = PBblobs[i].GetComponent<SpriteRenderer>().color};
		}

		levelSO.Save(new LevelInfo(level, blobs, tmap.cellBounds.position));
		Debug.Log("Saved Level to SO");
	}

	public void LoadLevel()
	{
		#region clear Level
		tmap.ClearAllTiles();
		PaintBlob[] blobs = FindObjectsOfType<PaintBlob>();
		foreach (PaintBlob item in blobs)
		{
			DestroyImmediate(item.gameObject);
		}
		#endregion
		if (!levelSO)
		{
			Debug.LogWarning("No LevelSO found on levelcontroller");
			return;
		}
		TileBase[,,] level = levelSO.GetLevel().tiles;

		Vector3Int startPos = levelSO.GetLevel().startPos;

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

		foreach (LevelInfo.BlobInfo item in levelSO.GetLevel().blobs)
		{
			GameObject b = Instantiate(blobPrefab, item.position,Quaternion.identity, tmap.transform);
			b.GetComponent<SpriteRenderer>().color = item.color;
		}
		Debug.Log("Loaded Level from SO");
	}

	[System.Serializable]
	public struct LevelInfo
	{
		[SerializeField]
		public TileBase[,,] tiles;
		[SerializeField]
		public BlobInfo[] blobs;
		[SerializeField]
		public Vector3Int startPos;

		public LevelInfo(TileBase[,,] tiles, BlobInfo[] blobs, Vector3Int startPos){
			this.tiles = tiles;
			this.blobs = blobs;
			this.startPos = startPos;
		}

		public struct BlobInfo
		{
			public Vector3 position;
			public Color color;
		}
	}
}
