using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICraftingItemBehaviour : MonoBehaviour
{
	[SerializeField] private GameObject _itemStackPrefab;
	[SerializeField] private RectTransform _componentParent;
	[SerializeField] private UIItemStack _resultStack;
	private Button _button;

	public CraftingRecipe Recipe;

	public void Init(CraftingRecipe recipe)
	{
		Recipe = recipe;

		foreach (var item in Recipe.Recipe)
		{
			Instantiate(_itemStackPrefab, _componentParent)
				.GetComponent<UIItemStack>().Init(ItemPool.ItemDict[item.Key].Sprite, $"x{item.Value}");
		}

		_resultStack.Init(ItemPool.ItemDict[Recipe.Result.Item1].Sprite, $"x{Recipe.Result.Item2}");

		_button = GetComponent<Button>();
		_button.onClick.AddListener(() => Recipe.TryCraft());
	}
}
