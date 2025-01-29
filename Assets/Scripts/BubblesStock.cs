using DG.Tweening;
using UnityEngine;

public class BubblesStock : MonoBehaviour
{
    [SerializeField] private Transform _rotateIcon;
    [SerializeField] private float _roateSpeed = 45;
    [SerializeField] private Bubble _stockBubble;
    [SerializeField] private BubbleShooter _bubbleShooter;

    private void Update()
    {
        _rotateIcon.Rotate(new Vector3(0f, 0f, _roateSpeed) * Time.deltaTime);
    }

    public void SetBubble()
    {   
        _stockBubble.SetSprite((BubbleType)Random.Range(1, 6), null);
    }

    public void OnSwapButton()
    {
        if (_bubbleShooter.CanShoot)
        {
            _stockBubble.SetSprite(_bubbleShooter.Swap(_stockBubble.Type), null);
            _stockBubble.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            _stockBubble.gameObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.2f);
            _stockBubble.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetDelay(0.2f);
        }
    }

}
