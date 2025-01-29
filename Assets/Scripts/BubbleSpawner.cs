using DG.Tweening;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] private BubbleShooter _bubbleShooter;
    [SerializeField] private BubblesStock _bubbleStock;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Bubble _bubblePrefab;
    
    public void Initialize()
    {
        SpawnBubble(true);
        _bubbleStock.SetBubble();
    }

    public Bubble CreateNewBubble(bool canShoot)
    {
        Bubble bubble = Instantiate(_bubblePrefab, _spawnPoint, false);
        int bubblesTypesAmount = 5;
        bubble.SetSprite((BubbleType) UnityEngine.Random.Range(1, bubblesTypesAmount + 1), null);

        bubble.transform.SetParent(transform.parent.transform);
        bubble.transform.localScale = new Vector3(0,0,0);
        if (canShoot)
            bubble.transform.DOScale(new Vector3(1,1,0), 0.2f).OnComplete(_bubbleShooter.SetCanShoot);
        else
            bubble.transform.DOScale(new Vector3(1,1,0), 0.2f).OnComplete(_bubbleShooter.StopShoot);
        return bubble;
    }

    public void SpawnBubble(bool canShoot)
    { 
        _bubbleShooter.SetBubble(CreateNewBubble(canShoot));
    }

    public void StopShoot()
    { 
        _bubbleShooter.StopShoot();
    }
}
