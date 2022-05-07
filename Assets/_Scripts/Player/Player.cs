using System.Collections;
using UnityEngine;

public class Player : Entity
{
	public static PlayerInventory InventoryWrapper;

	private PromptObject _promptObject;
	private IInteractable _interactTarget;

	private void Start()
	{
		InventoryWrapper = new PlayerInventory();

		_promptObject = new PromptObject(transform);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		// typecheck other as interactable
		var otherInteractable = other.GetComponent<IInteractable>();

		if (otherInteractable == null) return;

		// cache target if interactable
		_interactTarget = otherInteractable;

		_promptObject.SetSprite(
			other.TryGetComponent<ResourceFish>(out var _)
			? PromptObject.FishingIcon 
			: PromptObject.EKey);

		// display prompt
		_promptObject.SetEnabled(true);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		_promptObject.SetEnabled(false);
		_interactTarget = null;
	}

	public void Interact()
	{
		if (_interactTarget == null) return;
		_interactTarget.Interact();
	}
}

public class PromptObject
{
	public static Sprite EKey;
	public static Sprite FishingIcon;

	private GameObject _object;
	private SpriteRenderer _sr;

	public PromptObject(Transform parent)
	{
		_initSprites();

		// create object and set parent & position
		_object = new GameObject();
		_object.transform.parent = parent;
		_object.transform.position = parent.position;
		_object.transform.localPosition += Vector3.up * 0.75f + Vector3.right;

		// set up spriterenderer on prompt object
		_sr = _object.AddComponent<SpriteRenderer>();
		_sr.sprite = EKey;
		_sr.sortingOrder = 10;
		_sr.sortingLayerName = "Midground";

		SetEnabled(false);
	}

	public void SetEnabled(bool enabled) => _sr.color = new Color(1f, 1f, 1f, enabled ? 1f : 0f);
	public void SetSprite(Sprite sprite)
	{
		Debug.Log("changing sprite");
		_sr.sprite = sprite;
	}

	private void _initSprites()
	{
		EKey = Resources.Load<Sprite>("Sprites/Keys/E_Key_Light");
		FishingIcon = Resources.Load<Sprite>("Sprites/Keys/Fish_Icon");
	}
}