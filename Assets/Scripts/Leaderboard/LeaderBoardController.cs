using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Controller.Leaderboard
{
    public class LeaderBoardController: MonoBehaviour
    {
        private const string FilePath = "Leaderboard.json";
        private const string HighScoreText = "HightScore";

        private static List<LeaderBoardData> _leaderBoardList;
        private static int _lastScore;
        private static TimeSpan _lastTime;
        private static int _highScore;

        public static List<LeaderBoardData> Leaderboard
        {
            get
            {
                if(_leaderBoardList == null) ReadJson();
                return _leaderBoardList.OrderByDescending((data) => data.Score).ToList();
            }
        }

        public static int HighScore
        {
            get
            {
                if (_highScore != default) return _highScore;
                _highScore = PlayerPrefs.GetInt(HighScoreText);
                return _highScore;
            }
            set
            {
                _highScore = value;
                PlayerPrefs.SetInt(HighScoreText, _highScore);
            }
        }

        public static int LastScore => _lastScore;
        public static TimeSpan LastTime => _lastTime;

        private void Awake()
        {
            ReadJson();
            DontDestroyOnLoad(this);
        }

        public static bool HasNewRecord => _lastScore > HighScore;

        public static void RegisterScore(int score, TimeSpan time)
        {
            _lastScore = score;
            _lastTime = time;
        }

        public static void SaveToLeaderboard(string name)
        {
            var data = new LeaderBoardData()
            {
                Name = name,
                Score = _lastScore
            };
            HighScore = _lastScore;
            _leaderBoardList.Add(data);
            WriteToFile();
        }

        private static void WriteToFile()
        {
            var json =  JsonConvert.SerializeObject(_leaderBoardList);
            File.WriteAllText(FilePath, json);
        }

        private static void ReadJson()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                try
                {
                    _leaderBoardList = JsonConvert.DeserializeObject<List<LeaderBoardData>>(json);
                    _highScore = _leaderBoardList.Max(data => data.Score);
                    PlayerPrefs.SetInt(HighScoreText, _highScore);
                }
                catch
                {
                    _leaderBoardList = new List<LeaderBoardData>();
                }
            }
            else
            {
                File.Create(FilePath);
                _leaderBoardList = new List<LeaderBoardData>();
            }
            
        }
    }
    
    [Serializable]
    public struct LeaderBoardData
    {
        public string Name;
        public int Score;
    }
}