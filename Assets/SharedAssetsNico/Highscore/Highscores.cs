using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(DisplayHighscores))]
public class Highscores : MonoBehaviour {

    const string privateCode = "CS6ji0qZ9U2ahsuiR2BkEA0sfB4Ck8SEu60uHAQSvBug";
    const string publicCode = "5cb5f9363eba5e041c089e8e";
    // Dreamlo free leaderboards are HTTP-only ("SSL not enabled"); migrate to your HTTPS backend when ready.
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
        if (string.IsNullOrWhiteSpace(textStream))
        {
            highscoresList = Array.Empty<Highscore>();
            return;
        }

        string trimmed = textStream.Trim();
        if (trimmed.StartsWith("ERROR:", StringComparison.OrdinalIgnoreCase))
        {
            Debug.LogWarning("DreamLo / highscores: " + trimmed);
            highscoresList = Array.Empty<Highscore>();
            return;
        }

        string[] entries = trimmed.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        var list = new List<Highscore>(entries.Length);

        foreach (string line in entries)
        {
            string[] parts = line.Split('|');
            // pipe format: name|score|?|?|date|rank (dreamlo)
            if (parts.Length < 5)
            {
                continue;
            }

            if (!int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int score))
            {
                continue;
            }

            string name = parts[0]?.Trim() ?? "";
            string when = parts[4]?.Trim() ?? "";
            list.Add(new Highscore(name, score, when));
            Debug.Log(name + " : " + score + " : " + when);
        }

        highscoresList = list.ToArray();
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