using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] public TMP_Text[] scoreText;
    [SerializeField] public TMP_Text[] nameText;

    private void Start()
    {
        List<HighScoreEntry> Scores = XMLManager.Instance.LoadScores();
        for(int i = 0; i < 5; i++)
        {
            if (Scores[i] != null)
            {
                nameText[i].text = Scores[i].name;
                scoreText[i].text = Scores[i].score.ToString();
            }
        }
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
