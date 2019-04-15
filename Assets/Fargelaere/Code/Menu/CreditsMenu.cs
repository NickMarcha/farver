using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenu : MonoBehaviour
{
    public void BackToMain()
	{
		AsyncOperation loading = SceneManager.LoadSceneAsync(SceneUtility.GetScenePathByBuildIndex(0));

		loading.allowSceneActivation = true;
	}
}
