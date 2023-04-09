using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class MainBoard : MonoBehaviour
{
    public static MainBoard Instance;

    private int _points = 0;

    private int _tileCountWidth;
    private int _tileCountHeight;

    [SerializeField] private TextMeshProUGUI pointsText;
    
    public int tileColorNumber;

    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject[] tileCommonPrefab;
    [SerializeField] private GameObject tileEmptyPrefab;

    private bool _isGameActive;
    private bool _isMatch;

    private bool _isCheckNeeded;

    private GameObject[,] _mainBoardArr;
    [SerializeField] private GameObject mainBoard;

    private InputController _inputController;
    
    private List<GameObject> _matchingTilesVertical = new List<GameObject>();
    private List<GameObject> _matchingTilesHorizontal = new List<GameObject>();

    private void Awake()
    {
        Instance = GetComponent<MainBoard>();
        _inputController = gameObject.GetComponent<InputController>();
    }
    //Получение данных об игровом поле от InputController
    private void GetBoardStartInfo()
    {
        _tileCountWidth = _inputController.tileCountWidthInput;
        _tileCountHeight = _inputController.tileCountHeightInput;
        tileColorNumber = _inputController.colorNumberInput;
    }
    //Первый запуск игры
    public void StartGame()
    {
        AudioController.Instance.PlaySound("start");
        _inputController.HideWarning();
        if (_isGameActive)
        {
            CleanMainBoard();
        }
        GetBoardStartInfo();
        BuildBoard(isRebuild: false);
        pointsText.text = _points.ToString();
        _isGameActive = true;
    }
    //Очистка игрового поля от фишек
    private void CleanMainBoard()
    {
        foreach (Transform child in mainBoard.transform)
        {
            Destroy(child.gameObject);
        }
    }
    //Построение игрового поля и instantiate игровых фишек
    private void BuildBoard(bool isRebuild)
    {
        if (!isRebuild)
        {
            _points = 0;
            _mainBoardArr = new GameObject[_tileCountHeight, _tileCountWidth];
        }
        for (int heightIndex = 0; heightIndex < _tileCountHeight; heightIndex++)
        {
            GameObject tileRow = Instantiate(rowPrefab, mainBoard.transform);
            tileRow.name = $"[{heightIndex}] " + tileRow.name;
            for (int widthIndex = 0; widthIndex < _tileCountWidth; widthIndex++)
            {
                int tileNumber = 0;
                if (!isRebuild)
                {
                    tileNumber = Random.Range(0, tileColorNumber);
                }
                else
                {
                    tileNumber = _mainBoardArr[heightIndex, widthIndex].GetComponent<Tile>().tileNumber;
                }

                GameObject tile = Instantiate(tileCommonPrefab[tileNumber], tileRow.transform);
                tile.name = $"[{heightIndex}][{widthIndex}] " + tile.name;
                
                
                Tile tileInfo = tile.GetComponent<Tile>();
                tileInfo.tileNumber = tileNumber;
                tileInfo.xData = widthIndex;
                tileInfo.yData = heightIndex;
                
                _mainBoardArr[heightIndex, widthIndex] = tile;
            }

        }
    }
    //Смена фишек местами
    public void SwapTiles(Tile previousSelectedTile, Tile currentTile)
    {
        Tile tempGameObject = Tile.PreviousSelectedTile;
        _mainBoardArr[previousSelectedTile.yData, previousSelectedTile.xData] =
            _mainBoardArr[currentTile.yData, currentTile.xData];
        _mainBoardArr[currentTile.yData, currentTile.xData] =
            tempGameObject.gameObject;
        CleanMainBoard();
        BuildBoard(isRebuild: true);
        AudioController.Instance.PlaySound("swap");
        FindMatch(currentTile.yData, currentTile.xData);
        GetMatchPoints();
        StartCoroutine(MoveTileToGround());
    }
    //Поиск 3 и более шифек в одном ряду с перемещенной фишкой
    private void FindMatch(int y, int x)
    {
        _matchingTilesVertical.Add(_mainBoardArr[y, x]);
        
        for (int i = y; i < _mainBoardArr.GetLength(0) - 1; i++)
        {
            if (IsForwardTileMatches(i, "vertical"))
            {
                _matchingTilesVertical.Add(_mainBoardArr[i + 1, x]);
            }
            else
            {
                break;
            }
        }
        for (int i = y; i > 0; i--)
        {
            if (IsBackwardTileMatches(i, "vertical"))
            {
                _matchingTilesVertical.Add(_mainBoardArr[i - 1, x]);
            }
            else
            {
                break;
            }
        }
        if (!(_matchingTilesVertical.Count >= 3))
        {
            _matchingTilesVertical.Clear();
        }

        _matchingTilesHorizontal.Add(_mainBoardArr[y, x]);
        
        for (int i = x; i < _mainBoardArr.GetLength(1) - 1; i++)
        {
            if (IsForwardTileMatches(i, "horizontal"))
            {
                _matchingTilesHorizontal.Add(_mainBoardArr[y , i + 1]);
            }
            else
            {
                break;
            }
        }
        for (int i = x; i > 0; i--)
        {
            if (IsBackwardTileMatches(i, "horizontal"))
            {
                _matchingTilesHorizontal.Add(_mainBoardArr[y, i - 1]);
            }
            else
            {
                break;
            }
        }
        if (!(_matchingTilesHorizontal.Count >= 3))
        {
            _matchingTilesHorizontal.Clear();
        }

        bool IsForwardTileMatches(int i, string orientation)
        {
            switch (orientation)
            {
                case "vertical":
                    if (_mainBoardArr[i, x].GetComponent<Tile>().tileNumber ==
                        _mainBoardArr[i + 1, x].GetComponent<Tile>().tileNumber)
                    {
                        return true;
                    }
                    break;
                case "horizontal":
                    if (_mainBoardArr[y, i].GetComponent<Tile>().tileNumber ==
                        _mainBoardArr[y, i + 1].GetComponent<Tile>().tileNumber)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }
        bool IsBackwardTileMatches(int i, string orientation)
        {
            switch (orientation)
            {
                case "vertical":
                    if (_mainBoardArr[i, x].GetComponent<Tile>().tileNumber ==
                        _mainBoardArr[i - 1, x].GetComponent<Tile>().tileNumber)
                    {
                        return true;
                    }
                    break;
                case "horizontal":
                    if (_mainBoardArr[y, i].GetComponent<Tile>().tileNumber ==
                        _mainBoardArr[y, i - 1].GetComponent<Tile>().tileNumber)
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
    }
    //Получение очков за >=3 в ряд
    private void GetMatchPoints()
    {
        bool MultipleAxisMatch()
        {
            if (_matchingTilesVertical.Count > 1 && _matchingTilesHorizontal.Count > 1)
            {
                return true;
            }

            return false;
        }
        
        List<GameObject> matchedTiles = new List<GameObject>();
        matchedTiles.AddRange(_matchingTilesVertical);
        matchedTiles.AddRange(_matchingTilesHorizontal);

        if (matchedTiles.Count > 1)
        {
            AudioController.Instance.PlaySound("match");
        }
        
        if (MultipleAxisMatch())
        {
            _points += matchedTiles.Count - 1;
        }
        else
        {
            _points += matchedTiles.Count;
        }
        
        _matchingTilesVertical.Clear();
        _matchingTilesHorizontal.Clear();
        pointsText.text = _points.ToString();
        DestroyMatchTiles(matchedTiles);
    }
    //Уничтожение фишек при сборке >=3 в ряд
    private void DestroyMatchTiles(List<GameObject> matchedTiles)
    {
        for (int i = 0; i < matchedTiles.Count; i++)
        {
            Tile tempTile = matchedTiles[i].GetComponent<Tile>();
            var tempYData = tempTile.yData;
            var tempXData = tempTile.xData;
            _mainBoardArr[tempYData, tempXData] = tileEmptyPrefab;
        }
        matchedTiles.Clear();
        CleanMainBoard();
        BuildBoard(isRebuild: true);
        //StartCoroutine(MoveTileToGround());
    }
    //Перемещение заполненных клеток вниз, если снизу присутствует пустая клетка
    private void EmptyTileMover()
    {
        for (int heightIndex = 1; heightIndex < _tileCountHeight; heightIndex++)
        {
            for (int widthIndex = 0; widthIndex < _tileCountWidth; widthIndex++)
            {
                GameObject currentTileGameObject = _mainBoardArr[heightIndex, widthIndex];
                GameObject upperTileGameObject = _mainBoardArr[heightIndex - 1, widthIndex];

                Tile currentTile = currentTileGameObject.GetComponent<Tile>();
                Tile upperTile = upperTileGameObject.GetComponent<Tile>();

                if (upperTile.isEmpty == false && currentTile.isEmpty)
                {
                    _mainBoardArr[currentTile.yData, currentTile.xData] =
                        _mainBoardArr[upperTile.yData, upperTile.xData];
                    _mainBoardArr[upperTile.yData, upperTile.xData] = tileEmptyPrefab;
                }
            }
        }
    }
    //Проверка на пустые клетки снизу
    private bool IsCheckNeeded()
    {
        for (int heightIndex = 1; heightIndex < _tileCountHeight; heightIndex++)
        {
            for (int widthIndex = 0; widthIndex < _tileCountWidth; widthIndex++)
            {
                GameObject currentTileGameObject = _mainBoardArr[heightIndex, widthIndex];
                GameObject upperTileGameObject = _mainBoardArr[heightIndex - 1, widthIndex];

                Tile currentTile = currentTileGameObject.GetComponent<Tile>();
                Tile upperTile = upperTileGameObject.GetComponent<Tile>();

                if (upperTile.isEmpty == false && currentTile.isEmpty)
                {
                    return true;
                }
            }
        }

        return false;
    }
    //Корутина для анимации перемещения фишек
    private IEnumerator MoveTileToGround()
    {
        while (IsCheckNeeded())
        {
            yield return new WaitForSeconds(0.03f);
            EmptyTileMover();
            CleanMainBoard();
            BuildBoard(isRebuild: true);
        }
        
    }
}
