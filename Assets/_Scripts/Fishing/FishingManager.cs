using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishingManager : MonoBehaviour
{
	[SerializeField] private InputAction _reelAction;
	private bool _reel;
	private bool _canReel;

	private Player _player;
	private Transform _releasePoint;
	private GameObject _bobber;

	private FishItem[] _fishArr;
	private FishItem _fish;

	private IEnumerator _onBiteCoEnum;

	private FishingGameOne _fishingGameOne;

	private void Start()
	{
		_player = FindObjectOfType<Player>();
		_fishingGameOne = FindObjectOfType<FishingGameOne>();

		_releasePoint = _player.transform.Find("FishingReleasePoint");
		_bobber = transform.GetChild(0).gameObject;

		_bobber.SetActive(false);
		_fishingGameOne.gameObject.SetActive(false);

		_fishArr = Resources.LoadAll<FishItem>("ScriptableObjects/Fish");
		GetComponent<FishingSpot>().OnInteract += (s, a) => _onFish();

		_reelAction.performed += _ => {
			if (_canReel && _onBiteCoEnum != null)
			{
				_reel = true;
				StopCoroutine(_onBiteCoEnum);
				_onBiteCoEnum = null;
				print("co cancelled");
			}
		};
	}

	private async void _onFish()
	{
		var camSwitcher = FindObjectOfType<CinemachineStateSwitcher>();
		var playerInput = _player.GetComponent<PlayerInputHandler>();

		_player.StopMovementImmediate();
		playerInput.enabled = false;

		_fish = _fishArr[Random.Range(0, _fishArr.Length)];

		camSwitcher.SwitchState(CameraState.FISHING_CAMERA);

		await _playMinigames();

		// give player fish

		_bobber.GetComponent<Animator>().SetTrigger("Release");
		_bobber.SetActive(false);

		// change to player camera
		camSwitcher.SwitchState(CameraState.PLAYER_CAMERA);

		await Task.Delay(150);
		playerInput.enabled = true;
	}

	private async Task _playMinigames()
	{
		// only advance to the next game after the preceding game has been won
		if (!await _castBobber()) return;
		if (!await _minigameOne()) return;
		if (!await _minigameTwo()) return;
	}

	private async Task<bool> _castBobber()
	{
		// start coroutine and wait for it to finish
		_canReel = false;
		_onBiteCoEnum = _onBiteCo();
		StartCoroutine(_onBiteCoEnum);
		while (_onBiteCoEnum != null) await Task.Yield();

		// hide prompt
		_player.PromptObject.SetEnabled(false);
		_player.TryShowPrompt();

		return _reel;
	}

	private IEnumerator _onBiteCo()
	{
		var bobberOffset = new Vector3(-5f, 2f);
		var promptOffset = new Vector3(0f, 0.75f);

		_bobber.SetActive(true);
		_bobber.transform.position = _player.transform.position + bobberOffset;
		_fishingGameOne.transform.position = _bobber.transform.position;

		// wait for fish to come
		yield return new WaitForSeconds(Random.Range(1f, 3f));
		_bobber.GetComponent<Animator>().SetTrigger("Catch");

		// show prompt
		var prompt = _player.PromptObject;
		prompt.SetEnabled(true);
		prompt.SetSprite(PromptObject.EKey);
		prompt.SetPosition(_bobber.transform.position + promptOffset);
		prompt.SetScale(Vector3.one * 0.6f);

		// allow player to attempt catch
		_reel = false; // clear input
		_canReel = true;
		yield return new WaitForSeconds(Random.Range(1f, 2f)); // reel-in leniency
		_reel = false; // reel failed
		print("co finished");
		_onBiteCoEnum = null;
	}

	private async Task<bool> _minigameOne()
	{
		_fishingGameOne.gameObject.SetActive(true);

		_fishingGameOne.Init(_fish);
		while (!_fishingGameOne.Complete)
			await Task.Yield();

		_fishingGameOne.gameObject.SetActive(false);
		return _fishingGameOne.Success;
	}

	private async Task<bool> _minigameTwo()
	{
		await Task.Yield();
		return false;
	}

	private void OnEnable() => _reelAction.Enable();
	private void OnDisable() => _reelAction.Disable();
}
