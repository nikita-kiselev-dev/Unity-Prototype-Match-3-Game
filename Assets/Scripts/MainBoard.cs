using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainBoard : MonoBehaviour
{
    public static MainBoard Instance;

    [SerializeField] private int tileCountWidth;
    [SerializeField] private int tileCountHeight;
    public int tileColorNumber;

    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject[] tileCommonPrefab;

    [SerializeField] private GameObject[] tileSpecialPrefab;
    
    //private GameObject tilePrefab;

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
            _mainBoardArr = new GameObject[tileCountHeight, tileCountWidth];
        }

        GameObject mainBoard = GameObject.FindWithTag("Main Board");
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
        Debug.Log("first tile: " + matchingTilesVertical[0]);
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
            if (isBackwardTileMatches(i, "vertical"))
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
        ///////
        matchingTilesHorizontal.Add(_mainBoardArr[y, x]);
        Debug.Log("first tile: " + matchingTilesHorizontal[0]);
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
            if (isBackwardTileMatches(i, "horizontal"))
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
        bool isBackwardTileMatches(int i, string orientation)
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
}
