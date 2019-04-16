using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LeaderBoardHandler : MonoBehaviour
{

	public GameObject PostScoreMenuCanv;

	public TextMeshProUGUI Score;
	public TMP_InputField inputName;
	public MenuButton postScoreButton;
	public Highscores highscores;

	private void OnEnable()
	{
		if (LevelController.CompletedGame)
		{
			PostScoreMenu();
			postScoreButton.GotPressed.AddListener(PostScore);
		}
		else
		{
			PostScoreMenuCanv.SetActive(false);

		}
	}

	public void BackToMain()
	{
		AsyncOperation loading = SceneManager.LoadSceneAsync(SceneUtility.GetScenePathByBuildIndex(0));

		loading.allowSceneActivation = true;
	}

	public void PostScoreMenu()
	{
		PostScoreMenuCanv.SetActive(true);
		Score.text = (1000 - (LevelController.moves * 10)).ToString();

	}

	public void PostScore()
	{
		if (inputName.text.Length < 2)
		{
			return;
		}
		PostScoreMenuCanv.SetActive(false);
		Highscores.AddNewHighscore(inputName.text, (1000 - (LevelController.moves * 10)));

	}
}
