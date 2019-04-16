using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	public MenuButton HighscoreButton;
	public int HighscoreScene;

	public MenuButton StartGameButton;
	public int GameScene;

	public MenuButton LevelSelectButton;
	public int LevelSelectScene;

	public MenuButton CreditsButton;
	public int CreditsScene;

	public MenuButton QuitButton;


	private void Awake()
	{
		HighscoreButton?.GotPressed.AddListener(Highscore);
		StartGameButton?.GotPressed.AddListener(StartGame);
		LevelSelectButton?.GotPressed.AddListener(LevelSelect);
		CreditsButton?.GotPressed.AddListener(Credits);

		QuitButton?.GotPressed.AddListener(Quit);

	}

	public void Highscore()
	{
		AsyncOperation loading = SceneManager.LoadSceneAsync(SceneUtility.GetScenePathByBuildIndex(HighscoreScene));

		loading.allowSceneActivation = true;
	}

	public void StartGame()
	{
		AsyncOperation loading = SceneManager.LoadSceneAsync(SceneUtility.GetScenePathByBuildIndex(GameScene));

		loading.allowSceneActivation = true;
	}

	public void LevelSelect()
	{
		AsyncOperation loading = SceneManager.LoadSceneAsync(SceneUtility.GetScenePathByBuildIndex(LevelSelectScene));

		loading.allowSceneActivation = true;
	}

	public void Credits()
	{
		AsyncOperation loading = SceneManager.LoadSceneAsync(SceneUtility.GetScenePathByBuildIndex(CreditsScene));

		loading.allowSceneActivation = true;
	}

	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
	}
}
