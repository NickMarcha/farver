using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuHandler : MonoBehaviour
{
	private void OnEnable()
	{
		TouchInputController.Instance?.InputHappened.AddListener(UpdateGraphics);
	}

	private void OnDisable()
	{
		TouchInputController.Instance?.InputHappened.RemoveListener(UpdateGraphics);
	}
	public void Undo()
	{
		LevelController.UndoState();
	}

	public void Reset()
	{
		LevelController.ResetLevel();
	}

	public void UpdateGraphics()
	{
		//TODO: actually updateGraphics
	}
}
