using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resource : MonoBehaviour, IInteractable
{
	public abstract ItemType ItemDrop { get; }
	public abstract void Interact();
	protected PlayerInventory _playerInv => Player.InventoryWrapper;
}