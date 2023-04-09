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

    [SerializeField] private GameObject warningText;
    private TextMeshProUGUI _warningTextTMP;
    
    private const int MinSize = 10;
    private const int MaxSize = 50;
    private const int MinColorNumber = 2;
    private const int MaxColorNumber = 5;

    public int tileCountWidthInput;
    public int tileCountHeightInput;
    public int colorNumberInput;

    private void Awake()
    {
        _warningTextTMP = warningText.GetComponent<TextMeshProUGUI>();
        tileCountWidthInput = int.Parse(tileCountWidth.text);
        tileCountHeightInput = int.Parse(tileCountHeight.text);
        colorNumberInput = int.Parse(colorNumber.text);
    }

    public void SaveWidth()
    {
        if (int.Parse(tileCountWidth.text) < MinSize)
        {
            tileCountWidth.text = MinSize.ToString();
            _warningTextTMP.text = $"Выбранная ширина ниже минимума! ({MinSize})";
            warningText.SetActive(true);
        }
        else if (int.Parse(tileCountWidth.text) > MaxSize)
        {
            tileCountWidth.text = MaxSize.ToString();
            _warningTextTMP.text = $"Выбранная ширина выше максимума! ({MaxSize})";
            warningText.SetActive(true);
        }
        tileCountWidthInput = int.Parse(tileCountWidth.text);
    }

    public void SaveHeight()
    {
        if (int.Parse(tileCountHeight.text) < MinSize)
        {
            tileCountHeight.text = MinSize.ToString();
            _warningTextTMP.text = $"Выбранная высота ниже минимума! ({MinSize})";
            warningText.SetActive(true);
        }
        else if (int.Parse(tileCountHeight.text) > MaxSize)
        {
            tileCountHeight.text = MaxSize.ToString();
            _warningTextTMP.text = $"Выбранная высота выше максимума! ({MaxSize})";
            warningText.SetActive(true);
        }
        tileCountHeightInput = int.Parse(tileCountHeight.text);
    }

    public void SaveColor()
    {
        if (int.Parse(colorNumber.text) < MinColorNumber)
        {
            colorNumber.text = MinColorNumber.ToString();
            _warningTextTMP.text = $"Выбрано слишком мало цветов! ({MinColorNumber})";
            warningText.SetActive(true);
        }
        else if (int.Parse(colorNumber.text) > MaxColorNumber)
        {
            colorNumber.text = MaxColorNumber.ToString();
            _warningTextTMP.text = $"Выбрано слишком много цветов! ({MaxColorNumber})";
            warningText.SetActive(true);
        }
        colorNumberInput = int.Parse(colorNumber.text);
    }

    public void HideWarning()
    {
        if (warningText.activeSelf)
        {
            warningText.SetActive(false);
        }
    }
}
