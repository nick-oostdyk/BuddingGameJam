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
		var fade = FindObjectOfType<TeleportVignetteController>();
		var player = FindObjectOfType<Player>();
		var input = player.GetComponent<PlayerInputHandler>();

		player.StopMovementImmediate();
		input.enabled = false;
		await fade.FadeOut();
		
		player.SetPosition(_telepoint.position);
		await fade.FadeIn();

		input.enabled = true;
	}
}
