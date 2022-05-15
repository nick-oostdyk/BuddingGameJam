using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryBehaviour : MonoBehaviour
{
	[SerializeField] private GameObject _inventoryItemPrefab;

	private void Start()
	{
		PlayerInventory.OnPlayerInventoryChange += _updateInventory;
	}

	private void OnEnable()
	{
		_updateInventory(Player.InventoryWrapper.Inventory);
	}

	private void _updateInventory(Dictionary<ItemType, int> _inventory)
	{
		_clearInventory();
		_setInventory(_inventory);
	}

	private void _clearInventory()
	{
		foreach (Transform child in transform)
			Destroy(child.gameObject);
	}

	private void _setInventory(Dictionary<ItemType, int> _inventory)
	{
		// instantiates a ui prefab that holds item info
		foreach (var (key, value) in _inventory)
			Instantiate(_inventoryItemPrefab, transform)
				.GetComponent<UIInventoryItemBehaviour>().Init($"{key.ToString().ToLower()}", $"x{value}");
	}

	private void OnDestroy() => PlayerInventory.OnPlayerInventoryChange -= _updateInventory;
}
