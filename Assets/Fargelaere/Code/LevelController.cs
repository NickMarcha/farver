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

	//public bool TrackTileChanges = false;
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
		foreach (LevelInfo item in levelStates)
		{
			item.DeleteInfo();
		}
		levelStates = new Stack<LevelInfo>();
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
		foreach (Pushable item in FindObjectsOfType<Pushable>())
		{
			if (item.Sliding)
			{
				Debug.LogWarning("Things Are In Motion");
				return;
			}
		}

		if (Instance == null)
		{
			Debug.LogWarning("No instance of LevelController");
			return;
		}


		LevelInfo newInfo = GetState();

		if (newInfo.Equals(Instance.defaultState)) {
			Debug.Log("Didn't save default state");
			newInfo.DeleteInfo();

		}
		else if (Instance.levelStates.Count == 0 || !newInfo.Equals(Instance.levelStates.Peek()))
		{
			Instance.levelStates.Push(newInfo);
			Debug.Log("Saved state to history");

		} else
		{
			Debug.Log("Unexpected?");
			newInfo.DeleteInfo();
		}
	}

	public static void UndoState()
	{
		LevelInfo currentState = GetState();
		if (currentState.Equals(Instance.defaultState))
		{
			Debug.LogWarning("Nothing to undo");
			currentState.DeleteInfo();
			return;
		}
		currentState.DeleteInfo();

		if (Instance.levelStates.Count == 0)
		{
			SetState(Instance.defaultState);
			Debug.Log("Undid back to default");
		}
		else
		{
			LevelInfo lastState =Instance.levelStates.Pop();
			SetState(lastState);
			lastState.DeleteInfo();
			Debug.Log("Undid ");
		}
		
	}

	static LevelInfo GetState()
	{
		
		GridEntity[] PBblobs = FindObjectsOfType<GridEntity>();

		LevelInfo.GridEntityInfo[] gridEntities = new LevelInfo.GridEntityInfo[PBblobs.Length];
		for (int i = 0; i < PBblobs.Length; i++)
		{
			if (PBblobs[i].gameObject.activeSelf)
			{
				gridEntities[i] = new LevelInfo.GridEntityInfo(PBblobs[i], Instance.transform);
			}
		}
		return new LevelInfo(gridEntities);
	}

	static void SetState(LevelInfo newState)
	{
		#region clear Level
		GridEntity[] blobs = FindObjectsOfType<GridEntity>();
		foreach (GridEntity item in blobs)
		{
			if (item.gameObject.activeSelf)
			{
				Destroy(item.gameObject);
			}
		}
		#endregion

		foreach (LevelInfo.GridEntityInfo item in newState.Entities)
		{
			GameObject b = Instantiate(item.Original.gameObject, Instance.tmap.transform);
			b.transform.position = item.Original.transform.position;
			b.SetActive(true);
		}
	}
}
