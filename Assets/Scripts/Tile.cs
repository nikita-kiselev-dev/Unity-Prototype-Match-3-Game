using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Tile : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] public int xData;
    [SerializeField] public int yData;
    
    public static Tile previousSelectedTile = null;
    
    public int tileNumber;
    public bool isEmpty = false;
    [SerializeField] private bool isSelected = false;
    //TODO make private
    public Image backgroundImage;
    private void Awake()
    {
        backgroundImage = gameObject.GetComponent<Image>();
    }
    private void Start()
    {
        if (gameObject.transform.childCount == 0)
        {
            isEmpty = true;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isEmpty || MainBoard.Instance.IsShifting)
        {
            return;
        }

        if (isSelected)
        {
            Deselect(gameObject);
        }
        else
        {
            if (previousSelectedTile == null)
            {
                Select();
            }
            else
            {
                if (IsTileNearby() && (tileNumber != previousSelectedTile.tileNumber))
                {
                    MainBoard.Instance.SwapTiles(previousSelectedTile, this);
                }
                else
                {
                    Deselect(previousSelectedTile.gameObject);
                    Debug.Log("They are the same!");
                }
            }
        }
    }
    private void Select()
    {
        isSelected = true;
        backgroundImage.color = Color.gray;
        previousSelectedTile = gameObject.GetComponent<Tile>();
    }
    private void Deselect(GameObject tile)
    {
        isSelected = false;
        tile.GetComponent<Image>().color = Color.red;
        previousSelectedTile.isSelected = false;
        previousSelectedTile = null;
    }
    private bool IsTileNearby()
    {
        bool isXTileNear = Math.Abs(xData - previousSelectedTile.xData) == 1;

        bool isYTileNear = Math.Abs(yData - previousSelectedTile.yData) == 1;

        if ((isXTileNear && (yData == previousSelectedTile.yData)) || (isYTileNear && (xData == previousSelectedTile.xData)))
        {
            return true;
        }
        return false;
    }
}


