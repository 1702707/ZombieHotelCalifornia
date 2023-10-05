using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Controller.Leaderboard
{
    public class LeaderBoardController: MonoBehaviour
    {
        private const string HighScore = "HighScore";
        private const string LastScore = "LastScore";
        private const string FilePath = "Leaderboard.json";

        private static List<LeaderBoardData> _leaderBoardList = new List<LeaderBoardData>();

        public static List<LeaderBoardData> Leaderboard
        {
            get
            {
                return _leaderBoardList.OrderByDescending((data) => data.Score).ToList();
            }
        }

        private void Awake()
        {
            ReadJson();
        }

        public static bool IsNewRecord
        {
            get
            {
                var highScore = PlayerPrefs.GetInt(HighScore);
                var lastScore = PlayerPrefs.GetInt(LastScore);
                return lastScore > highScore;
            }
        }

        public static void SaveLastScore(int score)
        {
            PlayerPrefs.SetInt(LastScore, score);
        }

        public void SaveToLeaderboard(LeaderBoardData data)
        {
            PlayerPrefs.SetInt(HighScore, data.Score);
            _leaderBoardList.Add(data);
            WriteToFile();
        }

        private static void WriteToFile()
        {
            var json = JsonUtility.ToJson(_leaderBoardList);
            File.WriteAllTextAsync(FilePath, json);
        }

        private static void ReadJson()
        {
            var json = File.ReadAllText(FilePath);
            _leaderBoardList = JsonUtility.FromJson<List<LeaderBoardData>>(json);
        }
    }
    
    [Serializable]
    public struct LeaderBoardData
    {
        public string Name;
        public int Score;
    }
}