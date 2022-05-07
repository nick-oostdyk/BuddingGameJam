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

	public DropTable(Dictionary<ResourceType, List<(List<(ItemType, int)>, int)>> dictionary) => _dropTable = dictionary;

	public List<(ItemType, int)> RollResource(ResourceType resourceType)
	{
		// create list to store all drop lists for resource, and index
		var indexDropListTable = new SortedDictionary<int, List<(ItemType, int)>>();
		var sum = 0;

		// cache lists & store index's
		foreach (var (dropList, dropRolls) in _dropTable[resourceType])
		{
			sum += dropRolls;
			indexDropListTable.Add(sum, dropList);
		}

		var roll = Random.Range(0, sum);

		// pick winning resource list
		foreach (var (index, list) in indexDropListTable)
			if (roll < index) return list;

		return null;
	}
}