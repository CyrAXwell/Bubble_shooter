using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    [SerializeField] private BubbleSprite bubbleSprite;
    [SerializeField] private Image image;

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
                image.sprite = bubbleSprite.RedBubble;
                break;
            case BubbleType.Blue :
                image.sprite = bubbleSprite.BlueBubble;
                break;
            case BubbleType.Green :
                image.sprite = bubbleSprite.GreenBubble;
                break;
            case BubbleType.Yellow :
                image.sprite = bubbleSprite.YelowBubble;
                break;
            case BubbleType.Purple :
                image.sprite = bubbleSprite.PurpleBubble;
                break;
            default :
                image.sprite = bubbleSprite.RedBubble;
                break;
        }
    }

    public Cell GetNearCell()
    {
        return _cell.GetNearCell();
    }

}
