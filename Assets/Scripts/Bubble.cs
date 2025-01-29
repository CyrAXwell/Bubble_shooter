using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    public const string BUBBLE_GAME_OBJECT_TAG = "Bubble";

    [SerializeField] private BubbleSprite _bubbleSprite;
    [SerializeField] private Image _image;

    private BubbleType _type;
    private Cell _cell;

    public BubbleType Type => _type;

    public void SetSprite(BubbleType type, Cell cell)
    {
        _type = type;
        _cell = cell;

        switch(_type)
        {
            case BubbleType.Red :
                _image.sprite = _bubbleSprite.RedBubble;
                break;
            case BubbleType.Blue :
                _image.sprite = _bubbleSprite.BlueBubble;
                break;
            case BubbleType.Green :
                _image.sprite = _bubbleSprite.GreenBubble;
                break;
            case BubbleType.Yellow :
                _image.sprite = _bubbleSprite.YelowBubble;
                break;
            case BubbleType.Purple :
                _image.sprite = _bubbleSprite.PurpleBubble;
                break;
            default :
                _image.sprite = _bubbleSprite.RedBubble;
                break;
        }
    }

    public Cell GetNearCell()
    {
        return _cell.GetNearCell();
    }

}
