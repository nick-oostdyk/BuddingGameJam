using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemStack : MonoBehaviour
{
	[SerializeField] private Image _image;
	[SerializeField] private TextMeshProUGUI _itemAmount;

	public void Init(Sprite sprite, string amount)
	{
		_image.sprite = sprite;
		_itemAmount.text = amount;
	}
}
