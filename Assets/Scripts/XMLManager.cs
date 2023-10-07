using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class XMLManager : MonoBehaviour
{
    public static XMLManager Instance;
    public Leaderboard leaderboard;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("XML");
            Destroy(gameObject);
        }
        /*HighScoreEntry A = new HighScoreEntry();
        A.score = 50;
        A.name = "AAC";
        leaderboard.list.Add(A);
        leaderboard.list.Add(A);
        leaderboard.list.Add(A);
        leaderboard.list.Add(A);
        leaderboard.list.Add(A);
        SaveScores(leaderboard.list);*/
        /*if (!Directory.Exists(Application.persistentDataPath + "/HighScores/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/HighScores/");
        }*/

        /*HighScoreEntry A = new HighScoreEntry();
        A.score = 50;
        A.name = "AAC";
        SaveScores(A);*/
        if (!Directory.Exists(Application.dataPath + "/HighScores/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/HighScores/");
        }
    }

    public void SaveScores(HighScoreEntry scoreToSave)
    {
        leaderboard.list = LoadScores();
        for(int i = 0; i < 5; i++)
        {
            if(scoreToSave.score > leaderboard.list[i].score)
            {
                leaderboard.list.Insert(i, scoreToSave);
                leaderboard.list.RemoveAt(5);
                break;
            }
        }
        XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
        //FileStream stream = new FileStream(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Create);
        FileStream stream = new FileStream(Application.dataPath + "/HighScores/highscores.xml", FileMode.Create);
        serializer.Serialize(stream, leaderboard);
        stream.Close();
    }

    /*public void SaveScores(List<HighScoreEntry> scoresToSave)
    {
        leaderboard.list = scoresToSave;
        XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
        //FileStream stream = new FileStream(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Create);
        FileStream stream = new FileStream(Application.dataPath + "/HighScores/highscores.xml", FileMode.Create);
        serializer.Serialize(stream, leaderboard);
        stream.Close();
    }*/

    public List<HighScoreEntry> LoadScores()
    {
        if (File.Exists(Application.dataPath/*Application.persistentDataPath*/ + "/HighScores/highscores.xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
            //FileStream stream = new FileStream(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Open);
            FileStream stream = new FileStream(Application.dataPath + "/HighScores/highscores.xml", FileMode.Open);
            leaderboard = serializer.Deserialize(stream) as Leaderboard;
            stream.Close();

        }
        else
        {
            leaderboard.list.Clear();
            HighScoreEntry A = new HighScoreEntry();
            A.score = 0;
            A.name = "AAA";
            for (int i = 0; i < 5; i++)
            {
                leaderboard.list.Add(A);
            }
        }
        return leaderboard.list;
    }
}

[System.Serializable]
public class Leaderboard
{
    public List<HighScoreEntry> list = new List<HighScoreEntry>();
}

//[System.Serializable]
public class HighScoreEntry
{
    public string name;
    public int score;
}