using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerData
{
    [DllImport("__Internal")]
    private static extern void UpdateLeaderboardScore(int value);

    public const string RU_LANG = "ru";
    public const string EN_LANG = "en";

    private const bool Y_SDK_IS_ENABLED = YandexSDK.Y_SDK_IS_ENABLED;
    
    public event Action<int> ScoreChanged;
    
    private int _currentScore;
    private string _lang;

    public PlayerData()
    {
        _currentScore = 0;
        _lang = EN_LANG;
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
#if UNITY_WEBGL && !UNITY_EDITOR && Y_SDK_IS_ENABLED
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

    public string GetLang()
    {
        return _lang;
    }

    public void ChangeLang(string lang)
    {
        _lang = lang;
    }
}
