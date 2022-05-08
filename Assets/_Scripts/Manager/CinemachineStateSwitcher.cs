using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineStateSwitcher : MonoBehaviour
{
	private Animator _animator;

	public enum State
	{
		PLAYER_CAMERA,
		FISHING_CAMERA,
	}

	private Dictionary<State, string> _stateNames = new Dictionary<State, string>()
	{
		{ State.PLAYER_CAMERA, "PlayerCamera" },
		{ State.FISHING_CAMERA, "FishingCamera" },
	};

	private void Start() => _animator = GetComponent<Animator>();
	public void SwitchState(State state) => _animator.Play(_stateNames[state]);
}
