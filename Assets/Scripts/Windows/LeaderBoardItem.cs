using Controller.Leaderboard;
using TMPro;
using UnityEngine;

public class LeaderBoardItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _idText;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _scoreText;

    public void SetData(int index, LeaderBoardData data)
    {
        _idText.text = index.ToString();
        _nameText.text = data.Name;
        _scoreText.text = data.Score.ToString("000000000");
    }
}
