using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof (Highscore))]
public class DisplayHighscores : MonoBehaviour {

    public TextMeshProUGUI[] highscoreText;
    Highscores highscoreManager;

	
	void Start () {
        for (int i = 0; i < highscoreText.Length; i++)
        {
            highscoreText[i].text = i + 1  + ". Fetching...";
        }
        highscoreManager = GetComponent<Highscores>();

        StartCoroutine("RefreshHighscores");
	}
	
    public void OnHighscoresDownloaded(Highscore[] _highscoreList)
    {
        for (int i = 0; i < highscoreText.Length; i++)
        {
            highscoreText[i].text = i + 1 +". ";
            if(_highscoreList.Length > i)
            {
                highscoreText[i].text += _highscoreList[i].username + " - " + _highscoreList[i].score + " : " + _highscoreList[i].dateAndTime;
            }
        }
    }
	IEnumerator RefreshHighscores()
    {
        while (true)
        {
            highscoreManager.DownloadHighscores();
            yield return new WaitForSeconds(30);
        }
    }
}
