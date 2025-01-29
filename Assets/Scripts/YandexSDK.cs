using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class YandexSDK : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern string GetLang();

    [SerializeField] private ScoreView scoreView;
    [SerializeField] private TMP_Text loseText;

    public static YandexSDK instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            SetLang();
        } 
    }

    // private void Start()
    // {
    //     SetLang("en");
    // }

    public void SetLang()
    {
        var lang = GetLang();
        
        //Debug.Log("setlang");
        scoreView.Updatelang(lang);
        switch (lang)
        {
            case "ru":
                loseText.text = "Поражение";
                break;
            case "en":
                loseText.text = "Game Over";
                break;
            default:
                loseText.text = "Game Over";
                break;
        }
        Debug.Log("setlang");
    }
}
