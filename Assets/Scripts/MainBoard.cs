using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainBoard : MonoBehaviour
{
    public static MainBoard Instance;

    [SerializeField] private int points = 0;

    [SerializeField] private int tileCountWidth;
    [SerializeField] private int tileCountHeight;
    public int tileColorNumber;

    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject[] tileCommonPrefab;
    [SerializeField] private GameObject tileEmptyPrefab;

    private bool isGameActive;
    public bool IsShifting { get; set; }
    private bool isMatch;

    [SerializeField] private GameObject[,] _mainBoardArr;
    [SerializeField] private GameObject mainBoard;

    private InputController inputController;

    private void Awake()
    {
        Instance = GetComponent<MainBoard>();
        inputController = gameObject.GetComponent<InputController>();
    }

    public void StartGame()
    {
        if (isGameActive)
        {
            CleanMainBoard();
        }

        GetBoardStartInfo();
        BuildBoard(isRebuild: false);
        //AddTiles();
        isGameActive = true;
        //FillTileArray();
        //SpawnTile();
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
        tileCountWidth = inputController.tileCountWidthInput;
        tileCountHeight = inputController.tileCountHeightInput;
        tileColorNumber = inputController.colorNumberInput;
    }

    private void BuildBoard(bool isRebuild)
    {
        if (!isRebuild)
        {
            points = 0;
            _mainBoardArr = new GameObject[tileCountHeight, tileCountWidth];
        }

        //GameObject mainBoard = GameObject.FindWithTag("Main Board");
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
            }

        }
    }

    public void SwapTiles(Tile previousSelectedTile, Tile currentTile)
    {
        Tile tempGameObject = Tile.previousSelectedTile;
        Tile chosenTile = _mainBoardArr[currentTile.yData, currentTile.xData].GetComponent<Tile>();
        _mainBoardArr[previousSelectedTile.yData, previousSelectedTile.xData] =
            _mainBoardArr[currentTile.yData, currentTile.xData];
        _mainBoardArr[currentTile.yData, currentTile.xData] =
            tempGameObject.gameObject;
        CleanMainBoard();
        BuildBoard(isRebuild: true);
        FindMatch("vertical", currentTile.yData, currentTile.xData);
    }

    private void FindMatch(string axis, int y, int x)
    {
        List<GameObject> matchingTilesVertical = new List<GameObject>();
        List<GameObject> matchingTilesHorizontal = new List<GameObject>();
        
        matchingTilesVertical.Add(_mainBoardArr[y, x]);

        //////////
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
        if (matchingTilesVertical.Count >= 3)
        {
            foreach (var sameTile in matchingTilesVertical)
            {
                sameTile.GetComponent<Tile>().isEmpty = true;
                sameTile.GetComponent<Tile>().backgroundImage.color = Color.blue;
            }
        }
        else
        {
            matchingTilesVertical.Clear();
        }
        ///////
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
        if (matchingTilesHorizontal.Count >= 3)
        {
            foreach (var sameTile in matchingTilesHorizontal)
            {
                sameTile.GetComponent<Tile>().isEmpty = true;
                sameTile.GetComponent<Tile>().backgroundImage.color = Color.blue;
            }
        }
        else
        {
            matchingTilesHorizontal.Clear();
        }

        //Debug.Log(Math.Max(matchingTilesHorizontal.Count, matchingTilesVertical.Count));
        /////////
        /*
        switch (axis)
        {
            case "vertical":
                for (int i = y; i < _mainBoardArr.GetLength(0) - 1; i++)
                {
                    if (IsForwardTileMatches(i))
                    {
                        matchingTiles.Add(_mainBoardArr[i + 1, x]);
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = y; i > 0; i--)
                {
                    if (isBackwardTileMatches(i))
                    {
                        matchingTiles.Add(_mainBoardArr[i - 1, x]);
                    }
                    else
                    {
                        break;
                    }
                }
                if (matchingTiles.Count >= 3)
                {
                    yMatchingTilesNumber = matchingTiles.Count;
                    foreach (var sameTile in matchingTiles)
                    {
                        sameTile.GetComponent<Tile>().isEmpty = true;
                        sameTile.GetComponent<Tile>().backgroundImage.color = Color.blue;
                    }
                }
                break;
            case "horizontal":
                for (int i = y; i < _mainBoardArr.GetLength(1) - 1; i++)
                {
                    if (IsForwardTileMatches(i))
                    {
                        matchingTiles.Add(_mainBoardArr[y , i + 1]);
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = y; i > 0; i--)
                {
                    if (isBackwardTileMatches(i))
                    {
                        matchingTiles.Add(_mainBoardArr[y, i - 1]);
                    }
                    else
                    {
                        break;
                    }
                }
                if (matchingTiles.Count >= 3)
                {
                    xMatchingTilesNumber = matchingTiles.Count;
                    foreach (var sameTile in matchingTiles)
                    {
                        sameTile.GetComponent<Tile>().isEmpty = true;
                        sameTile.GetComponent<Tile>().backgroundImage.color = Color.blue;
                    }
                }
                break;
            default:
                Debug.Log("Incorrect axis");
                break;
        }
        */
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
        CleanMainBoard();
        BuildBoard(isRebuild: true);
        GetMatchPoints(matchingTilesVertical, matchingTilesHorizontal);
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
        
        if (MultipleAxisMatch())
        {
            points += matchedTiles.Count - 1;
        }
        else
        {
            points += matchedTiles.Count;
        }
        DestroyMatchTiles(matchedTiles);
    }
    private void DestroyMatchTiles(List<GameObject> matchedTiles)
    {
        /*
        string longerListAxisLenght()
        {
            if (matchingTilesVertical.Count > matchingTilesHorizontal.Count)
            {
                Debug.Log("+");
                return nameof(matchingTilesVertical);
            }
            Debug.Log("-");
            return nameof(matchingTilesHorizontal);
        }
        */
        for (int i = 0; i < matchedTiles.Count; i++)
        {
            var tempYData = matchedTiles[i].GetComponent<Tile>().yData;
            var tempXData = matchedTiles[i].GetComponent<Tile>().xData;
            _mainBoardArr[tempYData, tempXData] = tileEmptyPrefab;

        }
        matchedTiles.Clear();
        CleanMainBoard();
        BuildBoard(isRebuild: true);
        //MoveTilesDown();
    }

    private void MoveTilesDown()
    {
        int i = 0;
        Debug.Log(i);
        i++;
        do
        {
            Debug.Log($"I: {i}");
            i++;

            for (int heightIndex = 0; heightIndex < tileCountHeight - 1; heightIndex++)
            {
                for (int widthIndex = 0; widthIndex < tileCountWidth; widthIndex++)
                {
                    Tile currentTile = _mainBoardArr[heightIndex, widthIndex].GetComponent<Tile>();
                    Tile lowerTile = _mainBoardArr[heightIndex + 1, widthIndex].GetComponent<Tile>();
                        if (currentTile.isEmpty == false &&
                            lowerTile.isEmpty)
                        {
                            Debug.Log("lower tile before: " +  _mainBoardArr[lowerTile.yData, lowerTile.xData]);
                            _mainBoardArr[lowerTile.yData, lowerTile.xData] = currentTile.gameObject;
                            Debug.Log("lower tile after: " +  _mainBoardArr[lowerTile.yData, lowerTile.xData]);
                            Debug.Log("current tile before: " +  _mainBoardArr[currentTile.yData, currentTile.xData]);
                            _mainBoardArr[currentTile.yData, currentTile.xData] = tileEmptyPrefab;
                            Debug.Log("current tile after: " +  _mainBoardArr[currentTile.yData, currentTile.xData]);
                        }
                }
            }
        } while (i < 1);
        CleanMainBoard();
        BuildBoard(isRebuild: true);
    }

    private bool IsTileSwapDownNeeded()
    {
        for (int heightIndex = 1; heightIndex < tileCountHeight; heightIndex++)
        {
            for (int widthIndex = 0; widthIndex < tileCountWidth; widthIndex++)
            {
                Tile currentTile = _mainBoardArr[heightIndex, widthIndex].GetComponent<Tile>();
                Tile upperTile = _mainBoardArr[heightIndex - 1, widthIndex].GetComponent<Tile>();
                if (currentTile.isEmpty && upperTile.isEmpty == false)
                {
                    Debug.Log("Ping2");
                    return true;
                }
            }
        }
        return false;
    }
}
