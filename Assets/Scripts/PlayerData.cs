using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerData
{
    [DllImport("__Internal")]
    private static extern void UpdateLeaderboardScore(int value);
    
    public event Action<int> ScoreChanged;
    
    private int _currentScore;

    public PlayerData()
    {
        _currentScore = 0;
    }

    public void ChangeScore(int n)
    {
        if (n > 0)
        {
            _currentScore += n;
            ScoreChanged?.Invoke(_currentScore);
        }
        
        if (_currentScore > PlayerPrefs.GetInt("Score", 0))
        {
            PlayerPrefs.SetInt("Score", _currentScore);
            Save();
            #if UNITY_WEBGL && !UNITY_EDITOR
            UpdateLeaderboardScore(_currentScore);
            #endif
        }     
    }

    public void ResetScore()
    {
        _currentScore = 0;
        ScoreChanged?.Invoke(_currentScore);
    }

    public void Save()
    {
        if (_currentScore >= PlayerPrefs.GetInt("Score", 0))
            PlayerPrefs.Save();
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }

}
