using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICraftingBehaviour : MonoBehaviour
{
	[SerializeField] private GameObject _UICraftingItemPrefab;

	void Start()
	{
		PlayerInventory.OnPlayerInventoryChange += _updateCraftingMenu;
		_populateCraftingMenu();
	}

	private void _updateCraftingMenu(Dictionary<ItemType, int> _inventory)
	{
		// toggle if crafting menu item is interactable
		var recipes = CraftingManager.Recipes;

		for (int i = 0; i < recipes.Count; ++i)
			transform.GetChild(i).GetComponent<Button>().interactable = recipes[i].CheckIfCanCraft();
	}

	private void _clearCraftingMenu()
	{
		// remove everything from the menu
		foreach (Transform child in transform)
			Destroy(child.gameObject);
	}

	private void _populateCraftingMenu()
	{
		// add all of the recipes in the game to the menu
		foreach (var recipe in CraftingManager.Recipes)
		{
			var item = Instantiate(_UICraftingItemPrefab, transform);
			item.GetComponent<UICraftingItemBehaviour>().Init(recipe);

			if (!recipe.CheckIfCanCraft())
				item.GetComponent<Button>().interactable = false;
		}
	}

	private void OnDestroy() => PlayerInventory.OnPlayerInventoryChange -= _updateCraftingMenu;
}
