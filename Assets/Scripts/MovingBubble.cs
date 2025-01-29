using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovingBubble : MonoBehaviour
{
    [SerializeField] private float speed;

    private RectTransform _rt;
    private Rigidbody2D _rb;
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
            _rt.anchoredPosition = _rt.anchoredPosition + _direction * speed * Time.deltaTime;
        }
    }

    public void StartMove(Vector2 target)
    {
        _direction = target;
        _canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Wall"))
        {
            _direction.x *= -1;
        }

        if (other.collider.CompareTag("Bubble") && !_isPlacement)
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
}
