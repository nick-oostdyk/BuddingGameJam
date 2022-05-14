using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum GameState
{
	LOAD,
	CUTSCENE,
	HARVEST,
	PLAY,
	FISH,
	CAVE,
	SLEEP,
}

[System.Flags]
public enum Flags : int
{
	NONE = 0b0,
	TOOL_ONE = 0b1,
	TOOL_TWO = 0b1 << 1,
	FISH_ROD = 0b1 << 2,

	CAVE_PROMPT = 0b1 << 3,
}

public class GameManager : MonoBehaviour
{
	// singleton pattern -----
	private static GameManager _i;
	public static GameManager Instance => _i;

	private void Awake()
	{
		if (_i == null) _i = this;
		else Destroy(this);
	}
	// -----

	public GameState State { get; private set; }
	public Flags GameFlags { get; private set; }
	public System.Action<GameState> OnGameStateChanged;

	private Player _player;
	[SerializeField] private Transform _playerSpawnPosition;
	[SerializeField] private Animator _gameStartCutscene;
	[SerializeField] private ToggleUI _timerToggle;

	[SerializeField] private bool _playCutscene = true;

	private void Start()
	{
		if (_playCutscene) // debug ahoy
			_playOpening();
		else
		{
			SetState(GameState.PLAY);
			TimeManager.StartTimer();
			_timerToggle.Toggle();
		}
	}

	public void SetState(GameState state)
	{
		if (State == state) return;
		State = state;
		print($"new state {state}");
		OnGameStateChanged?.Invoke(state);
	}

	private async void _playOpening()
	{
		var dialogueBox = DialogueBoxManager.Instance;

		var vignetteController = FindObjectOfType<CustomVignetteController>();
		vignetteController.SetWeight(1f);

		await Task.Delay(500);

		SetState(GameState.CUTSCENE);

		// move player to cutscene location
		_player = FindObjectOfType<Player>();
		_player.transform.position = _playerSpawnPosition.position;

		// push opening dialogue
		dialogueBox.PushSequence("", new DialogueBoxManager.TextSequence(new string[] {
			"You wake up.",
			"You feel sand sticking to your skin.",
			"The sun is beating down on your face."
			}));

		// play paper plane cutscene after dialogue is finished
		dialogueBox.OnCurrentSequenceFinish += _playCutsceneOne;
	}

	public async void _playCutsceneOne()
	{
		var dialogueBox = DialogueBoxManager.Instance;
		var vignetteController = FindObjectOfType<CustomVignetteController>();

		// fade in
		await vignetteController.FadeFromBlack(1.5f);
		_gameStartCutscene.Play("GameStart");

		// wait for animation to start
		while (!_gameStartCutscene.GetCurrentAnimatorStateInfo(0).IsName("GameStart"))
			await Task.Yield();

		// wait for animation to end
		while (_gameStartCutscene.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
			await Task.Yield();

		dialogueBox.PushText("", "There's something written on the paper plane.");
		dialogueBox.PushSequence("Paper Plane", new DialogueBoxManager.TextSequence(new string[] {
				"We saw your balloon go down over the island!",
				"Please, stay put,",
				"we'll be back with a rescue team,",
				"IN THREE DAYS",
				}));

		// allow player movement after sequence finishes
		dialogueBox.OnCurrentSequenceFinish += () => {
			SetState(GameState.PLAY);
			TimeManager.StartTimer();
			_timerToggle.Toggle();

			// play additional help text after 3.5s 
			Util.DelayRunAction(3500, () => _pushGameStartHelp());
		};
	}

	private void _pushGameStartHelp()
	{
		// additional help text
		var dialogueBox = DialogueBoxManager.Instance;
		dialogueBox.PushSequence("", new DialogueBoxManager.TextSequence(new string[] {
				"How am I supposed to survive for three days?",
				"I guess I need to find some way to get FOOD and WATER.",
				"I can start by gathering some resources,",
				"Some of these sticks and stones look light enough to carry.",
				}));
	}
}

public static class Util
{
	public static async Task DelayRunAction(int delayMS, System.Action action)
	{
		await Task.Delay(delayMS);
		action();
	}
}