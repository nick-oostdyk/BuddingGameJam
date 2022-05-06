using UnityEngine;

public class ResourceDriftwood : Resource
{
	public override ItemType ItemDrop => ItemType.STICK;

	public override void Interact()
	{
		Player.InventoryWrapper.ModifyQuantity(ItemDrop, Random.Range(2, 6)); // 2-5
	}
}
