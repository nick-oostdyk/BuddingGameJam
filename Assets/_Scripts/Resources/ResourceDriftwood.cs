using UnityEngine;

public class ResourceDriftwood : Resource
{
	public override ResourceType Type => ResourceType.STICK;

	public override void Interact(Player p)
	{
		p.Inventory.ModifyQuantity(Type, Random.Range(2, 6)); // 2-5
	}
}
