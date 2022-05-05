using UnityEngine;

public class ResourceBoulder : Resource
{
	public override ItemType ItemDrop => ItemType.BOULDER;

	public override void Harvest(Player p)
	{
		int numResourcesDropped = Random.Range(0, 10);

		switch (numResourcesDropped)
		{
			case int n when n < 7:
				break;
			default:
				break;
		}

		p.InventoryWrapper.ModifyQuantity(ItemDrop, Random.Range(1, 3)); // 1-2
	}
}
