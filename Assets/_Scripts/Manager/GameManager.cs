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

	private void Start()
	{
		_playOpening();
	}

	public void ChangeState(GameState state)
	{
		if (State == state) return;
		State = state;
		OnGameStateChanged?.Invoke(state);
	}

	private void _playOpening()
	{
		ChangeState(GameState.CUTSCENE);
	}
}
