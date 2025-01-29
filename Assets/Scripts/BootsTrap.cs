using UnityEngine;

public class BootsTrap : MonoBehaviour
{
    [SerializeField] private ScoreView scoreView;
    [SerializeField] private GameField gameField;
    [SerializeField] private BubbleSpawner bubbleSpawner;

    private PlayerData _playerData;

    private void Awake()
    {
        PlayerDataInit();
        GameFieldInit();
        ScoreViewInit();
    }

    private void PlayerDataInit()
    {
        _playerData = new PlayerData();
    }

    private void GameFieldInit()
    {
        gameField.Initialize(_playerData);
    }

    private void ScoreViewInit()
    {
        scoreView.Initialize(_playerData);
    }


}
