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
		new CraftingRecipe(
			// basic multitool
			new Dictionary<ItemType, int>()
			{
				{ ItemType.STONE, 4 },
				{ ItemType.STICK, 2 },
				{ ItemType.FIBER, 2 },
			}, (ItemType.BASIC_MULTI_TOOL, 1));
	}
}

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
}