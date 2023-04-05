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
        BuildBoard(isRebuild:false);
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
        _mainBoardArr[previousSelectedTile.yData, previousSelectedTile.xData] = 
            _mainBoardArr[currentTile.yData, currentTile.xData];
        _mainBoardArr[currentTile.yData, currentTile.xData] = 
            tempGameObject.gameObject;
        CleanMainBoard();
        BuildBoard(isRebuild:true);
    }

    //Separate FileTileArray + Tile Instantiate realisation
    /*
    private void FillTileArray()
   {
       _mainBoardArr = new GameObject[tileCountHeight, tileCountWidth];
       for (int heightIndex = 0; heightIndex < _mainBoardArr.GetLength(0); heightIndex++)
       {
           for (int widthIndex = 0; widthIndex < _mainBoardArr.GetLength(1); widthIndex++)
           {
               int tileNumber = Random.Range(0, tileColorNumber);
               _mainBoardArr[heightIndex, widthIndex] = tileCommonPrefab[tileNumber];
           }
       }
   }
   /*
    /*
   private void SpawnTile()
   {
       GameObject[] previousLeftTile = new GameObject[tileCountHeight];
       GameObject previousBelowTile = null;
       for (int heightIndex = 0; heightIndex < _mainBoardArr.GetLength(0); heightIndex++)
       {
           GameObject tileRow = Instantiate(rowPrefab, mainBoard.transform);
           tileRow.name = $"[{heightIndex}] " + tileRow.name;
           for (int widthIndex = 0; widthIndex < _mainBoardArr.GetLength(1); widthIndex++)
           {
               GameObject tile = Instantiate(_mainBoardArr[heightIndex, widthIndex], tileRow.transform);
               tile.name = $"[{heightIndex}][{widthIndex}] " + tile.name;
           }
       }
   }
   */
    //Old BuildBoard method
    /*
    private void AddTiles()
    {
        _mainBoardArr = new GameObject[tileCountHeight, tileCountWidth];
        GameObject mainBoard = GameObject.FindWithTag("Main Board");
        for (int heightIndex = 0; heightIndex < tileCountHeight; heightIndex++)
        {
            GameObject tileRow = Instantiate(rowPrefab, mainBoard.transform);
            tileRow.name = $"[{heightIndex}] " + tileRow.name;
            for (int widthIndex = 0; widthIndex < tileCountWidth; widthIndex++)
            {
                int tileNumber = Random.Range(0, tileColorNumber);
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
    */
}
