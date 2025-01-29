using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowFullscreenAdv();

    private const bool Y_SDK_IS_ENABLED = YandexSDK.Y_SDK_IS_ENABLED;
    
    [SerializeField] private CanvasGroup _losePanel;
    
    public CanvasGroup LosePanel => _losePanel;

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
#if UNITY_WEBGL && !UNITY_EDITOR && Y_SDK_IS_ENABLED
        ShowFullscreenAdv();
#endif
        SceneManager.LoadScene(0);
    }
}
