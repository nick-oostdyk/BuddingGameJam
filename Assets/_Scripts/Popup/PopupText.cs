using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private Image _image;

    public void Setup(string _textIn, Sprite _spriteIn)
    {
        _textMeshPro.text = _textIn;
        _image.sprite = _spriteIn;
    }

    public void Clear()
    {
        _textMeshPro.text = "";
        _image.sprite = null;
        _image.color = new Color(0,0,0,0);
    }
}
