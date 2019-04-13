using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "LevelController/level")]
public class LevelSO : ScriptableObject
{

	private LevelController.LevelInfo level;


	public void Save(LevelController.LevelInfo level)
	{
		this.level = level;
	}

	public LevelController.LevelInfo GetLevel()
	{
		return level;
	}
}
