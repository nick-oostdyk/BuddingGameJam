using UnityEngine;

public class ResourceBoulder : Resource
{
	public override ItemType ItemDrop => ItemType.BOULDER;

	public override void Interact()
	{
		int numResourcesDropped = Random.Range(0, 10);

		switch (numResourcesDropped)
		{
			case int n when n < 7:
				break;
			default:
				break;
		}

		Player.InventoryWrapper.ModifyQuantity(ItemDrop, Random.Range(1, 3)); // 1-2
	}
}
