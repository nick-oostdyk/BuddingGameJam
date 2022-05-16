using System.Collections;
using UnityEngine;
using System.Threading.Tasks;

public class Player : Entity
{
	public static PlayerInventory InventoryWrapper;

	[SerializeField] private SpriteRenderer _toolSR;
	[SerializeField] private Sprite[] _toolSprites;

	public PromptObject PromptObject;
	private IInteractable _interactTarget;
	private PlayerInputHandler _controls;
	private Animator _animator;

	private void Start()
	{
		GameManager.Instance.OnGameStateChanged += _stateChangeHandler;

		InventoryWrapper = new PlayerInventory();

		PromptObject = new PromptObject(transform);
		_controls = GetComponent<PlayerInputHandler>();
		_animator = GetComponent<Animator>();
		_toolSR.enabled = false;
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

	public async void Interact()
	{
		// if there is no interact target return
		if (_interactTarget is null) return;

		try
		{
			// if the interact target is a resource
			var r = _interactTarget as Resource;
			bool canHarvest = true;
			if (r is not null)
				if (!await _harvestResource(r)) return;
		}
		catch (System.Exception e) { print(e.Message); }

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

	private async Task<bool> _harvestResource(Resource resource)
	{
		int numSwings = 0;
		var canHarvest = true;
		var flags = GameManager.Instance.GameFlags;
		Sprite toolSprite = _toolSprites[0];

		// no tools
		if (!flags.HasFlag(GameFlag.TOOL_ONE))
		{
			numSwings = 3;

			switch (resource.Type)
			{
				case ResourceType.BOULDER:
				case ResourceType.TREE:
					canHarvest = false;
					break;
			}
		}
		// has tool 1
		else if (!flags.HasFlag(GameFlag.TOOL_TWO))
		{
			toolSprite = _toolSprites[1];
			switch (resource.Type)
			{
				case ResourceType.FIBER:
				case ResourceType.STONE:
				case ResourceType.DRIFTWOOD:
					numSwings = 2;
					break;
				case ResourceType.BOULDER:
					numSwings = 4;
					break;
				case ResourceType.TREE:
					numSwings = 4;
					break;
			}
		}
		// has tool 2
		else
		{
			toolSprite = _toolSprites[2];
			switch (resource.Type)
			{
				case ResourceType.FIBER:
				case ResourceType.STONE:
				case ResourceType.DRIFTWOOD:
					numSwings = 1;
					break;
				case ResourceType.BOULDER:
					numSwings = 2;
					break;
				case ResourceType.TREE:
					numSwings = 3;
					break;
			}
		}

		if (!canHarvest)
		{
			FindObjectOfType<Player>().StopMovementImmediate();
			DialogueBoxManager.Instance.PushText("", "I need a better tool to harvest this.");
			return false;
		}

		// swing anim & player input lock
		GameManager.Instance.SetState(GameState.HARVEST);
		_toolSR.enabled = true;
		_toolSR.sprite = toolSprite;

		Util.PlayAndWaitForAnim(_animator, "SwingTool", numSwings, 1);

		// set end anim trigger and hide tool
		_animator.SetTrigger("FinishInteract");
		_toolSR.enabled = false;

		// re-enable player input
		GameManager.Instance.SetState(GameState.PLAY);

		return true;
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

	// actually just hides it
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