using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private float size = 80f;

    private int _xPos;
    private int _yPos;
    private GameField _gameField;
    private BubbleType _type;
    private Bubble _bubble;
    private bool _isWideLine;
    
    public float Size => size;
    public int XPos => _xPos;
    public int YPos => _yPos;
    public BubbleType Type => _type;
    public bool IsInPack;
    public bool IsWideLine => _isWideLine;
    

    public void Initialize(GameField gameField, int xPos, int yPos, bool line)
    {
        _gameField = gameField;
        SetPos(xPos, yPos);
        _isWideLine = line;
    }

    public void SetPos(int xPos, int yPos)
    {
        _xPos = xPos;
        _yPos = yPos;
    }

    public void SetBubbleType(BubbleType type, Bubble bubble)
    {
        _type = type;
        _bubble = bubble;
    }

    public Cell GetNearCell()
    {
        return _gameField.GetNearCell(this);
    }

    public void OnHit(BubbleType type)
    {   
        _type = type;
        _gameField.CheckHit(this);
    }

    public void PutInPack()
    {
        IsInPack = true;
    }

    public void GetFromPack()
    {
        IsInPack = false;
    }

    public void DestroyBubble()
    {
        _bubble.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        _type = BubbleType.Empty;
    }

    public void PlayPopAnimation()
    {
        if (_bubble != null)
        {
            _bubble.transform.DOScale(new Vector3(1.3f, 1.3f, 1f), 0.3f);
            _bubble.gameObject.GetComponent<Image>().DOFade(0.1f, 0.5f).OnComplete(() => Destroy(_bubble.gameObject)).SetLink(_bubble.gameObject);
        }
    }

    public void FallBubble()
    {
        if (_bubble != null)
        {
            RectTransform rt = _bubble.gameObject.GetComponent<RectTransform>();
            float newPos = rt.anchoredPosition.x + Random.Range(-30, 30);
            rt.DOAnchorPos(new Vector2(newPos, rt.anchoredPosition.y + Random.Range(30, 50)), 0.2f).SetLink(_bubble.gameObject);
            rt.DOAnchorPos(new Vector2(newPos, -500), 0.4f).SetDelay(0.2f).OnComplete(() => Destroy(_bubble.gameObject)).SetLink(_bubble.gameObject);
            _bubble.gameObject.GetComponent<Image>().DOFade(0.1f, 0.2f).SetLink(_bubble.gameObject);
        }
    }
    
}
