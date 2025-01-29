using UnityEngine;

[CreateAssetMenu(fileName = "BubbleSprite", menuName = "ScriptableObjects/BubbleSprite")]
public class BubbleSprite : ScriptableObject
{
    [SerializeField] private Sprite redBubble;
    [SerializeField] private Sprite blueBubble;
    [SerializeField] private Sprite greenBubble;
    [SerializeField] private Sprite yelowBubble;
    [SerializeField] private Sprite purpleBubble;

    public Sprite RedBubble => redBubble;
    public Sprite BlueBubble => blueBubble;
    public Sprite GreenBubble => greenBubble;
    public Sprite YelowBubble => yelowBubble;
    public Sprite PurpleBubble => purpleBubble;

}
