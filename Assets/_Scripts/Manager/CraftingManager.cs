using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
	public static List<CraftingRecipe> Recipes;

	public void Start()
	{
		_generateRecipes();
	}

	private void _generateRecipes()
	{
		Recipes = new List<CraftingRecipe>();

		// Basic Multitool
		new CraftingRecipe(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.STONE, 5 },
				{ ItemType.STICK, 2 },
				{ ItemType.FIBER, 2 },
			}, (ItemType.BASIC_MULTI_TOOL, 1));

		// Adv multitool
		new CraftingRecipe(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.BOULDER, 3 },
				{ ItemType.WOOD, 2 },
				{ ItemType.FIBER, 6 },
			}, (ItemType.ADVANCED_MULTI_TOOL, 1));

		// Fishing Rod
		new CraftingRecipe(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.WOOD, 2 },
				{ ItemType.ROPE, 2 },
			}, (ItemType.FISHING_ROD, 1));

		// Planks
		new CraftingRecipe(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.WOOD, 4 },
			}, (ItemType.PLANK, 2));

		// Rope
		new CraftingRecipe(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.FIBER, 8 },
			}, (ItemType.ROPE, 3));
	}
}

// holds a dictionary of items & values for crafting recipes
// and also contains functions to execute the craft
public class CraftingRecipe
{
	public Dictionary<ItemType, int> Recipe { get; private set; }
	public (ItemType, int) Result;

	public CraftingRecipe(Dictionary<ItemType, int> recipe, (ItemType, int) result)
	{
		Recipe = recipe;
		Result = result;
		CraftingManager.Recipes.Add(this);
	}

	// checks if the player has all of the requisite items in their inventory
	public bool CheckIfCanCraft()
	{
		var canCraft = true;
		var pInv = Player.InventoryWrapper.Inventory;

		foreach (var (key, value) in Recipe)
			if (!pInv.ContainsKey(key) || pInv[key] < value)
			{
				canCraft = false;
				break;
			}

		return canCraft;
	}

	// attempts to craft the item, and put it in the players inventory
	public void TryCraft()
	{
		if (!CheckIfCanCraft()) return;

		foreach (var (key, value) in Recipe)
			Player.InventoryWrapper.ModifyQuantity(key, -value);

		Player.InventoryWrapper.ModifyQuantity(Result.Item1, Result.Item2);

		switch (Result.Item1)
		{
			case ItemType.BASIC_MULTI_TOOL:
				GameManager.Instance.AddGameFlag(GameFlag.TOOL_ONE);
				break;
			case ItemType.ADVANCED_MULTI_TOOL:
				GameManager.Instance.AddGameFlag(GameFlag.TOOL_TWO);
				break;
			case ItemType.FISHING_ROD:
				GameManager.Instance.AddGameFlag(GameFlag.FISH_ROD);
				break;
			case ItemType.FIREPIT:
				break;
			case ItemType.WATER_PURIFIER:
				break;
			case ItemType.FURNACE:
				break;
			case ItemType.NUM_ITEMS:
				break;
			default:
				break;
		}

		Debug.Log($"Crafted {Result.Item1}");
	}
}