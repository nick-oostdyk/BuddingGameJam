using UnityEngine;
using System.Collections.Generic;

[System.Serializable, CreateAssetMenu(fileName = "New ItemObject", menuName = "ScriptableObjects/ItemObject")]
public class ItemObject : ScriptableObject
{
	[SerializeField] public Sprite Sprite;
	[SerializeField] public ItemType Type;
}

[System.Serializable]
public class ItemStack
{
	[SerializeField] public ItemType Item;
	[SerializeField] public int Amount;

	public ItemStack()
	{
		Item = ItemType.NUM_ITEMS;
		Amount = 0;
	}

	public ItemStack(ItemType type, int amount = 1)
	{
		Item = type;
		Amount = amount;
	}
}

[System.Serializable]
public class DropTablePacket
{
	[SerializeField] public List<ItemStack> Packet;
	[SerializeField] public int Rolls;

	public DropTablePacket()
	{
		Packet = new List<ItemStack>();
		Rolls = 0;
	}

	public DropTablePacket(DropTablePacket packet)
	{
		Packet = new List<ItemStack>(packet.Packet);
		Rolls = packet.Rolls;
	}

	public DropTablePacket(List<ItemStack> packet, int rolls)
	{
		Packet = new List<ItemStack>(packet);
		Rolls = rolls;
	}
}

[System.Serializable]
public class DropTable
{
	[SerializeField] public List<DropTablePacket> DropPackets;

	public DropTable() => DropPackets = new List<DropTablePacket>();
	public DropTable(List<DropTablePacket> list) => DropPackets = new List<DropTablePacket>(list);

	public void RollAndSet() => Player.InventoryWrapper.ModifyQuantity(Roll());
	public List<ItemStack> Roll()
	{
		var tmpPackets = new List<DropTablePacket>();
		var sum = 0;
		foreach (var packet in DropPackets)
		{
			sum += packet.Rolls;
			tmpPackets.Add(new DropTablePacket(packet));
			tmpPackets[tmpPackets.Count - 1].Rolls = sum;
		}

		var roll = Random.Range(0, sum);
		foreach (var packet in tmpPackets)
			if (roll < packet.Rolls) return packet.Packet;

		return null;
	}
}