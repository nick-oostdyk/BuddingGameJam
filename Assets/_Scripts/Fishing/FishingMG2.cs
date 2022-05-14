using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FishingMG2 : FishingMinigame
{
	[SerializeField] Material _whiteoutMaterial;

	private enum State
	{
		VIEW,
		PLAY,
	}

	private enum Beat : short
	{
		UP,
		LEFT,
		DOWN,
		RIGHT,

		NO_OF_BEATS,
	}

	// the sprite that flashes white in Fishing phase 3
	private class FishObject
	{
		public GameObject go { get; private set; }
		public CircleCollider2D col { get; private set; }

		private SpriteRenderer _childRenderer;

		public FishObject(Transform parent, Sprite sprite, Material whiteMat, int index)
		{
			go = new GameObject();
			_buildObj(parent, sprite, index);
			_buildChild(sprite, whiteMat);
		}

		public async void Flash()
		{
			// math
			var frameTimeMS = 4;
			var dtSeconds = frameTimeMS * .001f;
			var riseDurationSeconds = .1f;
			var fallDurationSeconds = .3f;

			var riseIterations = riseDurationSeconds / dtSeconds;
			var fallIterations = fallDurationSeconds / dtSeconds;

			var deltaRise = 1f / riseIterations;
			var deltaFall = 1f / fallIterations;

			// rise
			Color transpWhite = new Color(1f, 1f, 1f, 0f);
			var progress = 0f;
			for (int i = 0; i < riseIterations; ++i)
			{
				_childRenderer.color = Color.Lerp(transpWhite, Color.white, progress);
				progress += deltaRise;
				await Task.Delay(frameTimeMS);
			}

			// fall
			var sr = go.GetComponent<SpriteRenderer>();
			progress = 0f;
			for (int i = 0; i < fallIterations; ++i)
			{
				_childRenderer.color = Color.Lerp(Color.white, transpWhite, progress);
				sr.color = Color.Lerp(Color.white, Color.black, progress);
				progress += deltaFall;
				await Task.Delay(frameTimeMS);
			}
		}

		private void _buildObj(Transform parent, Sprite sprite, int index)
		{
			go.name = ((Beat)index).ToString();

			// position
			var theta = (index + 1) * Mathf.PI / 2;
			Vector3 offset = Vector3.zero;
			offset.x = Mathf.Cos(theta);
			offset.y = Mathf.Sin(theta);

			go.transform.parent = parent.transform;
			go.transform.position = parent.transform.position + offset;
			go.transform.localScale = Vector3.one;
			
			// sprite renderer
			var sr = go.AddComponent<SpriteRenderer>();
			sr.color = Color.black;
			sr.sprite = sprite;
			sr.sortingLayerName = "Foreground";
			sr.sortingOrder = 15;

			// collider
			col = go.AddComponent<CircleCollider2D>();
			col.isTrigger = true;
			col.radius = .5f;
		}

		// this is the sprite with the whiteout material that overlays the fish
		private void _buildChild(Sprite sprite, Material whiteMat)
		{
			// position & parent
			var child = new GameObject();
			child.transform.parent = go.transform;
			child.transform.position = go.transform.position;
			child.transform.localScale = Vector3.one;

			// sprite renderer
			_childRenderer = child.AddComponent<SpriteRenderer>();
			_childRenderer.color = Color.clear;
			_childRenderer.sprite = sprite;
			_childRenderer.sortingLayerName = "Foreground";
			_childRenderer.sortingOrder = 16;
			_childRenderer.material = whiteMat;
		}
	}

	[SerializeField] private FishingGameBobble _bobble;
	[SerializeField] private GameObject _ring;

	private FishObject[] _fishObjects;
	private Beat[] _pattern;

	private State _state;

	protected async override void _init()
	{
		_state = State.VIEW;
		_ring.transform.localScale = Vector3.zero;
		_generateFishObjects();
		_generateBeatPattern();
		await _viewPattern();
		await _playGame();
	}

	// tick player
	private void FixedUpdate() => _updatePlayerPosition(_action.ReadValue<Vector2>());

	private void _generateFishObjects()
	{
		var sprite = _fishItem.Sprite;
		_fishObjects = new FishObject[4];

		for (int i = 0; i < 4; ++i)
			_fishObjects[i] = new FishObject(transform, sprite, _whiteoutMaterial, i);
	}

	// creates a random pattern of beats
	private void _generateBeatPattern()
	{
		_pattern = new Beat[_fishItem.NumBeats];
		for (int i = 0; i < _pattern.Length; ++i)
			_pattern[i] = (Beat)Random.Range(0, 4);
	}

	// displays the pattern
	private async Task _viewPattern()
	{
		var noteTime = _fishItem.NoteTime;

		var beatQueue = new Queue<Beat>(_pattern);
		foreach (var beat in beatQueue)
		{
			await Task.Delay(Mathf.RoundToInt(noteTime * 1000));
			_fishObjects[(short)beat].Flash();
		}
		await Task.Delay(Mathf.RoundToInt(noteTime * 1000));
	}

	// now its the players turn
	private async Task _playGame()
	{
		_state = State.PLAY;

		// create a queue from the pattern generated earlier
		var beatQueue = new Queue<Beat>(_pattern);

		var msPerFrame = 10;
		var dtSeconds = msPerFrame * 0.001f;

		var noteDurationSeconds = _fishItem.NoteTime;
		var numIters = noteDurationSeconds / dtSeconds;

		var deltaScale = 1f / numIters;

		foreach (var beat in beatQueue)
		{
			// grow ring
			var progress = 0f;
			for (int i = 0; i < numIters; ++i)
			{
				_ring.transform.localScale = Vector3.one * progress;
				progress += deltaScale;
				await Task.Delay(msPerFrame);
			}

			// this is the moment where the ring hits its max size
			var hit = Physics2D.OverlapCircle(_bobble.transform.position, .5f);
			if (hit != null && hit.name == beat.ToString())
				_fishObjects[(short)beat].Flash();
			else
			{
				// if the player misses the beat
				OnGameOver(false);
				return;
			}
		}
		// if the player succeeded
		OnGameOver(true);
	}

	private void _updatePlayerPosition(Vector2 movementInput)
	{
		_bobble.SetLocalPosition(movementInput * 5f);

		switch (_state)
		{
			case State.VIEW:
				_bobble.ClampPosition(.25f);
				break;
			case State.PLAY:
				_bobble.ClampPosition(2f);
				break;
		}
	}

	protected override void _onUpdate() { }
	protected async override Task _onGameOver()
	{
		_ring.transform.localScale = Vector3.zero;
		await Task.Delay(1000);

		foreach (var fish in _fishObjects)
			Destroy(fish.go);
	}
}
