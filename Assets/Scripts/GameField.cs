using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameField : MonoBehaviour
{
    [SerializeField] private int _width = 12;
    [SerializeField] private int _hieght = 5;
    [SerializeField] private int _maxHieght = 13;
    [SerializeField] private int _shootAmountToShiftDown = 5;
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Bubble _bubblePrefab;
    [SerializeField] private RectTransform _leftWall;
    [SerializeField] private RectTransform _rightWall;
    [SerializeField] private RectTransform _topWall;
    [SerializeField] private BubbleSpawner _spawner;
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _spawnerPanel;
    [SerializeField] private RectTransform _borderLine;
    [SerializeField] private RectTransform _scoreDisplay;
    [SerializeField] private UIManager _uIManager;

    private List<List<Cell>> _cells = new List<List<Cell>>();
    private RectTransform _rt;
    private float _vertivalSpacing;
    private float _fieldWidth;
    private float _fieldHieght;
    private float _maxFieldHieght;
    private float _startX;
    private float _startY;
    private int _shootCounter;
    private int _shiftCounter;
    private int _wave;
    private float _shootDelay;
    private PlayerData _playerData;
    private AudioManager _audioManager;

    public void Initialize(PlayerData playerData)
    {
        _playerData = playerData;
        _rt = GetComponent<RectTransform>();
        _wave = 0;
        CreateField();
        _spawner.Initialize();
        _audioManager = GameObject.FindGameObjectWithTag(AudioManager.AUDIO_MANAGER_GAME_OBJECT_TAG).GetComponent<AudioManager>();
    }

    private void CreateField()
    {
        float referenceScreenHieght = 1080;

        _vertivalSpacing = Mathf.Sqrt(3) * _cellPrefab.Size / 2;
        _fieldWidth = _width * _cellPrefab.Size;
        _fieldHieght = _hieght * _vertivalSpacing + _cellPrefab.Size - _vertivalSpacing;
        _maxFieldHieght = _maxHieght * _vertivalSpacing + _cellPrefab.Size - _vertivalSpacing;

        _leftWall.anchoredPosition = new Vector2(- _fieldWidth / 2, 0f);
        _rightWall.anchoredPosition = new Vector2(_fieldWidth / 2, 0f);
        _topWall.anchoredPosition = new Vector2(0f, _rt.anchoredPosition.y);
        _borderLine.anchoredPosition = new Vector2(0f, _rt.anchoredPosition.y - _maxFieldHieght + _cellPrefab.Size);
        _spawnerPanel.sizeDelta = new Vector2(_fieldWidth, referenceScreenHieght + _borderLine.anchoredPosition.y);

        _scoreDisplay.sizeDelta = new Vector2(_fieldWidth, 100);
        _borderLine.sizeDelta = new Vector2(_fieldWidth, 10);
        _rt.sizeDelta = new Vector2(_fieldWidth, _fieldHieght);
        _background.sizeDelta = new Vector2(_fieldWidth, referenceScreenHieght);

        FillField();
    }

    private void FillField()
    {
        _startX = - (_fieldWidth / 2) + (_cellPrefab.Size / 2);
        _startY = - (_cellPrefab.Size / 2);

        for (int i = 0; i < _maxHieght; i++)
        {
            _cells.Add(new List<Cell>());

            float offsetX = (i + _wave) % 2 == 0 ? 0 : _cellPrefab.Size / 2;
            int lineWidth = (i + _wave) % 2 == 0 ? _width : _width - 1;

            for (int j = 0; j < lineWidth; j++)
            {
                Cell cell = Instantiate(_cellPrefab, transform, false);
                cell.transform.localPosition = new Vector2(_startX + offsetX + j * _cellPrefab.Size, _startY - i * _vertivalSpacing);

                if (i < _hieght)
                    CreateRandomBubble(cell);
                else
                    CreateEmptyCell(cell);

                cell.Initialize(this, j, i, _width > lineWidth);
                _cells[i].Add(cell);
            }
        }
    }


    public void CreateRandomBubble(Cell cell)
    {
        Bubble bubble = Instantiate(_bubblePrefab, cell.transform, false);
        BubbleType type = (BubbleType) UnityEngine.Random.Range(1, 6);
        bubble.SetSprite(type, cell);
        cell.SetBubbleType(type, bubble);
    }

    public void CreateBubble(Cell cell, BubbleType type)
    {
        Bubble bubble = Instantiate(_bubblePrefab, cell.transform, false);
        bubble.SetSprite(type, cell);

        cell.SetBubbleType(type, bubble);
    }

    private void CreateEmptyCell(Cell cell)
    {
        cell.SetBubbleType(BubbleType.Empty, null);
    }

    public Cell GetNearCell(Cell cell)
    {
        if (!cell.IsWideLine)
        {   
            if (cell.XPos == 0)
                return _cells[cell.YPos + 1][0];
            else
                return _cells[cell.YPos + 1][cell.XPos - 1];
        }
        else
        {
            if (cell.XPos == 0)
                return _cells[cell.YPos + 1][0];
            else
                return _cells[cell.YPos + 1][cell.XPos + 1];
        }
        
    }

    public Cell GetEmptyCell(float pos)
    {
        if (pos > 0)
        {
            if (_cells[0][_cells[0].Count-1].Type == BubbleType.Empty)
                return _cells[0][_cells[0].Count-1];
            else
                return _cells[1][_cells[1].Count-1];
        }
        else
        {
            if (_cells[0][0].Type == BubbleType.Empty)
                return _cells[0][0];
            else
                return _cells[1][0];
        }
        
    }

    private IEnumerator StartSpawnNewBubble(float delay, bool canShoot)
    {
        yield return new WaitForSeconds(delay);
        _spawner.SpawnBubble(canShoot);
    }

    public void CheckHit(Cell cell)
    {
        _shootDelay = 0f;
        CreateBubble(cell, cell.Type);

        List<Cell> bubblesPcak = new List<Cell>();

        CheckNeighbour(cell, bubblesPcak, true);
        
        ClearPack(bubblesPcak);
        
        if (bubblesPcak.Count > 2)
        {
            
            _shootDelay = 0f; 
            DestroyBubblesInPack(bubblesPcak);
            int fallenBubbles = CheckField(bubblesPcak.Count);
            _playerData.ChangeScore(bubblesPcak.Count * 10 + fallenBubbles * 30);
            _shootDelay += (bubblesPcak.Count + fallenBubbles) * 0.05f;
        }

        _shootCounter ++;
        if (_shootCounter == _shootAmountToShiftDown)
        {
            _shiftCounter ++;
            if (_shiftCounter == 5)
                _shootAmountToShiftDown --;
            else if (_shiftCounter == 10)
                _shootAmountToShiftDown --;

            _shootCounter = 0;
            ShiftDown();
            _shootDelay += 0.3f;
        }
        CheckLevelBoundary();
        
    }  

    private int CheckField(int bubbles)
    {
        int fallBubbles = 0;
        List<Cell> bubblesPcak = new List<Cell>();
        
        for (int j = 0; j < _cells[0].Count; j++)
        {
            if (_cells[0][j].Type != BubbleType.Empty)
                CheckNeighbour(_cells[0][j], bubblesPcak, false);

        }

        for (int i = 0; i < _cells.Count; i++)
        {
            for (int j = 0; j < _cells[i].Count; j++)
            {
                if (!_cells[i][j].IsInPack && _cells[i][j].Type != BubbleType.Empty)
                {
                    StartCoroutine(DestroyBubbleWithDelay(_cells[i][j], (bubbles + fallBubbles) * 0.05f));
                    bubbles ++;
                }
            }
        }
        ClearPack(bubblesPcak);

        return bubbles;
    }

    private void ShiftDown()
    {
        _wave ++;
        List<List<Cell>> tempCells = new List<List<Cell>>();

        for (int i = 0; i < _maxHieght; i++)
        {
            if (i == 0)
                tempCells.Add(new List<Cell>());
            else
                tempCells.Add(_cells[i - 1]);
                
            float offsetX = (i + _wave) % 2 == 0 ? 0 : _cellPrefab.Size / 2;
            int lineWidth = (i + _wave) % 2 == 0 ? _width : _width - 1;

            for (int j = 0; j < lineWidth; j++)
            {
                Cell cell;
                if (i == 0)
                {
                    cell = Instantiate(_cellPrefab, transform, false);
                    CreateRandomBubble(cell);
                    tempCells[i].Add(cell);
                }
                else
                {
                    cell = tempCells[i][j];
                }

                if(i == _maxHieght)
                {
                    Destroy(cell.gameObject);
                }
                else
                {
                    cell.transform.localPosition = new Vector2(_startX + offsetX + j * _cellPrefab.Size,_vertivalSpacing + _startY - i * _vertivalSpacing);
                    cell.transform.DOLocalMove(new Vector2(_startX + offsetX + j * _cellPrefab.Size, _startY - i * _vertivalSpacing), 0.3f).SetLink(gameObject);
                    cell.Initialize(this, j, i, _width > lineWidth);
                }     
            }
        }
        foreach (var cell in _cells[_maxHieght - 1])
            Destroy(cell.gameObject);
        
        _cells = new List<List<Cell>>(tempCells);

        tempCells.Clear();
    }

    private void CheckLevelBoundary()
    {
        foreach (var cell in _cells[_maxHieght - 1])
            if (cell.Type != BubbleType.Empty)
            {
                StartSpawnNewBubble(_shootDelay, false);
                _uIManager.OpenPanel(_uIManager.LosePanel);
                return;
            }
        
        StartCoroutine(StartSpawnNewBubble(_shootDelay, true)); 
    }

    private void CheckNeighbour(Cell cell, List<Cell> bubblesPcak, bool isHit)
    {
        cell.PutInPack();
        bubblesPcak.Add(cell);

        int x = cell.XPos;
        int y = cell.YPos;
        int offset = (y + _wave) % 2;

        int left = x - 1;
        int right = x + 1;
        int up = y - 1;
        int down = y + 1;
        int leftCorner = left + offset;
        int rightCorner = x + offset;

        if (left >= 0)
        {
            CheckBubble(_cells[y][left], bubblesPcak, cell.Type, isHit);
        }
        
        if (leftCorner >= 0)
        {
            if (up >= 0 && isHit)
            {
                CheckBubble(_cells[up][leftCorner], bubblesPcak, cell.Type, isHit);
            }
                
            if (down < _cells.Count)
            {
                CheckBubble(_cells[down][leftCorner], bubblesPcak, cell.Type, isHit);
            }
        }


        if (right < _cells[y].Count)
        {
            CheckBubble(_cells[y][right], bubblesPcak, cell.Type, isHit);
        }

        if (up >= 0 && rightCorner < _cells[up].Count && isHit)
        {
            CheckBubble(_cells[up][rightCorner], bubblesPcak, cell.Type, isHit);
        }

        if (down < _cells.Count && rightCorner < _cells[down].Count)
        {
            CheckBubble(_cells[down][rightCorner], bubblesPcak, cell.Type, isHit);
        }
    }

    private void CheckBubble(Cell cell, List<Cell> BallsPack, BubbleType targetType, bool isHit)
    {
        if (isHit)
        {
            if (cell.Type == targetType && !cell.IsInPack)
                CheckNeighbour(cell, BallsPack, isHit);      
        }
        else
        {
            if (!cell.IsInPack && cell.Type != BubbleType.Empty)
                CheckNeighbour(cell, BallsPack, isHit); 
        }
    }

    private void DestroyBubblesInPack(List<Cell> bubblesPcak)
    {
        int bubbles = 0;
        foreach (var cell in bubblesPcak)
        {
            StartCoroutine(DestroyBubbleWithDelay(cell, bubbles * 0.05f));
            bubbles ++;
        }
            
    }

    private IEnumerator DestroyBubbleWithDelay(Cell cell, float delay)
    {
        cell.DestroyBubble();
        yield return new WaitForSeconds(delay);
        _audioManager.PlaySFX(_audioManager.PopSound);
        cell.PlayPopAnimation();
    }

    private void ClearPack(List<Cell> bubblesPcak)
    {
        foreach (var cell in bubblesPcak)
            cell.GetFromPack();
    }
}
