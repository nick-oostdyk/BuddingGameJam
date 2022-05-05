using UnityEngine;

public class ResourceTree : Resource
{
	public override ItemType ItemDrop => ItemType.WOOD;

	public override void Harvest(Player p)
	{
		p.InventoryWrapper.ModifyQuantity(ItemDrop, Random.Range(1, 5)); // 1-4
	}
}