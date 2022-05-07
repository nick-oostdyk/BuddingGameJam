using System.Collections.Generic;
using UnityEngine;
public class Table
{
	public Dictionary<ItemType, int> Item { get; private set; }
	public Table(Dictionary<ItemType, int> item) => Item = item;
}

public class DropTable
{
	public List<(Table, int)> TableIntPairs;
	public DropTable() => TableIntPairs = new List<(Table, int)>();
	public DropTable(List<(Table, int)> pairs) => TableIntPairs = new List<(Table, int)>(pairs);
	public DropTable(Table table, int priority) => TableIntPairs = new List<(Table, int)>() { (table, priority) };
	public void AddTableIntPair(Table table, int priority) => TableIntPairs.Add((table, priority));
	public void AddTableIntPair((Table, int) pair) => AddTableIntPair(pair.Item1, pair.Item2);
}

public class DropTableManager
{
	private Dictionary<ResourceType, DropTable> _dropTables;
	public DropTableManager() => _dropTables = new Dictionary<ResourceType, DropTable>();

	public void AddTable(ResourceType type, DropTable table) => _dropTables.Add(type, table);
	public void AddTable((ResourceType, DropTable) item) => _dropTables.Add(item.Item1, item.Item2);

	public Dictionary<ItemType, int> RollResource(ResourceType resourceType)
	{
		// cache lists & store indexes
		var indexDroplistTable = new DropTable();
		var sum = 0;

		foreach (var (droplist, dropRolls) in _dropTables[resourceType].TableIntPairs)
		{
			sum += dropRolls;
			indexDroplistTable.AddTableIntPair((droplist, sum));
		}

		// pick winning resource list
		var roll = Random.Range(0, sum);

		foreach (var (list, index) in indexDroplistTable.TableIntPairs)
			if (roll < index) return list.Item;

		Debug.Log($"No drop table found for {resourceType}!");
		return null;
	}

	public void RollAndSet(ResourceType type)
	{
		var droptable = RollResource(type);
		if (droptable != null) Player.InventoryWrapper.ModifyQuantity(droptable);
	}
}