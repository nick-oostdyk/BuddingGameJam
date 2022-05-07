using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType : short
{
	STONE,
	BOULDER,

	DRIFTWOOD,
	TREE,

	FIBER,
	FISH,
}

public abstract class Resource : MonoBehaviour, IInteractable
{
	public System.Action<Resource> onResourceHarvest;
	public virtual void Interact() => onResourceHarvest?.Invoke(this);

	public abstract ResourceType Type { get; }
	protected PlayerInventory _playerInv => Player.InventoryWrapper;
}