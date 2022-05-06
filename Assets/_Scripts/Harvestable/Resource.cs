using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resource : MonoBehaviour, IInteractable
{
	public System.Action<Resource> onResourceHarvest;
	public abstract ItemType ItemDrop { get; }
	public virtual void Interact() => onResourceHarvest?.Invoke(this);
	protected PlayerInventory _playerInv => Player.InventoryWrapper;
}