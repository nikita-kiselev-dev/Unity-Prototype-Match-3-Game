using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputController : MonoBehaviour
{
    [SerializeField] private TMP_InputField tileCountWidth;
    [SerializeField] private TMP_InputField tileCountHeight;
    [SerializeField] private TMP_InputField colorNumber;
    
    [SerializeField] private const int MinSize = 10;
    [SerializeField] private const int MaxSize = 50;
    [SerializeField] private const int MinColorNumber = 2;
    [SerializeField] private const int MaxColorNumber = 5;

    public int tileCountWidthInput;
    public int tileCountHeightInput;
    public int colorNumberInput;

    private void Awake()
    {
        tileCountWidthInput = int.Parse(tileCountWidth.text);
        tileCountHeightInput = int.Parse(tileCountHeight.text);
        colorNumberInput = int.Parse(colorNumber.text);
    }

    public void SaveWidth()
    {
        if (int.Parse(tileCountWidth.text) >= MinSize && int.Parse(tileCountWidth.text) <= MaxSize)
        {
            tileCountWidthInput = int.Parse(tileCountWidth.text);
        }
        else if (int.Parse(tileCountWidth.text) < MinSize)
        {
            tileCountWidth.text = MinSize.ToString();
        }
        else if (int.Parse(tileCountWidth.text) > MaxSize)
        {
            tileCountWidth.text = MaxSize.ToString();
        }
    }

    public void SaveHeight()
    {
        if (int.Parse(tileCountHeight.text) >= MinSize && int.Parse(tileCountHeight.text) <= MaxSize)
        {
            tileCountHeightInput = int.Parse(tileCountHeight.text);
        }
        else if (int.Parse(tileCountHeight.text) < MinSize)
        {
            tileCountHeight.text = MinSize.ToString();
        }
        else if (int.Parse(tileCountHeight.text) > MaxSize)
        {
            tileCountHeight.text = MaxSize.ToString();
        }
    }

    public void SaveColor()
    {
        if (int.Parse(colorNumber.text) >= MinColorNumber && int.Parse(colorNumber.text) <= MaxColorNumber)
        {
            colorNumberInput = int.Parse(colorNumber.text);
        }
        else if (int.Parse(colorNumber.text) < MinColorNumber)
        {
            colorNumber.text = MinColorNumber.ToString();
        }
        else if (int.Parse(colorNumber.text) > MaxColorNumber)
        {
            colorNumber.text = MaxColorNumber.ToString();
        }
    }
}
