using UnityEngine;
using UnityEngine.InputSystem;

public abstract class FishingMinigame : MonoBehaviour
{
	[SerializeField] protected InputAction _action;
	[HideInInspector] public bool Complete;
	[HideInInspector] public bool Success;
	protected bool _started = false;

	protected FishItem _fishItem;

	private void FixedUpdate()
	{
		if (!_started || Complete) return;
		_onUpdate();
	}

	protected abstract void _init();
	public void Init(FishItem fish)
	{
		_fishItem = fish;
		Complete = Success = false;

		_init();
	}

	protected abstract void _onUpdate();
	protected abstract void _onGameOver();
	public void OnGameOver(bool success)
	{
		Success = success;
		_started = false;

		_onGameOver();
	}

	private void OnEnable() => _action.Enable();
	private void OnDisable() => _action.Disable();
}
