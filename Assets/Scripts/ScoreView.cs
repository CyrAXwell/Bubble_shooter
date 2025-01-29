using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreDisplay;
    [SerializeField] private TMP_Text highscoreDisplay;
    [SerializeField] private float countDuration;

    private PlayerData _playerData;
    private float _current;
    private int _target;
    private Coroutine _countScore;
    private string _lang = "en";
    private float _highScore;

    public void Initialize(PlayerData playerData)
    {
        _playerData = playerData;
        playerData.ScoreChanged += OnScoreChange;
        _current = 0;
        DisplayScore(); 
        _highScore = PlayerPrefs.GetInt("Score", 0);
        DisplayHighscore(); 
    }

    private void DisplayScore()
    {
        switch(_lang)
        {
            case "ru":
                scoreDisplay.text = "—чет:\n" + ((int)_current).ToString();
                break;
            case "en":
                scoreDisplay.text = "Score:\n" + ((int)_current).ToString();
                break;
            default:
                scoreDisplay.text = "Score:\n" + ((int)_current).ToString();
                break;   
        }  
    }

    private void DisplayHighscore()
    {
        switch(_lang)
        {
            case "ru":
                highscoreDisplay.text = "–екорд:\n" + _highScore;
                break;
            case "en":
                highscoreDisplay.text = "High Score:\n" + _highScore;
                break;
            default:
                highscoreDisplay.text = "High Score:\n" + _highScore;
                break;   
        }  
        
    }

    public void Updatelang(string lang)
    {
        _lang = lang;
        DisplayScore();
        DisplayHighscore();

    }

    public void OnScoreChange(int value)
    {
        if (value == 0)
        {
            _current = 0;
            DisplayScore(); 
        }
        else
        {
            _target = value;

            if (_countScore != null)
                StopCoroutine(_countScore);
            
            _countScore = StartCoroutine(CountTo(_target));
        }
    }

    private IEnumerator CountTo(int target)
    {

        float rate = Mathf.Abs(target - _current) / countDuration;

        while(_current != target)
        {
            _current = Mathf.MoveTowards(_current, target, rate * Time.deltaTime);
            DisplayScore(); 
            yield return null;
        }
    }

    private void OnDestroy() => _playerData.ScoreChanged -= OnScoreChange;
}
