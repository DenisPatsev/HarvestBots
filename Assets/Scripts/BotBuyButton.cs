using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BotBuyButton : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private Button _button;

    ColorBlock _startColorBlock;

    private void Start()
    {
        _startColorBlock = _button.colors;
    }

    public void SetNewColor()
    {
        if (_base.BoxCount >= _base.BotCost)
        {
            ColorBlock colors = _button.colors;
            colors.highlightedColor = Color.green;
            _button.colors = colors;
        }
        else
        {
            ColorBlock colors = _button.colors;
            colors.highlightedColor = Color.red;
            _button.colors = colors;
        }
    }

    public void SetStartColor()
    {
        _button.colors = _startColorBlock;
    }
}
