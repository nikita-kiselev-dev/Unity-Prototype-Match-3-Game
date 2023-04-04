using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
    private MainBoard _mainBoard;
    
    public int tileNumber;
    private Image _tileIcon;

    private int _maxTileTypes;

    private void Awake()
    {
        _mainBoard = MainBoard.Instance;
        _maxTileTypes = _mainBoard.tileColorNumber;
    }

    void Start()
    {
        tileNumber = Random.Range(0, _maxTileTypes);
    }
}


