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
    
    public static Tile PreviousSelectedTile = null;
    
    public int tileNumber;
    public bool isEmpty = false;
    [SerializeField] private bool isSelected = false;

    private Image backgroundImage;
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
    //Клик по фишкам
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isEmpty)
        {
            return;
        }

        if (isSelected)
        {
            Deselect(gameObject);
        }
        else
        {
            if (PreviousSelectedTile == null)
            {
                Select();
            }
            else
            {
                if (IsTileNearby() && (tileNumber != PreviousSelectedTile.tileNumber))
                {
                    MainBoard.Instance.SwapTiles(PreviousSelectedTile, this);
                }
                else
                {
                    Deselect(PreviousSelectedTile.gameObject);
                }
            }
        }
    }
    //Выбор фишки при клике
    private void Select()
    {
        isSelected = true;
        backgroundImage.color = new Color32(255,203,164,255);
        PreviousSelectedTile = gameObject.GetComponent<Tile>();
    }
    //Отмена выбора фишки
    private void Deselect(GameObject tile)
    {
        isSelected = false;
        tile.GetComponent<Image>().color = Color.white;
        PreviousSelectedTile.isSelected = false;
        PreviousSelectedTile = null;
    }
    //Проверка близости двух выбранных фишек
    private bool IsTileNearby()
    {
        bool isXTileNear = Math.Abs(xData - PreviousSelectedTile.xData) == 1;

        bool isYTileNear = Math.Abs(yData - PreviousSelectedTile.yData) == 1;

        if ((isXTileNear && (yData == PreviousSelectedTile.yData)) || (isYTileNear && (xData == PreviousSelectedTile.xData)))
        {
            return true;
        }
        return false;
    }
}


