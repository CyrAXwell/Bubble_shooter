using UnityEngine;

public class BootsTrap : MonoBehaviour
{
    [SerializeField] private ScoreView _scoreView;
    [SerializeField] private GameField _gameField;
    [SerializeField] private BubbleSpawner _bubbleSpawner;
    [SerializeField] private YandexSDK _yandexSDK;

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
        _gameField.Initialize(_playerData);
    }

    private void ScoreViewInit()
    {
        _yandexSDK.Initialize(_playerData);
        _scoreView.Initialize(_playerData);
    }
}
