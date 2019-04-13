using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelController))]
public class LevelControllerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		LevelController myScript = (LevelController)target;
		if (GUILayout.Button("Save"))
		{
			myScript.SaveLevel();
		}

		if (GUILayout.Button("Load"))
		{
			myScript.LoadLevel();
		}
	}
}
