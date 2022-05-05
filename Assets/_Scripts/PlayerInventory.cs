using System.Collections.Generic;

public class PlayerInventory
{
	public Dictionary<ResourceType, int> Inventory { get; private set; }

	public PlayerInventory() => Inventory = new Dictionary<ResourceType, int>();

	public void ModifyQuantity(ResourceType resource, int value)
	{
		if (Inventory.ContainsKey(resource))
			Inventory[resource] += value;
		else if (value > 0)
			Inventory[resource] = value;
	}
}