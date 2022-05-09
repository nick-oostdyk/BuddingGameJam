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

	[SerializeField] private FishingMinigame[] _minigames;

	private void Start()
	{
		_player = FindObjectOfType<Player>();

		_releasePoint = _player.transform.Find("FishingReleasePoint");
		_bobber = transform.GetChild(0).gameObject;

		_bobber.SetActive(false);
		_minigames[0].gameObject.SetActive(false);
		_minigames[1].gameObject.SetActive(false);

		_fishArr = Resources.LoadAll<FishItem>("ScriptableObjects/Fish");
		GetComponent<FishingSpot>().OnInteract += (s, a) => _onFish();

		_reelAction.performed += _ => {
			if (_canReel && _onBiteCoEnum != null)
			{
				_reel = true;
				StopCoroutine(_onBiteCoEnum);
				_onBiteCoEnum = null;
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

		// reset state
		_bobber.GetComponent<Animator>().SetTrigger("Release");
		_bobber.SetActive(false);

		// change to player camera
		camSwitcher.SwitchState(CameraState.PLAYER_CAMERA);

		await Task.Delay(1000);
		playerInput.enabled = true;
	}

	private async Task _playMinigames()
	{
		// only advance to the next game after the preceding game has been won
		if (!await _castBobber()) return;
		if (!await _playGame(_minigames[0])) return;
		if (!await _playGame(_minigames[1])) return;
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

		// show bobber, move minigames to bobber location
		_bobber.SetActive(true);
		_bobber.transform.position = _player.transform.position + bobberOffset;
		_minigames[0].transform.position = _bobber.transform.position;
		_minigames[1].transform.position = _bobber.transform.position;

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
		_reel = false;
		_canReel = true;

		// reel-in leniency
		yield return new WaitForSeconds(Random.Range(1f, 2f));

		// reel failed
		_reel = false; 
		_onBiteCoEnum = null;
	}

	private async Task<bool> _playGame(FishingMinigame game)
	{
		game.gameObject.SetActive(true);
		game.Init(_fish);

		while (!game.Complete)
			await Task.Yield();

		game.gameObject.SetActive(false);
		return game.Success;
	}

	private void OnEnable() => _reelAction.Enable();
	private void OnDisable() => _reelAction.Disable();
}
