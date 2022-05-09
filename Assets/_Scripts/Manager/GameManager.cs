using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum GameState
{
	LOAD,
	CUTSCENE,
	PLAY,
	FISH,
}

public class GameManager : MonoBehaviour
{
	public static GameState State { get; private set; }
	public static System.Action<GameState> OnGameStateChanged;

	private Player _player;
	[SerializeField] private Transform _playerSpawnPosition;

	private void Start()
	{
		_playOpening();
	}

	public void ChangeState(GameState state)
	{
		if (State == state) return;
		State = state;
		print($"new state {state}");
		OnGameStateChanged?.Invoke(state);
	}

	private async void _playOpening()
	{
		var vignetteController = FindObjectOfType<CustomVignetteController>();
		vignetteController.SetWeight(1f);

		await Task.Delay(500);

		ChangeState(GameState.CUTSCENE);

		_player = FindObjectOfType<Player>();
		_player.transform.position = _playerSpawnPosition.position;

		DialogueBoxManager.Instance.PushSquence("", new DialogueBoxManager.TextSequence(new string[] {
			"You wake up.",
			"You feel sand sticking to your skin.",
			"The sun is beating down on your face."
			}));

		DialogueBoxManager.Instance.OnCurrentSequenceFinish += async () => {
			await vignetteController.FadeFromBlack(1.5f);
			ChangeState(GameState.PLAY);
		};

	}
}
