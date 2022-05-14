using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
	PLAYER_CAMERA,
	FISHING_CAMERA,
	CAVE_CAMERA,
}

// handles which camera is active by setting the animation state
// camera changes happen on state changes
public class CinemachineStateSwitcher : MonoBehaviour
{
	private Animator _animator;

	private Dictionary<CameraState, string> _stateNames = new Dictionary<CameraState, string>()
	{
		{ CameraState.PLAYER_CAMERA, "PlayerCamera" },
		{ CameraState.FISHING_CAMERA, "FishingCamera" },
		{ CameraState.CAVE_CAMERA, "CaveCamera" },
	};
	public void SwitchState(CameraState state) => _animator.Play(_stateNames[state]);

	private void Start()
	{
		_animator = GetComponent<Animator>();

		GameManager.Instance.OnGameStateChanged += state => {

			switch (state)
			{
				case GameState.FISH:
					SwitchState(CameraState.FISHING_CAMERA);
					break;

				case GameState.CAVE:
					SwitchState(CameraState.CAVE_CAMERA);
					break;

				default:
					SwitchState(CameraState.PLAYER_CAMERA);
					break;
			}
		};
	}

}
