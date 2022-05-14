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

		// hide everything
		_bobber.SetActive(false);
		_minigames[0].gameObject.SetActive(false);
		_minigames[1].gameObject.SetActive(false);

		// grab all fish assets
		_fishArr = Resources.LoadAll<FishItem>("ScriptableObjects/Fish");

		// add fishing behaviour to interact event
		GetComponent<FishingSpot>().OnInteract += (s, a) => _onFish(); 

		// add event to action for when player can reel in the fish
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
		var playerInput = _player.GetComponent<PlayerInputHandler>();
		var fishLock = PlayerInputHandler.LockState.FISH;

		_player.StopMovementImmediate();
		playerInput.Lock(fishLock);

		_fish = _fishArr[Random.Range(0, _fishArr.Length)];

		GameManager.Instance.SetState(GameState.FISH);

		// wait for the minigames to finish
		await _playMinigames();

		// give player fish

		// reset state
		_bobber.GetComponent<Animator>().SetTrigger("Release");
		_bobber.SetActive(false);

		// set game state back to play
		GameManager.Instance.SetState(GameState.PLAY);

		await Task.Delay(1000);
		playerInput.Unlock(fishLock);
	}

	private async Task _playMinigames()
	{
		// only advance to the next game after the preceding game has been won
		if (!await _castBobber()) return;
		if (!await _playGame(_minigames[0])) return;
		if (!await _playGame(_minigames[1])) return;

		print($"Fish Caught: {_fish.Type}!");
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
		// setup game
		game.gameObject.SetActive(true);
		game.Init(_fish);

		// wait for the game to finish
		while (!game.Complete)
			await Task.Yield();

		// close game & return success or nah
		game.gameObject.SetActive(false);
		return game.Success;
	}

	private void OnEnable() => _reelAction.Enable();
	private void OnDisable() => _reelAction.Disable();
}
