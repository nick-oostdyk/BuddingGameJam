using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
	private Player _player;
	private Transform _releasePoint;
	private GameObject _bobber;

	private FishItem _fish;

	private void Start()
	{
		_player = FindObjectOfType<Player>();
		_releasePoint = _player.transform.Find("FishingReleasePoint");
		_bobber = transform.GetChild(0).gameObject;
		_bobber.SetActive(false);

		GetComponent<FishingSpot>().OnInteract += (s, a) => _onFish();
	}

	private void _onFish()
	{
		var playerInput = _player.GetComponent<PlayerInputHandler>();

		_player.StopAllMovementImmediate();
		playerInput.enabled = false;

		var bobberOffset = new Vector3(-3.5f, .5f);

		_bobber.SetActive(true);
		_bobber.transform.position = _player.transform.position + bobberOffset;

		// select which fish to spawn

		// play minigame 1

		// play minigame 2

		playerInput.enabled = true;
	}

	private bool _minigameOne()
	{
		return true;
	}
}
