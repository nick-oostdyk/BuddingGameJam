using UnityEngine;

public class ResourceBoulder : Resource
{
	public override ResourceType Type => ResourceType.BOULDER;

	public override void Interact(Player p)
	{
		p.Inventory.ModifyQuantity(Type, Random.Range(1, 3));
	}
}
