using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;

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

			EditorUtility.SetDirty(myScript.levelSO);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		if (GUILayout.Button("Load"))
		{
			Undo.RegisterFullObjectHierarchyUndo(myScript.tmap.gameObject, "Loaded level From SO");
			myScript.LoadLevel();
		}
	}
}
