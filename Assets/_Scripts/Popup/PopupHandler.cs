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

	private Queue<PopupText> _popupPool;
	private Queue<PopupText> _activePool;

	private float _pushDelay = 0.3f;
	private float _nextPushTime = 0f;

	private void Start()
	{
		_activePool = new Queue<PopupText>();
		_fillPool();
	}

	public async void Update()
	{
		if (_activePool.Count == 0) return;
		if (Time.time > _nextPushTime)
		{
			_nextPushTime = Time.time + _pushDelay;
			var popup = _activePool.Dequeue();
			await popup.PlayAnim();
			popup.Clear();
			_popupPool.Enqueue(popup);
		}
	}

	public void PushPopup(Sprite sprite, string text)
	{
		var popup = _popupPool.Dequeue();
		popup.SetAttributes(sprite, text);
		_activePool.Enqueue(popup);
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
