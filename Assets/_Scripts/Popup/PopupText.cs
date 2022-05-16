using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupText : MonoBehaviour
{
	[SerializeField] private Animator _anim;
	[SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private Image _image;

	public async Task PlayPopup(Sprite sprite, string text)
	{
		_image.enabled = true;
		_image.sprite = sprite;
		_image.color = Color.white;

		_text.text = text;
		_text.color = Color.white;

		await Util.PlayAndWaitForAnim(_anim, "PopupText");
	}

	public void Clear()
	{
		var clearWhite = new Color(1f, 1f, 1f, 0f);
		
		_image.color = clearWhite;
		_image.sprite = null;
		_image.enabled = false;

		_text.color = clearWhite;
		_text.text = "";
	}
}
