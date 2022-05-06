using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resource : MonoBehaviour, IInteractable
{
	public System.Action<Resource> onResourceHarvest;
	public virtual void Interact() => onResourceHarvest?.Invoke(this);

	public abstract ItemType ItemDrop { get; }
	protected PlayerInventory _playerInv => Player.InventoryWrapper;
}