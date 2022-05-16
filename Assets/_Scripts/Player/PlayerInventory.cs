using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInventory
{
	public static event System.Action<Dictionary<ItemType, int>> OnPlayerInventoryChange;

	public Dictionary<ItemType, int> Inventory { get; private set; }
	public PlayerInventory()
	{
		Inventory = new Dictionary<ItemType, int>();
		OnPlayerInventoryChange?.Invoke(Inventory);
	}

	// adds item to inventory if it does not exist
	// mutates quantity if item exists
	// removes item if quantity reaches or drops below 0
	public void ModifyQuantity(ItemType item, int value)
	{
		if (value == 0) return;

		PopupHandler.Instance.PushPopup(ItemPool.ItemDict[item].Sprite,
			$"{(value < 0 ? "-" : "+")}{Mathf.Abs(value)}x");

		if (Inventory.ContainsKey(item))
			Inventory[item] += value;

		else
			Inventory[item] = value;

		if (Inventory[item] <= 0)
			Inventory.Remove(item);

		Sort();
		OnPlayerInventoryChange?.Invoke(Inventory);
	}

	public void ModifyQuantity(List<ItemStack> stackList)
	{
		foreach (var stack in stackList) ModifyQuantity(stack.Item, stack.Amount);
	}

	public void ModifyQuantity(ItemStack stack) => ModifyQuantity(stack.Item, stack.Amount);
	public void ModifyQuantity(ItemType item) => ModifyQuantity(item, 1);
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