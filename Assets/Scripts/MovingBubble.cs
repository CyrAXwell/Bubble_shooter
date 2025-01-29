using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovingBubble : MonoBehaviour
{
    [SerializeField] private float _speed = 600;

    private RectTransform _rt;

    private bool _canMove;
    private bool _isPlacement;
    private Vector2 _direction;
    
    private void Start()
    {
        _rt = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_canMove)
        {
            _rt.anchoredPosition = _rt.anchoredPosition + _direction * _speed * Time.deltaTime;
        }
    }

    public void StartMove(Vector2 target)
    {
        _direction = target;
        _canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag(BubbleShooter.WALL_GAME_OBJECT_TAG))
        {
            _direction.x *= -1;
        }

        if (other.collider.CompareTag(Bubble.BUBBLE_GAME_OBJECT_TAG) && !_isPlacement)
        {
            _isPlacement = true;
            Destroy(gameObject);
            _canMove = false;

            Cell currentCell = GetCell();
            
            if (currentCell == null)
            {
                currentCell = other.gameObject.GetComponent<Bubble>().GetNearCell();
            }

            currentCell.OnHit(gameObject.GetComponent<Bubble>().Type);
        }
    }

    private Cell GetCell()
    {
        var eventData = new PointerEventData(EventSystem.current);
        Vector3 pos = _rt.anchoredPosition;

        float deltaX = Screen.width / 2;
        float deltaY = Screen.height / 2;
        pos.x += deltaX;
        pos.y += deltaY;
        
        eventData.position = pos;

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);


        var hit = results.Where(r => r.gameObject.layer == 5).Where(r => r.gameObject.CompareTag(Cell.CELL_GAME_OBJECT_TAG)).ToArray();
        if(hit.Count() > 0)
        {
            if (hit[0].gameObject.CompareTag(Cell.CELL_GAME_OBJECT_TAG))
            {
                return hit[0].gameObject.GetComponent<Cell>();
            }
        }

        return null;
    }
}
