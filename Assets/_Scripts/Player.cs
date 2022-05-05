using System.Collections;
using UnityEngine;

public class Player : Entity
{
	private GameObject _promptObject;
	private PlayerInventory _inventory;
	private IInteractable _interactTarget;

	private void Start()
	{
		_inventory = new PlayerInventory();
		_generatePromptObject();

		_promptObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		var otherInteractable = other.GetComponent<IInteractable>();

		if (otherInteractable == null) return;

		_interactTarget = otherInteractable;

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
		_promptObject = new GameObject();
		_promptObject.transform.parent = transform;
		_promptObject.transform.position = transform.position;
		_promptObject.transform.localPosition += Vector3.up * 0.75f + Vector3.right;

		var sr = _promptObject.AddComponent<SpriteRenderer>();
		sr.sprite = Resources.Load<Sprite>("Sprites/Keys/E_Key_Light");
		sr.sortingOrder = 10;
		sr.sortingLayerName = "Midground";
	}
}