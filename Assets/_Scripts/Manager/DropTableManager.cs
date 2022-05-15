using System.Collections.Generic;
using UnityEngine;

public class DropTableManager
{
	[SerializeField ]private Dictionary<ResourceType, DropTable> DropTables;
	public DropTableManager() => DropTables = new Dictionary<ResourceType, DropTable>();


	public List<ItemStack> RollResource(ResourceType resourceType)
	{
		// cache lists & store indexes
		var tmpDropTable = new DropTable();
		var sum = 0;

		foreach (var packet in DropTables[resourceType].DropPackets)
		{
			sum += packet.Rolls;
			tmpDropTable.DropPackets.Add(packet);
		}

		// pick winning resource list
		var roll = Random.Range(0, sum);

		foreach (var packet in tmpDropTable.DropPackets)
			if (roll < packet.Rolls) return packet.Packet;

		Debug.Log($"No drop table found for {resourceType}!");
		return null;
	}

	public void RollAndSet(ResourceType type)
	{
		//var droptable = RollResource(type);
		//if (droptable is not null) 
		//	Player.InventoryWrapper.ModifyQuantity(droptable);
	}
}
