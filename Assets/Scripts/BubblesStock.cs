using DG.Tweening;
using UnityEngine;

public class BubblesStock : MonoBehaviour
{
    [SerializeField] private Transform rotateIcon;
    [SerializeField] private float roateSpeed;
    [SerializeField] private Bubble stockBubble;
    [SerializeField] private BubbleShooter bubbleShooter;

    private void Update()
    {
        rotateIcon.Rotate(new Vector3(0f, 0f, roateSpeed) * Time.deltaTime);
    }

    public void SetBubble()
    {   
        stockBubble.SetSprite((BubbleType)Random.Range(1, 6), null);
    }

    public void OnSwapButton()
    {
        if (bubbleShooter.CanShoot)
        {
            stockBubble.SetSprite(bubbleShooter.Swap(stockBubble.Type), null);
            stockBubble.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            stockBubble.gameObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.2f);
            stockBubble.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetDelay(0.2f);
        }
    }

}
