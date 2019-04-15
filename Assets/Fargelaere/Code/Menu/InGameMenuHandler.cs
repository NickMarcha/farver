using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuHandler : MonoBehaviour
{
	public MenuButton UndoButton;
	public MenuButton RestartButton;
	private void OnEnable()
	{
		TouchInputController.Instance?.InputHappened.AddListener(UpdateGraphics);
		UndoButton?.GotPressed.AddListener(Undo);
		RestartButton?.GotPressed.AddListener(ResetLevel);
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
