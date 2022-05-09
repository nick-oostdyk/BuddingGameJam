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
	protected abstract System.Threading.Tasks.Task _onGameOver();
	public async void OnGameOver(bool success)
	{
		await _onGameOver();

		Success = success;
		Complete = true;
		_started = false;
	}

	private void OnEnable() => _action.Enable();
	private void OnDisable() => _action.Disable();
}
