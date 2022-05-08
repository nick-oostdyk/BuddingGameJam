using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirepitBehaviour : MonoBehaviour, IInteractable
{
	private SpriteRenderer _sr;
	private Sprite _litSprite, _unlitSprite;
	private bool _isLit;

	private void Start()
	{
		_sr = GetComponent<SpriteRenderer>();
		_loadSprites();
		_isLit = false;
	}

	public void Interact()
	{
		_toggle();
	}

	private void _toggle()
	{
		_isLit = !_isLit;
		_sr.sprite = _isLit ? _litSprite : _unlitSprite;
	}

	private void _loadSprites()
	{
		_litSprite = Resources.Load<Sprite>("Sprites/Cave/CampfireLit");
		_unlitSprite = Resources.Load<Sprite>("Sprites/Cave/CampfireUnlit");
	}
}
