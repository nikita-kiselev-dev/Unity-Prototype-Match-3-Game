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
    private bool isSelected = false;

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
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isEmpty || MainBoard.Instance.IsShifting)
        {
            Debug.Log("2");
            return;
        }

        if (isSelected)
        {
            Debug.Log("3");
            Deselect();
        }
        else
        {
            if (previousSelectedTile == null)
            {
                Debug.Log("4");
                Select();
            }
            else
            {
                if (IsTileNearby())
                {
                    MainBoard.Instance.SwapTiles(previousSelectedTile, this);
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
    private void Deselect()
    {
        isSelected = false;
        gameObject.GetComponent<Image>().color = Color.red;
        previousSelectedTile = null;
    }
    private bool IsTileNearby()
    {
        bool isXTileNear = Math.Abs(xData - previousSelectedTile.xData) == 1;

        bool isYTileNear = Math.Abs(yData - previousSelectedTile.yData) == 1;

        if ((isXTileNear && (yData == previousSelectedTile.yData)) || (isYTileNear && (xData == previousSelectedTile.xData)))
        {
            Debug.Log("true");
            return true;
        }
        Debug.Log("false");
        return false;
    }
    
    /*
   private GameObject GetTilesNearby(Vector2 raycastDirection)
   {
       RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection);
       if (hit.collider != null) 
       {
           Debug.Log("Tile found!");
           return hit.collider.gameObject;
       }
       Debug.Log("Tile not found!");
       return null;
   }
   private List<GameObject> GetAllTilesNearby()
   {
       List<GameObject> tilesNearby = new List<GameObject>();
       for (int directionIndex = 0; directionIndex < raycastDirections.Length; directionIndex++)
       {
           tilesNearby.Add(GetTilesNearby(raycastDirections[directionIndex]));
       }
       return tilesNearby;
   }
*/

}


