using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIInventoryItemBehaviour : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _itemName;
	[SerializeField] private TextMeshProUGUI _itemAmount;

	public void Init(string name, string amount)
	{
		_itemName.text = name;
		_itemAmount.text = amount;
	}
}
