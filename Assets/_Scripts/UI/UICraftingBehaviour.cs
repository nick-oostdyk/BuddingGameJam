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
		var recipes = CraftingManager.Recipes;

		for (int i = 0; i < recipes.Count; ++i)
			transform.GetChild(i).GetComponent<Button>().interactable = recipes[i].CheckIfCanCraft();
	}

	private void _clearCraftingMenu()
	{
		foreach (Transform child in transform)
			Destroy(child.gameObject);
	}

	private void _populateCraftingMenu()
	{
		foreach (var recipe in CraftingManager.Recipes)
		{
			var item = Instantiate(_UICraftingItemPrefab, transform);
			item.GetComponent<UICraftingItemBehaviour>().Init(recipe);

			if (!recipe.CheckIfCanCraft())
				item.GetComponent<Button>().interactable = false;
		}
	}

	private void OnDestroy()
	{
		PlayerInventory.OnPlayerInventoryChange -= _updateCraftingMenu;
	}
}
