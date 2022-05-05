using System.Collections;
using UnityEngine;

public class Player : Entity
{
	public static PlayerInventory InventoryWrapper;

	private GameObject _promptObject;
	private IInteractable _interactTarget;

	private void Start()
	{
		InventoryWrapper = new PlayerInventory();
		_generatePromptObject();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		// typecheck other as interactable
		var otherInteractable = other.GetComponent<IInteractable>();

		if (otherInteractable == null) return;

		// cache target if interactable
		_interactTarget = otherInteractable;

		// display prompt
		_promptObject.SetActive(true);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		_promptObject.SetActive(false);
		_interactTarget = null;
	}

	public void Interact()
	{
		if (_interactTarget == null) return;
		_interactTarget.Interact();
	}

	private void _generatePromptObject()
	{
		// create object and set parent & position
		_promptObject = new GameObject();
		_promptObject.transform.parent = transform;
		_promptObject.transform.position = transform.position;
		_promptObject.transform.localPosition += Vector3.up * 0.75f + Vector3.right;

		// set up spriterenderer on prompt object
		var sr = _promptObject.AddComponent<SpriteRenderer>();
		sr.sprite = Resources.Load<Sprite>("Sprites/Keys/E_Key_Light");
		sr.sortingOrder = 10;
		sr.sortingLayerName = "Midground";

		// hide object
		_promptObject.SetActive(false);
	}
}