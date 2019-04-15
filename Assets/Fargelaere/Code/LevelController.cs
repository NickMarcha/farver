using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelController : MonoBehaviour
{
	public List<GameObject> Levels = new List<GameObject>();
	public int currentLevel = 1;
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

		//TouchInputController.AddListeners(SaveOldState, SaveOldState);
		defaultState = GetState();

        TouchInputController.AddListeners(swipe: dir => Pushable.PushAll(tmap, dir));

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

		if (Input.GetKeyDown(KeyCode.N))
		{
			WonGame();
		}
	}

	public static void ResetLevel()
	{
		SetState(Instance.defaultState);
		foreach (LevelInfo item in Instance.levelStates)
		{
			item.DeleteInfo();
		}
		Instance.levelStates = new Stack<LevelInfo>();

		//TouchInputController.Instance?.InputHappened?.Invoke();
		Debug.Log("Reset Level & history");
		InGameMenuHandler.UpGraphs?.Invoke();
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


	static bool saving = false;

	public static void SaveOldState()
	{
		if (!saving)
		{
			saving = true;
			Instance?.StartCoroutine(nameof(SaveStateCo));
		}
	}
	private IEnumerator SaveStateCo()
	{
		yield return new WaitForEndOfFrame();
		saveOldState();
		saving = false;
	}
	static void saveOldState()
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
		if (Instance.levelStates.Count == 0)
		{
			if (newInfo.Equals(Instance.defaultState))
			{
				Debug.Log("Didn't save default state");
				return;
			}else
			{
				Instance.levelStates.Push(newInfo);
				Debug.Log("Saved state to history");
				return;
			}
			
		} else if (!newInfo.Equals(Instance.levelStates.Peek()))
		{
			Instance.levelStates.Push(newInfo);
			Debug.Log("Saved state to history");
			return;
		}else
		{
			Debug.Log("State Already saved");
			newInfo.DeleteInfo();
			return;
		}
	}

	public static bool CanUndo()
	{
		if(Instance?.levelStates.Count > 0)
		{
			return true;
			
		}
		else
		{
			LevelInfo currentState = GetState();
			bool returnValue = true;

			if (currentState.Equals(Instance?.defaultState))
			{
				returnValue = false;
			}

			currentState.DeleteInfo();
			return returnValue;
		}
	}
	public static void UndoState()
	{
		if (Instance.levelStates.Count > 0)
		{
			LevelInfo lastState = Instance.levelStates.Pop();
			SetState(lastState);
			lastState.DeleteInfo();
			Debug.Log("Undid ");
		}
		else
		{
			LevelInfo currentState = GetState();
			

			if (currentState.Equals(Instance.defaultState))
			{
				Debug.LogWarning("Nothing to undo");
			} else
			{
				SetState(Instance.defaultState);
				Debug.Log("Undid back to default");
			}

			currentState.DeleteInfo();
			
		}
	}

	static LevelInfo GetState()
	{

		return new LevelInfo(
			FindObjectsOfType<GridEntity>()
			.Where(i => i.gameObject.activeSelf)
			.Select(i => new LevelInfo.GridEntityInfo(i, Instance.transform))
			.ToList()
			);
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

	public static void WonGame()
	{
		if (!Instance)
		{
			Debug.Log("No level controller");
		}
		Destroy(Instance.tmap.transform.parent.gameObject);

		Instance.currentLevel++;
		if(Instance.currentLevel > Instance.Levels.Count)
		{
			Debug.Log("Won whole game");
			return;
		}
		GameObject level = Instantiate(Instance.Levels.ElementAt(Instance.currentLevel-1));
		Instance.tmap = level.GetComponentInChildren<Tilemap>();
	}
}
