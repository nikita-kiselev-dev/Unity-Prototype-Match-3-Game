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
        tileCountWidthInput = int.Parse(tileCountWidth.text);
    }

    public void SaveHeight()
    {
        tileCountHeightInput = int.Parse(tileCountHeight.text);
    }

    public void SaveColor()
    {
        colorNumberInput = int.Parse(colorNumber.text);
    }
}
