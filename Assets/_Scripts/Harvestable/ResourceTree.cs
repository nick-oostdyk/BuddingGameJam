using UnityEngine;

public class ResourceTree : Resource
{
	public override ItemType ItemDrop => ItemType.WOOD;

	public override void Interact()
	{
		base.Interact();

		Player.InventoryWrapper.ModifyQuantity(ItemDrop, Random.Range(1, 5)); // 1-4
	}

}