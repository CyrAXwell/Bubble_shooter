using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    

    [SerializeField] private TMP_Text _scoreDisplay;
    [SerializeField] private TMP_Text _highscoreDisplay;
    [SerializeField] private float _countDuration = 0.5f;

    private PlayerData _playerData;
    private float _current;
    private int _target;
    private Coroutine _countScore;
    private string _lang;
    private float _highScore;

    public void Initialize(PlayerData playerData)
    {

        _playerData = playerData;
        playerData.ScoreChanged += OnScoreChange;
        _current = 0;
        _lang = _playerData.GetLang();

        DisplayScore(); 
        _highScore = PlayerPrefs.GetInt("Score", 0);
        DisplayHighscore(); 
    }

    private void DisplayScore()
    {
        switch(_lang)
        {
            case PlayerData.RU_LANG:
                _scoreDisplay.text = "—чет:\n" + ((int)_current).ToString();
                break;
            case PlayerData.EN_LANG:
                _scoreDisplay.text = "Score:\n" + ((int)_current).ToString();
                break;
            default:
                _scoreDisplay.text = "Score:\n" + ((int)_current).ToString();
                break;   
        }  
    }

    private void DisplayHighscore()
    {
        switch(_lang)
        {
            case PlayerData.RU_LANG:
                _highscoreDisplay.text = "–екорд:\n" + _highScore;
                break;
            case PlayerData.EN_LANG:
                _highscoreDisplay.text = "High Score:\n" + _highScore;
                break;
            default:
                _highscoreDisplay.text = "High Score:\n" + _highScore;
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

        float rate = Mathf.Abs(target - _current) / _countDuration;

        while(_current != target)
        {
            _current = Mathf.MoveTowards(_current, target, rate * Time.deltaTime);
            DisplayScore(); 
            yield return null;
        }
    }

    private void OnDestroy() => _playerData.ScoreChanged -= OnScoreChange;
}
