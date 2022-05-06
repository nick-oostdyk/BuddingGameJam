using UnityEngine;

public class ResourceBoulder : Resource
{
	public override ItemType ItemDrop => ItemType.BOULDER;

	public override void Interact()
	{
		base.Interact();

		int dropRoll = Random.Range(0, 10);

		switch (dropRoll)
		{
			case int n when n < 5:
				_playerInv.ModifyQuantity(ItemDrop, Random.Range(1, 3));
				break;

			case int n when n < 8:
				_playerInv.ModifyQuantity(ItemDrop, Random.Range(2, 5));
				_playerInv.ModifyQuantity(ItemType.METAL_SCRAP, Random.Range(1, 3));
				break;

			default:
				_playerInv.ModifyQuantity(ItemDrop, Random.Range(3, 7));
				_playerInv.ModifyQuantity(ItemType.METAL_SCRAP, Random.Range(1, 4));
				break;
		}

	}
}
