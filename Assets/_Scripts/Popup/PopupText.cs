using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupText : MonoBehaviour
{
	[SerializeField] private Animator _anim;
	[SerializeField] private TextMeshProUGUI _tmpugui;
	[SerializeField] private Image _image;

	private Sprite _sprite;
	private string _text;

	public void SetAttributes(Sprite s, string t)
	{
		_sprite = s;
		_text = t;
	}

	public async Task PlayAnim()
	{
		_enable();
		await Util.PlayAndWaitForAnim(_anim, "PopupText");
	}

	private void _enable()
	{
		_image.enabled = true;
		_image.sprite = _sprite;
		_image.color = Color.white;

		_tmpugui.text = _text;
		_tmpugui.color = Color.white;
	}

	public void Clear()
	{
		var clearWhite = new Color(1f, 1f, 1f, 0f);

		_image.color = clearWhite;
		_image.sprite = null;
		_image.enabled = false;

		_tmpugui.color = clearWhite;
		_tmpugui.text = "";
	}
}
