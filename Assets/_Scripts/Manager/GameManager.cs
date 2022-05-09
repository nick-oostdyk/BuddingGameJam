using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
	LOAD,
	CUTSCENE,
	DIALOGUE,
	PLAY,
	FISH,
}

public class GameManager : MonoBehaviour
{
	public static GameState State { get; private set; }
	public static System.Action<GameState> OnGameStateChanged;

	private async void Start()
	{
		await System.Threading.Tasks.Task.Delay(50);
		//_playOpening();
	}

	public void ChangeState(GameState state)
	{
		if (State == state) return;
		State = state;
		OnGameStateChanged?.Invoke(state);
	}

	private void _playOpening()
	{
		ChangeState(GameState.DIALOGUE);
		DialogueBoxManager.Instance.PushSquence("", new DialogueBoxManager.TextSequence(new string[] {
			"You wake up.",
			"You feel sand sticking to your skin.",
			"The sun is beating down on your face."
			}));
		DialogueBoxManager.Instance.OnCurrentSequenceFinish += () => { print("Sequence Finished"); };
	}
}
