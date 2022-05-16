using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupHandler : MonoBehaviour
{
	public static PopupHandler Instance { get; private set; }
	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(this);
	}

	[SerializeField] private GameObject _popupPrefab;
	[SerializeField] private Sprite _sprite;

	private Queue<PopupText> _popupPool;

	private void Start() => _fillPool();

	public async void PushPopup(Sprite sprite, string text)
	{
		var popup = _popupPool.Dequeue();
		await popup.PlayPopup(sprite, text);
		popup.Clear();
		_popupPool.Enqueue(popup);
	}

	private void _fillPool()
	{
		print("popup pool created");

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
