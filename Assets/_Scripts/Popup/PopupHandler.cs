using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupHandler : MonoBehaviour
{
	// attached to Player right now to test functionality
	[SerializeField] private GameObject _popupPrefab;
	[SerializeField] private Sprite _sprite;

	private Queue<PopupText> _popupPool;

	void Start()
	{
		GameObject p = Instantiate(_popupPrefab, Vector3.zero, Quaternion.identity);
		var popup = p.GetComponent<PopupText>();
		popup.PlayPopup(_sprite, "big banana bitches");
		Util.DelayRunAction(2500, () => Destroy(popup.gameObject));

		_fillPool();
	}

	public void PushPopup(Sprite sprite, string text)
	{
		var popup = _popupPool.Dequeue();
		popup.PlayPopup(sprite, text);

		
	}

	private void _fillPool()
	{
		if (_popupPool is not null) _popupPool.Clear();
		_popupPool = new Queue<PopupText>();

		var numInstances = 10;
		for (int i = 0; i < numInstances; ++i)
		{
			var go = Instantiate(_popupPrefab, transform);
			var popup = go.GetComponent<PopupText>();
			popup.Clear();
			_popupPool.Enqueue(popup);
		}
	}
}
