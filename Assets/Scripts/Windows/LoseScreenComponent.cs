using Controller.Leaderboard;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreenComponent : UIWidget
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Transform _recordWindow;
    [SerializeField] private TMP_InputField _inputField;
    
    private void Awake()
    {
        _recordWindow.gameObject.SetActive(LeaderBoardController.HasNewRecord);
        _scoreText.text = LeaderBoardController.LastScore.ToString();
        var time = LeaderBoardController.LastTime;
        _timeText.text =$"{time.Minutes:D2}:{time.Seconds:D2}";
    }

    public void OnOkClick()
    {
        var name = _inputField.text;
        LeaderBoardController.SaveToLeaderboard(name);
        _recordWindow.gameObject.SetActive(false);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
