using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowFullscreenAdv();
    
    [SerializeField] private CanvasGroup losePanel;
    [SerializeField] private YandexSDK yandexSDK;
    

    public CanvasGroup LosePanel => losePanel;

    public void OpenPanel(CanvasGroup panel)
    {
        panel.alpha = 0f;
        panel.gameObject.SetActive(true);
        panel.DOFade(1, 0.5f);
    }

    public void ClosePanel(CanvasGroup panel)
    {
        panel.DOFade(0, 0.5f).OnComplete(()=> panel.gameObject.SetActive(false));
    }

    public void ResetGame()
    {
        ShowFullscreenAdv();
        SceneManager.LoadScene(0);
    }
}
