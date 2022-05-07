using System.Collections.Generic;
using UnityEngine;

public class DropTable
{
	//	ResourceType			-> Resource that was harvested
	//	,
	//	(	
	//		List
	//			List
	//				(
	//				ItemType	-> item to drop
	//				,
	//				int			-> number of items to drop
	//				)
	//		,
	//		int					-> roll chances
	//	)
	private Dictionary<ResourceType, List<(List<(ItemType, int)>, int)>> _dropTable;

	private PlayerInventory _pInv => Player.InventoryWrapper;
	public DropTable(Dictionary<ResourceType, List<(List<(ItemType, int)>, int)>> dictionary) => _dropTable = dictionary;

	public void RollResource(ResourceType resourceType)
	{
		// cache lists & store indexes
		var indexDroplistTable = new List<(List<(ItemType, int)>, int)>();
		var sum = 0;

		foreach (var (droplist, dropRolls) in _dropTable[resourceType])
		{
			sum += dropRolls;
			indexDroplistTable.Add((droplist, sum));
		}

		// pick winning resource list
		var roll = Random.Range(0, sum);

		foreach (var (list, index) in indexDroplistTable)
			if (roll < index)
			{
				Player.InventoryWrapper.ModifyQuantity(list);
				break;
			}

	}

	private void _givePlayerDrops(List<(ItemType, int)> drops)
	{
		foreach (var (item, quantity) in drops)
			_pInv.ModifyQuantity(item, quantity);
	}
}