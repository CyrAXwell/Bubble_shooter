using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class YandexSDK : MonoBehaviour
{
    public const bool Y_SDK_IS_ENABLED = false;

    [DllImport("__Internal")]
    private static extern string GetLang();

    [SerializeField] private ScoreView _scoreView;
    [SerializeField] private TMP_Text _loseText;

    private PlayerData _playerData;

    public void Initialize(PlayerData playerData)
    {
        _playerData = playerData;
        SetLang();
    }

    public void SetLang()
    {
        string lang = _playerData.GetLang();
#if UNITY_WEBGL && !UNITY_EDITOR && Y_SDK_IS_ENABLED
        lang = GetLang();
        _playerData.ChangeLang(lang);
        scoreView.Updatelang(lang);
#endif
        
        switch (lang)
        {
            case PlayerData.RU_LANG:
                _loseText.text = "Поражение";
                break;
            case PlayerData.EN_LANG:
                _loseText.text = "Game Over";
                break;
            default:
                _loseText.text = "Game Over";
                break;
        }
    }
}
