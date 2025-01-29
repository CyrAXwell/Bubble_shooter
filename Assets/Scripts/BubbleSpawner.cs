using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] private BubbleShooter bubbleShooter;
    [SerializeField] private BubblesStock bubbleStock;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Bubble bubblePrefab;
    
    public void Initialize()
    {
        SpawnBubble(true);
        bubbleStock.SetBubble();
    }

    public Bubble CreateNewBubble(bool canShoot)
    {
        Bubble bubble = Instantiate(bubblePrefab, spawnPoint, false);
        bubble.SetSprite((BubbleType) UnityEngine.Random.Range(1, 6), null);

        bubble.transform.SetParent(transform.parent.transform);
        bubble.transform.localScale = new Vector3(0,0,0);
        if (canShoot)
            bubble.transform.DOScale(new Vector3(1,1,0), 0.2f).OnComplete(bubbleShooter.SetCanShoot);
        else
            bubble.transform.DOScale(new Vector3(1,1,0), 0.2f).OnComplete(bubbleShooter.StopShoot);
        return bubble;
    }

    public void SpawnBubble(bool canShoot)
    { 
        bubbleShooter.SetBubble(CreateNewBubble(canShoot));
    }

    public void StopShoot()
    { 
        bubbleShooter.StopShoot();
    }

    public void StartShoot()
    { 
        //bubbleShooter.SetBubble(CreateNewBubble());
    }
}
