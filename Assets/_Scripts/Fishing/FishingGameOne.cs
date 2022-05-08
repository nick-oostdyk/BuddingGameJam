using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishingGameOne : MonoBehaviour
{
	[SerializeField] private InputAction _action;

	private FishItem _fishItem;
	[SerializeField] private Fish _fish;
	[SerializeField] private ProgressBar _playerBar;
	[SerializeField] private ProgressBar _fishBar;

	[HideInInspector] public bool Complete;
	[HideInInspector] public bool Success;

	private bool _started = false;

	public void Init(FishItem fish)
	{
		_fishItem = fish;
		_fish.Init(_fishItem);

		_playerBar.SetProgress(0f);
		_fishBar.SetProgress(0f);

		Complete = Success = false;
		_started = true;
	}

	private void FixedUpdate()
	{
		if (!_started) return;

		_fish.DoMove(_getDirection());
		_fish.ClampPosition();
		_tickProgress();
	}

	private Vector2 _getDirection()
	{
		// perlin
		var direction = Vector2.right;
		var influence = 2f * Mathf.PerlinNoise(Time.time * 1.2f, 0f) - 1f;

		// will to escape
		var fishLocalPos = _fish.transform.localPosition;
		influence += .1f * (fishLocalPos.x < 0f ? -1f : 1f);
		_fish.GetComponent<SpriteRenderer>().flipX = influence <= 0f;

		// player influence
		influence += .6f * _action.ReadValue<float>();

		return direction * influence;
	}

	private void _tickProgress()
	{
		var playerDelta = Time.fixedDeltaTime / _fishItem.PlayerFillTime;
		var fishDelta = Time.fixedDeltaTime / _fishItem.FishFillTime;

		if (_fish.GetIsInZone())
			_playerBar.AddProgress(playerDelta);
		else
			_fishBar.AddProgress(fishDelta);

		var (playerComplete, fishComplete) = (_playerBar.Progress == 1f, _fishBar.Progress == 1f);
		Complete = playerComplete || fishComplete;
		Success = Complete && playerComplete;

		if (Complete) _started = false;
	}

	private void OnEnable() => _action.Enable();
	private void OnDisable() => _action.Disable();
}
