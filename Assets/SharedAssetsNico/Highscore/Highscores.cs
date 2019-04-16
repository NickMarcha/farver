using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(DisplayHighscores))]
public class Highscores : MonoBehaviour {

    const string privateCode = "CS6ji0qZ9U2ahsuiR2BkEA0sfB4Ck8SEu60uHAQSvBug";
    const string publicCode = "5cb5f9363eba5e041c089e8e";
    const string webURL = "http://dreamlo.com/lb/";

    public Highscore[] highscoresList;

    DisplayHighscores highscoresDisplay;

    static Highscores instance;
    private void Awake()
    {
        instance = this;
        highscoresDisplay = GetComponent<DisplayHighscores>();
    }

    public static void AddNewHighscore(string username, int score)
    {

        instance.StartCoroutine(instance.UploadNewHighscore(username, score));
    }

    IEnumerator UploadNewHighscore(string username, int score)
    {
		UnityWebRequest www = UnityWebRequest.Get(webURL + privateCode + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + score);
		//WWW www = new WWW(webURL + privateCode +"/add/"+ WWW.EscapeURL(username) +  "/" + score);

		yield return www.SendWebRequest();

        if(string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Upload Succesfull");
            DownloadHighscores();
        } else
        {
            Debug.Log("Error uploading: " + www.error);
        }
    }

    public void DownloadHighscores()
    {
        StartCoroutine("DownloadHighscoresFromDatabase");
    }

    IEnumerator DownloadHighscoresFromDatabase()
    {
		UnityWebRequest www =  UnityWebRequest.Get(webURL + publicCode + "/pipe/0/10");
        //WWW www = new WWW(webURL + publicCode + "/pipe/0/10");
        yield return www.SendWebRequest();

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Download Succesfull");
            FormatHighscores(www.downloadHandler.text);
            highscoresDisplay.OnHighscoresDownloaded(highscoresList);
        }
        else
        {
            Debug.Log("Error Downloading: " + www.error);
        }
    }

    void FormatHighscores(string textStream)
    {
        string[] entries = textStream.Split(new char[] {'\n'},System.StringSplitOptions.RemoveEmptyEntries);

        highscoresList = new Highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] {'|'});

            highscoresList[i] = new Highscore(entryInfo[0], int.Parse(entryInfo[1]), entryInfo[4]);
            Debug.Log(highscoresList[i].username +" : "+ highscoresList[i].score +" : " + highscoresList[i].dateAndTime);
        }
        
    }
}

public struct Highscore
{
    public string username;
    public int score;
    public string dateAndTime;

    public Highscore(string _username, int _score, string _dateAndTime)
    {
        username = _username;
        score = _score;
        dateAndTime = _dateAndTime;
    }
}