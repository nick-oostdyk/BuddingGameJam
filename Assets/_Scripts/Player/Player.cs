using System.Collections;
using UnityEngine;

public class Player : Entity
{
	public static PlayerInventory InventoryWrapper;

	public PromptObject PromptObject;
	private IInteractable _interactTarget;
	private PlayerInputHandler _controls;
	private GameObject _tool;

	private void Start()
	{
		GameManager.Instance.OnGameStateChanged += _stateChangeHandler;

		InventoryWrapper = new PlayerInventory();

		PromptObject = new PromptObject(transform);
		_controls = GetComponent<PlayerInputHandler>();

		_tool = GameObject.Find("/Player/ToolHandle/Tool");
		_tool.GetComponent<SpriteRenderer>().enabled = false;
	}

	private void _stateChangeHandler(GameState state)
	{
		var sceneLock = PlayerInputHandler.LockState.SCENE;
		switch (state)
		{
			case GameState.PLAY:
			case GameState.CAVE:
				_controls.Unlock(sceneLock);
				break;

			default:
				_controls.Lock(sceneLock);
				break;

		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		// set interact target
		if (other.TryGetComponent<IInteractable>(out var interactable))
			_interactTarget = interactable;

		TryShowPrompt();
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		PromptObject.SetEnabled(false);
		_interactTarget = null;
	}

	public void Interact()
	{
		if (_interactTarget == null)	return;

		// enable tool during interact & play anim
		// not sure how to disable after the anim but wip
		_tool.GetComponent<SpriteRenderer>().enabled = true;
		GetComponent<Animator>().Play("WeaponLayer.SwingTool");

		_interactTarget.Interact();
	}

	public void TryShowPrompt()
	{
		if (PromptObject.Enabled) return;

		var hits = Physics2D.OverlapCircleAll(transform.position, .5f);

		if (hits.Length > 0) PromptObject.Reset();

		foreach (var hit in hits)
		{
			// if collided obj is interactable
			if (hit.TryGetComponent<IInteractable>(out var comp))
			{
				// set sprite
				PromptObject.SetSprite(
				hit.TryGetComponent<FishingSpot>(out var _)
				? PromptObject.FishingIcon
				: PromptObject.EKey);

				// display prompt
				PromptObject.SetEnabled(true);
			}
		}
	}
}

public class PromptObject
{
	public static Sprite EKey;
	public static Sprite FishingIcon;

	private GameObject _object;
	private SpriteRenderer _sr;

	public bool Enabled { get; private set; }

	public PromptObject(Transform parent)
	{
		_initSprites();

		// create object and set parent & position
		_object = new GameObject();
		_object.transform.parent = parent;
		Reset();

		// set up spriterenderer on prompt object
		_sr = _object.AddComponent<SpriteRenderer>();
		_sr.sprite = EKey;
		_sr.sortingOrder = 10;
		_sr.sortingLayerName = "Midground";

		SetEnabled(false);
	}

	public void SetEnabled(bool enabled)
	{
		Enabled = enabled;
		_sr.color = new Color(1f, 1f, 1f, enabled ? 1f : 0f);
	}

	public void SetSprite(Sprite sprite) => _sr.sprite = sprite;
	public void SetPosition(Vector3 position) => _object.transform.position = position;
	public void SetScale(Vector3 scale) => _object.transform.localScale = scale;
	public void Reset()
	{
		_object.transform.position = _object.transform.parent.position;
		_object.transform.localPosition = Vector3.up * 0.75f + Vector3.right;
		_object.transform.localScale = Vector3.one;
	}

	private void _initSprites()
	{
		EKey = Resources.Load<Sprite>("Sprites/Keys/E_Key_Light");
		FishingIcon = Resources.Load<Sprite>("Sprites/Keys/Fish_Icon");
	}
}