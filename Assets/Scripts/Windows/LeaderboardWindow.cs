using System;
using Controller.Leaderboard;
using UnityEngine;

public class LeaderboardWindow : UIWidget
{
    [SerializeField] private Transform _content;
    [SerializeField] private LeaderBoardItem _itemPrefab;

    private void OnEnable()
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        for (var index = 0; index < LeaderBoardController.Leaderboard.Count; index++)
        {
            LeaderBoardItem item;
            item = _content.childCount > index ? _content.GetChild(index).GetComponent<LeaderBoardItem>() : Instantiate(_itemPrefab, _content);
            var leaderBoardData = LeaderBoardController.Leaderboard[index];
            item.SetData(index + 1, leaderBoardData);
        }
    }
}
