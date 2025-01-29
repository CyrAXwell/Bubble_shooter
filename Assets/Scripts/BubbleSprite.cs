using UnityEngine;

[CreateAssetMenu(fileName = "BubbleSprite", menuName = "ScriptableObjects/BubbleSprite")]
public class BubbleSprite : ScriptableObject
{
    [SerializeField] private Sprite _redBubble;
    [SerializeField] private Sprite _blueBubble;
    [SerializeField] private Sprite _greenBubble;
    [SerializeField] private Sprite _yelowBubble;
    [SerializeField] private Sprite _purpleBubble;

    public Sprite RedBubble => _redBubble;
    public Sprite BlueBubble => _blueBubble;
    public Sprite GreenBubble => _greenBubble;
    public Sprite YelowBubble => _yelowBubble;
    public Sprite PurpleBubble => _purpleBubble;

}
