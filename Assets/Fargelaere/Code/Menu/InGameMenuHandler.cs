using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InGameMenuHandler : MonoBehaviour
{
	public MenuButton UndoButton;
	public MenuButton RestartButton;

	public static UnityEvent UpGraphs = new UnityEvent();
	private void OnEnable()
	{
		TouchInputController.Instance?.InputHappened.AddListener(UpdateGraphics);
		Pushable.blockStopped.AddListener(UpdateGraphics);
		UndoButton?.GotPressed.AddListener(Undo);
		RestartButton?.GotPressed.AddListener(ResetLevel);
		RestartButton.Active = UndoButton.Active = LevelController.CanUndo();
		UpGraphs.AddListener(UpdateGraphics);
	}

	private void OnDisable()
	{
		TouchInputController.Instance?.InputHappened.RemoveListener(UpdateGraphics);
		UndoButton?.GotPressed.AddListener(Undo);
		RestartButton?.GotPressed.AddListener(ResetLevel);
	}
	void Undo()
	{
		LevelController.UndoState();
	}

	void ResetLevel()
	{
		LevelController.ResetLevel();
	}

	public void UpdateGraphics()
	{

		//TODO: actually updateGraphics
		RestartButton.Active = UndoButton.Active = LevelController.CanUndo();
	}
}
