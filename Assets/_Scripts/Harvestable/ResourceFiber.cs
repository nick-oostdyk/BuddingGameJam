using UnityEngine;

public class ResourceFiber : Resource
{
	public override ItemType ItemDrop => ItemType.FIBER;

	public override void Interact()
	{
		base.Interact();

		Player.InventoryWrapper.ModifyQuantity(ItemDrop, Random.Range(4, 8)); // 4-7
	}

}