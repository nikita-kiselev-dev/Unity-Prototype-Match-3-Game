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

    [SerializeField] private int points = 0;

    [SerializeField] private int tileCountWidth;
    [SerializeField] private int tileCountHeight;

    [SerializeField] private TextMeshProUGUI pointsText;
    
    public int tileColorNumber;

    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject[] tileCommonPrefab;
    [SerializeField] private GameObject tileEmptyPrefab;

    private bool _isGameActive;
    private bool _isMatch;

    private bool _isCheckNeeded;

    [SerializeField] private GameObject[,] _mainBoardArr;
    [SerializeField] private GameObject mainBoard;

    private InputController _inputController;
    
    [SerializeField] private List<GameObject> matchingTilesVertical = new List<GameObject>();
    [SerializeField] private List<GameObject> matchingTilesHorizontal = new List<GameObject>();

    private void Awake()
    {
        Instance = GetComponent<MainBoard>();
        _inputController = gameObject.GetComponent<InputController>();
    }

    public void StartGame()
    {
        _inputController.HideWarning();
        if (_isGameActive)
        {
            CleanMainBoard();
        }
        GetBoardStartInfo();
        BuildBoard(isRebuild: false);
        pointsText.text = points.ToString();
        _isGameActive = true;
    }
    
    private void CleanMainBoard()
    {
        foreach (Transform child in mainBoard.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void GetBoardStartInfo()
    {
        tileCountWidth = _inputController.tileCountWidthInput;
        tileCountHeight = _inputController.tileCountHeightInput;
        tileColorNumber = _inputController.colorNumberInput;
    }
    
    private void BuildBoard(bool isRebuild)
    {
        if (!isRebuild)
        {
            points = 0;
            _mainBoardArr = new GameObject[tileCountHeight, tileCountWidth];
        }
        for (int heightIndex = 0; heightIndex < tileCountHeight; heightIndex++)
        {
            GameObject tileRow = Instantiate(rowPrefab, mainBoard.transform);
            tileRow.name = $"[{heightIndex}] " + tileRow.name;
            for (int widthIndex = 0; widthIndex < tileCountWidth; widthIndex++)
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
                //Debug.Log(_mainBoardArr[heightIndex, widthIndex].GetComponent<Tile>().tileNumber);
                //Debug.Log(_mainBoardArr[heightIndex, widthIndex].GetComponent<Tile>().isEmpty);
                
            }

        }
    }

    public void SwapTiles(Tile previousSelectedTile, Tile currentTile)
    {
        Tile tempGameObject = Tile.PreviousSelectedTile;
        _mainBoardArr[previousSelectedTile.yData, previousSelectedTile.xData] =
            _mainBoardArr[currentTile.yData, currentTile.xData];
        _mainBoardArr[currentTile.yData, currentTile.xData] =
            tempGameObject.gameObject;
        CleanMainBoard();
        BuildBoard(isRebuild: true);
        FindMatch(currentTile.yData, currentTile.xData);

        GetMatchPoints(matchingTilesVertical, matchingTilesHorizontal);
        AudioController.Instance.PlaySound("swap");
    }

    private void FindMatch(int y, int x)
    {
        matchingTilesVertical.Add(_mainBoardArr[y, x]);
        
        for (int i = y; i < _mainBoardArr.GetLength(0) - 1; i++)
        {
            if (IsForwardTileMatches(i, "vertical"))
            {
                matchingTilesVertical.Add(_mainBoardArr[i + 1, x]);
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
                matchingTilesVertical.Add(_mainBoardArr[i - 1, x]);
            }
            else
            {
                break;
            }
        }
        if (!(matchingTilesVertical.Count >= 3))
        {
            matchingTilesVertical.Clear();
        }

        matchingTilesHorizontal.Add(_mainBoardArr[y, x]);
        
        for (int i = x; i < _mainBoardArr.GetLength(1) - 1; i++)
        {
            if (IsForwardTileMatches(i, "horizontal"))
            {
                matchingTilesHorizontal.Add(_mainBoardArr[y , i + 1]);
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
                matchingTilesHorizontal.Add(_mainBoardArr[y, i - 1]);
            }
            else
            {
                break;
            }
        }
        if (!(matchingTilesHorizontal.Count >= 3))
        {
            matchingTilesHorizontal.Clear();
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

    private void GetMatchPoints(List<GameObject> matchingTilesVertical, List<GameObject> matchingTilesHorizontal)
    {
        bool MultipleAxisMatch()
        {
            if (matchingTilesVertical.Count > 1 && matchingTilesHorizontal.Count > 1)
            {
                return true;
            }

            return false;
        }
        
        List<GameObject> matchedTiles = new List<GameObject>();
        matchedTiles.AddRange(matchingTilesVertical);
        matchedTiles.AddRange(matchingTilesHorizontal);

        if (matchedTiles.Count > 1)
        {
            AudioController.Instance.PlaySound("match");
        }
        
        if (MultipleAxisMatch())
        {
            points += matchedTiles.Count - 1;
        }
        else
        {
            points += matchedTiles.Count;
        }
        
        matchingTilesVertical.Clear();
        matchingTilesHorizontal.Clear();
        pointsText.text = points.ToString();
        DestroyMatchTiles(matchedTiles);
    }
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
        StartCoroutine(MoveTileToGround());
    }

    //TODO make private
    private void EmptyTileMover()
    {
        for (int heightIndex = 1; heightIndex < tileCountHeight; heightIndex++)
        {
            for (int widthIndex = 0; widthIndex < tileCountWidth; widthIndex++)
            {
                GameObject currentTileGameObject = _mainBoardArr[heightIndex, widthIndex];
                GameObject upperTileGameObject = _mainBoardArr[heightIndex - 1, widthIndex];

                Tile currentTile = currentTileGameObject.GetComponent<Tile>();
                Tile upperTile = upperTileGameObject.GetComponent<Tile>();

                if (upperTile.isEmpty == false && currentTile.isEmpty)
                {
                    Debug.Log("recolor!");
                    _mainBoardArr[currentTile.yData, currentTile.xData] =
                        _mainBoardArr[upperTile.yData, upperTile.xData];
                    _mainBoardArr[upperTile.yData, upperTile.xData] = tileEmptyPrefab;
                }
            }
        }
    }

    private bool IsCheckNeeded()
    {
        for (int heightIndex = 1; heightIndex < tileCountHeight; heightIndex++)
        {
            for (int widthIndex = 0; widthIndex < tileCountWidth; widthIndex++)
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
    
    private IEnumerator MoveTileToGround()
    {
        while (IsCheckNeeded())
        {
            Debug.Log("Iteration");
            yield return new WaitForSeconds(0.03f);
            EmptyTileMover();
            CleanMainBoard();
            BuildBoard(isRebuild: true);
        }
        
    }
}
