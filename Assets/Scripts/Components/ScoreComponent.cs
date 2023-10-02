using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreComponent : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Animator _animator;
    private int _newScore;

    public void SetScore(int newScore)
    {
        _newScore = newScore;
        _animator.Play("AddScore");
    }

    public void ChangeText()
    {
        _scoreText.text = _newScore.ToString("000000000");
    }
}
