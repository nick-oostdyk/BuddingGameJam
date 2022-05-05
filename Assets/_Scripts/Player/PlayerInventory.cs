using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory
{
	public Dictionary<ResourceType, int> Inventory { get; private set; }
	public PlayerInventory() => Inventory = new Dictionary<ResourceType, int>();

	// adds item to inventory if it does not exist
	// mutates quantity if item exists
	// removes item if quantity reaches or drops below 0
	public void ModifyQuantity(ResourceType resource, int value)
	{
		if (Inventory.ContainsKey(resource))
			Inventory[resource] += value;

		else 
			Inventory[resource] = value;

		if (Inventory[resource] <= 0)
			Inventory.Remove(resource);
	}

	public void Print()
	{
		Debug.Log("Inventory:");

		foreach (var (key, value) in Inventory)
			Debug.Log($"{key} : x{value}");
	}
}