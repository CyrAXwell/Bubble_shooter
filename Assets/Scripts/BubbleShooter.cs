using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BubbleShooter : MonoBehaviour
{
    [SerializeField] private BubbleSpawner bubbleSpawner;
    [SerializeField] private TrajectoryLine trajectoryLine;
    [SerializeField] private GameObject collPoint;
    [SerializeField] private GameObject rayPoint;
    [SerializeField] private GameField gameField;

    private Camera _cam;
    private Bubble _bubble;
    private Vector2 _direction;
    private bool _canShoot;
    private bool _isCursorInField;
    private AudioManager _audioManager;

    public bool CanShoot => _canShoot;

    private void Start()
    {
        _cam = Camera.main;
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }    

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = _cam.ScreenToWorldPoint(mousePos);
        _direction = mousePos - transform.position;

        
        var startPos = transform.position;
        startPos.z = 0;

        if (mousePos.y - transform.position.y < 0.5)
        {
            _isCursorInField = false;
            trajectoryLine.StopDisplay();
        }
        else
        {
            _isCursorInField = true;
            if (_canShoot)
                trajectoryLine.StartDisplay();
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_canShoot && _isCursorInField)
            {
                _audioManager.PlaySFX(_audioManager.ShootSound);
                _canShoot = false;
                trajectoryLine.StopDisplay();
                Sequence sequence = DOTween.Sequence();
                RayCastShoot(transform.position, _direction, sequence);
            }
        }
    }

    private void PhysicsShoot()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = _cam.ScreenToWorldPoint(mousePos);

        if (_canShoot && _isCursorInField)
        {
            _canShoot = false;
            trajectoryLine.StopDisplay();

            _direction.Normalize();

            _bubble.GetComponent<MovingBubble>().StartMove(_direction);
            //StartCoroutine(CreatBubbleWithDelay());

        }
    }

    private void RayCastShoot(Vector3 rayPos, Vector3 rayDir, Sequence sequence)
    {
        
        RaycastHit2D[] rayHit = Physics2D.RaycastAll(rayPos, rayDir);
        if (rayHit.Length > 0)
        {
            var results = new List<RaycastHit2D>(rayHit);

            var bubbleHit = results.Where(r => r.collider.gameObject.CompareTag("Bubble")).ToArray();
            var wallHit = results.Where(r => r.collider.gameObject.CompareTag("Wall")).ToArray();

            bool isWallHit = false;

            if (bubbleHit.Count() > 0 && wallHit.Count() > 0)
            {
                if (bubbleHit[0].distance > wallHit[0].distance)
                    isWallHit = true;
            }
            else if (wallHit.Count() > 0)
            {
                isWallHit = true;
            }

            if (!isWallHit)
            {
                rayPos = bubbleHit[0].point - 35f/108f *  (Vector2)rayDir.normalized;

                Cell currentCell = GetCell(rayPos);
            
                if (currentCell == null)
                {
                    if (bubbleHit[0].collider.name != "Top Wall")
                    {
                        currentCell = bubbleHit[0].collider.gameObject.GetComponent<Bubble>().GetNearCell();
                        if (currentCell.Type != BubbleType.Empty)
                            currentCell = currentCell.GetNearCell();
                    }
                    else
                    {
                        currentCell = gameField.GetEmptyCell(bubbleHit[0].point.x);
                    }
                    
                }
                sequence.Append(_bubble.transform.DOMove(rayPos, bubbleHit[0].distance / 15).SetEase(Ease.Linear));

                sequence.OnComplete(()=>{
                    Destroy(_bubble.gameObject);
                    currentCell.OnHit(_bubble.Type);
                });
                
            }
            else
            {
                if (wallHit.Count() > 0)
                {
                    float radius = 35f/108f;
                    float x = radius / Mathf.Sin(Mathf.Deg2Rad * (90 - Vector2.Angle(-rayDir, wallHit[0].normal)));

                    rayPos = wallHit[0].point - x *  (Vector2)rayDir.normalized;
                    rayDir = Vector3.Reflect(rayDir, wallHit[0].normal);
                    sequence.Append(_bubble.transform.DOMove(rayPos, wallHit[0].distance / 15).SetEase(Ease.Linear)); 
                    
                    RayCastShoot(rayPos, rayDir, sequence);
                }
            }
        }
    }

    private Cell GetCell(Vector2 point)
    {
        var eventData = new PointerEventData(EventSystem.current);
        

        float deltaX = Screen.width / 2;
        float deltaY = Screen.height / 2;
        point.x = deltaX + point.x * Screen.height / 10;
        point.y = deltaY + point.y * Screen.height / 10;
        
        eventData.position = point;

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);


        var hit = results.Where(r => r.gameObject.layer == 5).Where(r => r.gameObject.CompareTag("Cell")).ToArray();
        if(hit.Count() > 0)
        {
            if (hit[0].gameObject.CompareTag("Cell"))
            {
                return hit[0].gameObject.GetComponent<Cell>();
            }
        }

        return null;
    }

    private IEnumerator CreatBubbleWithDelay()
    {
        yield return new WaitForSeconds(0.8f);
        _bubble = bubbleSpawner.CreateNewBubble(true);
    }

    public void SetCanShoot()
    {
        _canShoot = true;
        trajectoryLine.StartDisplay();
    }

    public void StopShoot()
    {
        _canShoot = false;
        trajectoryLine.StopDisplay();
    }

    public void SetBubble(Bubble bubble)
    {
        _bubble = bubble;
    }

    public BubbleType Swap(BubbleType type)
    {
        _audioManager.PlaySFX(_audioManager.SwapSound);

        _canShoot = false;
        BubbleType tempBubbleType = _bubble.Type;
        _bubble.SetSprite(type, null);
        _bubble.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
        _bubble.gameObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.2f);
        _bubble.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetDelay(0.15f).OnComplete(()=> _canShoot = true);

        return tempBubbleType;
    }
    
}
