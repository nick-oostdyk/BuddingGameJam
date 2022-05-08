using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
	PLAYER_CAMERA,
	FISHING_CAMERA,
}

public class CinemachineStateSwitcher : MonoBehaviour
{
	private Animator _animator;

	private Dictionary<CameraState, string> _stateNames = new Dictionary<CameraState, string>()
	{
		{ CameraState.PLAYER_CAMERA, "PlayerCamera" },
		{ CameraState.FISHING_CAMERA, "FishingCamera" },
	};

	private void Start() => _animator = GetComponent<Animator>();
	public void SwitchState(CameraState state) => _animator.Play(_stateNames[state]);
}
