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
        StaticInstanceNullCheck();
        inputController = gameObject.GetComponent<InputController>();
    }

    public void StartGame()
    {
        if (isGameActive)
        {
            CleanMainBoard();
        }
        isGameActive = true;
        GetBoardStartInfo();
        AddTiles();
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
    private void StaticInstanceNullCheck()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void AddTiles()
    {
        GameObject[] previousLeftTile = new GameObject[tileCountHeight];
        GameObject previousBelowTile = null;
        
        _mainBoardArr = new GameObject[tileCountHeight, tileCountWidth];
        GameObject mainBoard = GameObject.FindWithTag("Main Board");
        for (int heightIndex = 0; heightIndex < tileCountHeight; heightIndex++)
        {
            GameObject tileRow = Instantiate(rowPrefab, mainBoard.transform);
            tileRow.name = $"[{heightIndex}] " + tileRow.name;
            for (int widthIndex = 0; widthIndex < tileCountWidth; widthIndex++)
            {
                //GameObject possibleTiles = new GameObject();
                GameObject tile = Instantiate(InputRandomCommonTile(), tileRow.transform);
                tile.name = $"[{heightIndex}][{widthIndex}] " + tile.name;
                _mainBoardArr[heightIndex, widthIndex] = tile;
            }
        }
    }
    private GameObject InputRandomCommonTile()
    {
        int tileNumber = Random.Range(0, tileCommonPrefab.Length);
        return tileCommonPrefab[tileNumber];
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
}
