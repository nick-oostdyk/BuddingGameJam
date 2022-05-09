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

		NONE,
		NO_OF_BEATS,
	}

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
			var theta = (index + 1) * Mathf.PI / 2;
			Vector3 offset = Vector3.zero;
			offset.x = Mathf.Cos(theta);
			offset.y = Mathf.Sin(theta);

			go.transform.parent = parent.transform;
			go.transform.position = parent.transform.position + offset;
			go.transform.localScale = Vector3.one;

			var sr = go.AddComponent<SpriteRenderer>();
			sr.color = Color.black;
			sr.sprite = sprite;
			sr.sortingLayerName = "Foreground";
			sr.sortingOrder = 15;

			col = go.AddComponent<CircleCollider2D>();
			col.isTrigger = true;
			col.radius = .5f;
		}

		private void _buildChild(Sprite sprite, Material whiteMat)
		{
			var child = new GameObject();
			child.transform.parent = go.transform;
			child.transform.position = go.transform.position;
			child.transform.localScale = Vector3.one;

			_childRenderer = child.AddComponent<SpriteRenderer>();
			_childRenderer.color = Color.clear;
			_childRenderer.sprite = sprite;
			_childRenderer.sortingLayerName = "Foreground";
			_childRenderer.sortingOrder = 16;
			_childRenderer.material = whiteMat;
		}
	}

	[SerializeField] private FishingGameBobble _bobble;
	private FishObject[] _fishObjects;
	private Beat[] _pattern;

	private State _state;

	protected override void _init()
	{
		_state = State.VIEW;
		_generateFishObjects();
		_generateBeatPattern();
		_viewPattern();
	}

	private void FixedUpdate()
	{
		_updatePlayerPosition(_action.ReadValue<Vector2>());
	}

	private void _generateFishObjects()
	{
		var sprite = _fishItem.Sprite;
		_fishObjects = new FishObject[4];

		for (int i = 0; i < 4; ++i)
			_fishObjects[i] = new FishObject(transform, sprite, _whiteoutMaterial, i);
	}

	private void _generateBeatPattern()
	{
		_pattern = new Beat[_fishItem.NumBeats];
		for (int i = 0; i < _pattern.Length; ++i)
		{
			if (Random.Range(0, 6) != 0)
				_pattern[i] = (Beat)Random.Range(0, 4);
			else
				_pattern[i] = Beat.NONE;
		}
	}

	private async void _viewPattern()
	{
		await Task.Delay(800);
		var beatQueue = new Queue<Beat>(_pattern);
		foreach (var beat in beatQueue)
		{
			print(beat);
			if (beat != Beat.NONE)
				_fishObjects[(short)beat].Flash();
			await Task.Delay(800);
		}
		await Task.Delay(2000);
		_state = State.PLAY;
		_started = true;
	}


	private void _updatePlayerPosition(Vector2 movementInput)
	{
		print(movementInput);
		_bobble.SetLocalPosition(movementInput);

		switch (_state)
		{
			case State.VIEW:
				_bobble.ClampPosition(.2f);
				break;
			case State.PLAY:
				_bobble.ClampPosition(1f);
				break;
		}
	}

	protected override void _onGameOver() { }
	protected override void _onUpdate() { }
}
