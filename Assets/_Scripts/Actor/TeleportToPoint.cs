using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToPoint : MonoBehaviour
{
	[SerializeField] private bool _tpOnTriggerEnter;
	private Transform _telepoint;

	private void Start()
	{
		_telepoint = transform.GetChild(0);
	}

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

		player.StopMovementImmediate();
		input.enabled = false;
		await fade.FadeToBlack(1f);
		
		player.SetPosition(_telepoint.position);
		await fade.FadeFromBlack(1f);

		input.enabled = true;
	}
}
