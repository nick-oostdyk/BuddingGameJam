using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInventory
{
	public Dictionary<ItemType, int> Inventory { get; private set; }
	public PlayerInventory() => Inventory = new Dictionary<ItemType, int>();

	// adds item to inventory if it does not exist
	// mutates quantity if item exists
	// removes item if quantity reaches or drops below 0
	public void ModifyQuantity(ItemType resource, int value)
	{
		if (Inventory.ContainsKey(resource))
			Inventory[resource] += value;

		else
			Inventory[resource] = value;

		if (Inventory[resource] <= 0)
			Inventory.Remove(resource);

		Sort();
	}

	public void Sort()
	{
		Inventory = Inventory
			.OrderBy(item => (int)item.Key)
			.ToDictionary(item => item.Key, item => item.Value);
	}

	public void Print()
	{
		Debug.Log("Inventory:");

		foreach (var (key, value) in Inventory)
			Debug.Log($"{key} : x{value}");
	}
}