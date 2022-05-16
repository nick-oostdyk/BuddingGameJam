using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupText : MonoBehaviour
{
	[SerializeField] private Animator _anim;
	[SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private Image _image;

	public void PlayPopup(Sprite sprite, string text)
	{
		_image.sprite = sprite;
		_text.text = text;

		_image.color = Color.white;
		_text.color = Color.white;

		_anim.Play("PopupText");
	}

	public void Clear()
	{
		_image.sprite = null;
		_text.text = "";
	}
}
