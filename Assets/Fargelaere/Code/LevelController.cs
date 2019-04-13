using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelController : MonoBehaviour
{
	public static LevelController Instance { get; private set; }
	public Tilemap tmap;

	Stack<LevelInfo> LevelStates;

	public GameObject blobPrefab;

	private void Awake()
	{
		if (!Instance)
		{
			Instance = this;
		}
		else
		{
			Debug.LogWarning("There is more than one LevelController in scene");
			enabled = false;
			return;
		}

		LevelStates = new Stack<LevelInfo>();
	}

	private void Update()
	{
		//Only for debug purposes

		if (Input.GetKeyDown(KeyCode.Z))
		{
			UndoState();
		}
	}

	public static void SaveNewState()
	{
		if (Instance == null)
		{
			Debug.LogWarning("No instance of LevelController");
			return;
		}

		Tilemap tmap = Instance.tmap;
		TileBase[,,] level = new TileBase[tmap.cellBounds.size.x, tmap.cellBounds.size.y, tmap.cellBounds.size.z];

		for (int x = 0; x < level.GetLength(0); x++)
		{
			for (int y = 0; y < level.GetLength(1); y++)
			{
				for (int z = 0; z < level.GetLength(2); z++)
				{
					level[x, y, z] = tmap.GetTile(new Vector3Int(x, y, z) + tmap.cellBounds.position);
				}
			}
		}

		PaintBlob[] PBblobs = FindObjectsOfType<PaintBlob>();

		LevelInfo.BlobInfo[] blobs = new LevelInfo.BlobInfo[PBblobs.Length];
		for (int i = 0; i < PBblobs.Length; i++)
		{
			blobs[i] = new LevelInfo.BlobInfo { position = PBblobs[i].transform.position, color = PBblobs[i].GetComponent<SpriteRenderer>().color };
		}

		Instance.LevelStates.Push(new LevelInfo(level, blobs, tmap.cellBounds.position));
		Debug.Log("Saved Level to SO");
	}

	public void UndoState()
	{
		if (LevelStates.Count ==0)
		{
			Debug.LogWarning("Nothing to undo");
			return;
		}

		#region clear Level
		tmap.ClearAllTiles();
		PaintBlob[] blobs = FindObjectsOfType<PaintBlob>();
		foreach (PaintBlob item in blobs)
		{
			Destroy(item.gameObject);
		}
		#endregion

		LevelInfo newState = LevelStates.Pop();

		TileBase[,,] level = newState.tiles;

		Vector3Int startPos = newState.startPos;

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

		foreach (LevelInfo.BlobInfo item in newState.blobs)
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
