using UnityEngine;

public class ResourceDriftwood : Resource
{
	public override ItemType ItemDrop => ItemType.STICK;

	public override void Harvest(Player p)
	{
		p.InventoryWrapper.ModifyQuantity(ItemDrop, Random.Range(2, 6)); // 2-5
	}
}
