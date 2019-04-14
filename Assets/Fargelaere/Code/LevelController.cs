using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelController : MonoBehaviour
{
	public static LevelController Instance { get; private set; }
	public Tilemap tmap;

	Stack<LevelInfo> levelStates;

	LevelInfo defaultState;

	public GameObject blobPrefab;

	public bool TrackTileChanges = false;
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

		levelStates = new Stack<LevelInfo>();

		TouchInputController.AddListeners(SaveOldState, SaveOldState);
		defaultState = GetState();
		//SaveOldState();

	}

	private void Update()
	{
		//Only for debug purposes

		if (Input.GetKeyDown(KeyCode.Z))
		{
			UndoState();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			ResetLevel();
		}
	}

	public void ResetLevel()
	{
		SetState(defaultState);
		levelStates = new Stack<LevelInfo>();
		SaveOldState();
	}
	#region So it can listen to touchInputController
	public static void SaveOldState(Direction4 dir)
	{
		SaveOldState();
	}

	public static void SaveOldState(Vector2 point)
	{
		SaveOldState();
	}
	#endregion
	public static void SaveOldState()
	{
		if (Instance == null)
		{
			Debug.LogWarning("No instance of LevelController");
			return;
		}


		LevelInfo newInfo = GetState();
		if (newInfo.Equals(Instance.defaultState) &&(Instance.levelStates.Count == 0 || newInfo.Equals(Instance.levelStates.Peek())))
		{
			Instance.levelStates.Push(newInfo);
			Debug.Log("Saved state to history");
		} else
		{
			newInfo.DeleteInfo();
		}
	}

	public static void UndoState()
	{
		if (GetState().Equals(Instance.defaultState))
		{
			Debug.LogWarning("Nothing to undo");
			return;
		}

		if (Instance.levelStates.Count == 0)
		{
			SetState(Instance.defaultState);

		}
		else
		{
			LevelInfo lastState =Instance.levelStates.Pop();
			SetState(lastState);
			lastState.DeleteInfo();
		}
		Debug.Log("Undid (TrackTileChanges: " + Instance.TrackTileChanges + ")");
	}

	static LevelInfo GetState()
	{
		Tilemap tmap = Instance.tmap;
		TileBase[,,] level = new TileBase[tmap.cellBounds.size.x, tmap.cellBounds.size.y, tmap.cellBounds.size.z];

		if (Instance.TrackTileChanges)
		{
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

		}
		GridEntity[] PBblobs = FindObjectsOfType<GridEntity>();

		LevelInfo.GridEntityInfo[] blobs = new LevelInfo.GridEntityInfo[PBblobs.Length];
		for (int i = 0; i < PBblobs.Length; i++)
		{
			blobs[i] = new LevelInfo.GridEntityInfo(PBblobs[i],Instance.transform);
		}
		return new LevelInfo(level, blobs, tmap.cellBounds.position);
	}

	static void SetState(LevelInfo newState)
	{
		#region clear Level

		if (Instance.TrackTileChanges)
		{
			Instance.tmap.ClearAllTiles();
		}
		PaintBlob[] blobs = FindObjectsOfType<PaintBlob>();
		foreach (PaintBlob item in blobs)
		{
			Destroy(item.gameObject);
		}
		#endregion

		if (Instance.TrackTileChanges)
		{
			TileBase[,,] level = newState.tiles;

			Vector3Int startPos = newState.startPos;

			for (int i = 0; i < level.GetLength(0); i++)
			{
				for (int u = 0; u < level.GetLength(1); u++)
				{
					for (int o = 0; o < level.GetLength(2); o++)
					{
						Instance.tmap.SetTile(new Vector3Int(i, u, o) + startPos, level[i, u, o]);
					}
				}
			}

		}

		foreach (LevelInfo.GridEntityInfo item in newState.Entities)
		{
			GameObject b = Instantiate(item.Original.gameObject, Instance.tmap.transform);
			b.SetActive(true);
		}
	}
}
