using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
	private Player _player;
	private Transform _releasePoint;
	private GameObject _bobber;

	private FishItem[] _fishArr;
	private FishItem _fish;

	private FishingGameOne _fishingGameOne;

	private void Start()
	{
		_player = FindObjectOfType<Player>();
		_releasePoint = _player.transform.Find("FishingReleasePoint");
		_bobber = transform.GetChild(0).gameObject;

		_fishingGameOne = FindObjectOfType<FishingGameOne>();
		_fishingGameOne.gameObject.SetActive(false);

		_bobber.SetActive(false);

		_fishArr = Resources.LoadAll<FishItem>("ScriptableObjects/Fish");
		GetComponent<FishingSpot>().OnInteract += (s, a) => _onFish();
	}

	private async void _onFish()
	{
		var camSwitcher = FindObjectOfType<CinemachineStateSwitcher>();
		var playerInput = _player.GetComponent<PlayerInputHandler>();

		_player.StopMovementImmediate();
		playerInput.enabled = false;

		_castBobber();

		// select which fish to spawn
		_fish = _fishArr[Random.Range(0, _fishArr.Length)];

		// change to fish camera
		camSwitcher.SwitchState(CameraState.FISHING_CAMERA);

		await Task.Delay(Random.Range(750, 2250));

		await _playMinigames();

		// give player fish


		_bobber.SetActive(false);

		// change to player camera
		camSwitcher.SwitchState(CameraState.PLAYER_CAMERA);

		playerInput.enabled = true;
	}

	private void _castBobber()
	{
		var bobberOffset = new Vector3(-5f, 2f);

		_bobber.SetActive(true);
		_bobber.transform.position = _player.transform.position + bobberOffset;
	}

	private async Task _playMinigames()
	{
		if (!await _minigameOne()) return;

		if (!await _minigameTwo()) return;
	}

	private async Task<bool> _minigameOne()
	{
		_fishingGameOne.gameObject.SetActive(true);

		_fishingGameOne.Init(_fish);
		while (!_fishingGameOne.Complete)
			await Task.Delay(50);

		_fishingGameOne.gameObject.SetActive(false);
		return _fishingGameOne.Success;
	}

	private async Task<bool> _minigameTwo()
	{
		await Task.Delay(1);
		return false;
	}


}
