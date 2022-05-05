using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryBehaviour : MonoBehaviour
{
	private Player _player;
	private Dictionary<ItemType, int> _lastInv; // inventory cache

	[SerializeField] private GameObject _inventoryItemPrefab;

	private Dictionary<ItemType, int> _playerInv => Player.InventoryWrapper.Inventory;

	private void FixedUpdate()
	{
		if (_player == null)
		{
			_player = FindObjectOfType<Player>();
			_lastInv = new Dictionary<ItemType, int>(_playerInv);

			return;
		}

		if (_playerInv != _lastInv)
		{
			_lastInv = _lastInv = new Dictionary<ItemType, int>(_playerInv);
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
		foreach (var (key, value) in _playerInv)
			Instantiate(_inventoryItemPrefab, transform)
				.GetComponent<UIInventoryItemBehaviour>().Init($"{key.ToString().ToLower()}", $"x{value}");
	}
}
