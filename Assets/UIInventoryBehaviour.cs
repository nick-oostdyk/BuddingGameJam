using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryBehaviour : MonoBehaviour
{
	private Player _player;
	private Dictionary<ItemType, int> _lastInv; // inventory cache

	[SerializeField] private GameObject _inventoryItemPrefab;

	private void FixedUpdate()
	{
		if (_player == null)
		{
			_player = FindObjectOfType<Player>();
			_lastInv = new Dictionary<ItemType, int>(_player.InventoryWrapper.Inventory);

			return;
		}

		var playerInv = _player.InventoryWrapper.Inventory;
		if (playerInv != _lastInv)
		{
			_lastInv = _lastInv = new Dictionary<ItemType, int>(playerInv);
			_updateInventory();
		}
	}

	private void _updateInventory()
	{
		_clearInventory();
		_setInventory();
	}

	private void _clearInventory()
	{
		foreach (Transform child in transform)
			Destroy(child.gameObject);
	}

	private void _setInventory()
	{
		foreach (var (key, value) in _player.InventoryWrapper.Inventory)
			Instantiate(_inventoryItemPrefab, transform)
				.GetComponent<UIInventoryItemBehaviour>().Init($"{key.ToString().ToLower()}", $"x{value}");
	}
}
