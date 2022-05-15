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
	[SerializeField] public ItemObject Item;
	[SerializeField] public int Amount;
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

	public DropTablePacket(DropTablePacket otherPacket)
	{
		Packet = new List<ItemStack>(otherPacket.Packet);
		Rolls = otherPacket.Rolls;
	}


}

[System.Serializable]
public class DropTable
{
	[SerializeField] public List<DropTablePacket> DropPackets;

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

	public void RollAndSet()
	{
		var player = GameObject.FindObjectOfType<Player>();
	}
}