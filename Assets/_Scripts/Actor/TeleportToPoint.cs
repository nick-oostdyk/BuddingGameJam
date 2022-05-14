using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToPoint : MonoBehaviour
{
	[SerializeField] private bool _tpOnTriggerEnter;
	private Transform _telepoint;

	private void Start() => _telepoint = transform.GetChild(0);

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!_tpOnTriggerEnter) return;
		if (other.tag != "Player") return;

		Teleport();
	}

	public async void Teleport()
	{
		var fade = FindObjectOfType<CustomVignetteController>();
		var player = FindObjectOfType<Player>();
		var input = player.GetComponent<PlayerInputHandler>();

		// stop movement & disable input
		player.StopMovementImmediate();
		input.Lock(PlayerInputHandler.LockState.TELEPORT);
		await fade.FadeToBlack(1f);

		// set state depending on if the player is entering or leaving the cave
		var state = GameManager.Instance.State;
		GameManager.Instance.SetState(state == GameState.PLAY ? GameState.CAVE : GameState.PLAY);

		// teleports the player to the new position
		player.SetPosition(_telepoint.position);
		await fade.FadeFromBlack(1f);

		input.Unlock(PlayerInputHandler.LockState.TELEPORT);
	}
}
