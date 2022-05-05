using UnityEngine;

public class ResourceTree : Resource
{
	public override ResourceType Type => ResourceType.WOOD;

	public override void Interact(Player p)
	{
		p.Inventory.ModifyQuantity(Type, Random.Range(1, 5));
	}
}