using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICraftingItemBehaviour : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _itemName;
	[SerializeField] private TextMeshProUGUI _itemAmount;
	private Button _button;

	public CraftingRecipe Recipe;

	public void Init(CraftingRecipe recipe)
	{
		Recipe = recipe;

		_itemName.text = Recipe.Result.Item1.ToString();
		_itemAmount.text = Recipe.Result.Item2.ToString();

		_button = GetComponent<Button>();
		_button.onClick.AddListener(() => Recipe.TryCraft());
	}
}
