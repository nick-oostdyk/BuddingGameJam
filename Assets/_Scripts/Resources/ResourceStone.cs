using UnityEngine;

public class ResourceStone : Resource
{
	public override ResourceType Type => ResourceType.STONE;

	public override void Interact(Player p)
	{
		p.Inventory.ModifyQuantity(Type, Random.Range(3, 7)); // 3-6
	}
}
