using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Timers;

public class ResourceManager : MonoBehaviour
{
	private int _minSpawnTimeMS = 2 * 1000;
	private int _maxSpawnTimeMS = 4 * 1000;

	public Dictionary<ResourceType, DropTable> DropTables;

	public void Start()
	{
		_generateDropTable();

		var resourceArr = GetComponentsInChildren<Resource>();

		// iterate over all the children and subscribe to harvest event
		foreach (var resource in resourceArr)
			resource.OnInteract += (s, e) => _resourceInteractHandler(s as Resource);
	}

	private async void _resourceInteractHandler(Resource resource)
	{
		if (resource == null) return;

		// give player resource drops
		DropTables[resource.Type].RollAndSet();
		resource.gameObject.SetActive(false);

		// respawn the resource after a random time
		Util.DelayRunAction(
			Random.Range(_minSpawnTimeMS, _maxSpawnTimeMS), 
			() => resource.gameObject.SetActive(true));
	}

	private void _generateDropTable()
	{
		DropTables = new Dictionary<ResourceType, DropTable>();

		DropTables[ResourceType.STONE] = _generateStoneDropTable();
		DropTables[ResourceType.BOULDER] = _generateBoulderDropTable();
		DropTables[ResourceType.DRIFTWOOD] = _generateDriftwoodDropTable();
		DropTables[ResourceType.TREE] = _generateTreeDropTable();
		DropTables[ResourceType.FIBER] = _generateFiberDropTable();
	}

	private DropTable _generateStoneDropTable()
	{
		var packetList = new List<DropTablePacket>();
		
		packetList.Add(new DropTablePacket(new List<ItemStack>()
		{
			new ItemStack(ItemType.STONE, _roll(1, 3)),
		}, 4));

		packetList.Add(new DropTablePacket(new List<ItemStack>()
		{
			new ItemStack(ItemType.STONE, _roll(3, 7)),
			new ItemStack(ItemType.BOULDER, _roll(0, 1)),
		}, 4));
		
		packetList.Add(new DropTablePacket(new List<ItemStack>()
		{
			new ItemStack(ItemType.STONE, _roll(3, 7)),
			new ItemStack(ItemType.BOULDER, _roll(0, 1)),
			new ItemStack(ItemType.METAL_SCRAP, _roll(0, 1)),
		}, 2));

		return new DropTable(packetList);
	}

	private DropTable _generateBoulderDropTable()
	{
		var packetList = new List<DropTablePacket>();

		packetList.Add(new DropTablePacket(new List<ItemStack>()
		{
			new ItemStack(ItemType.STONE, _roll(0, 2)),
			new ItemStack(ItemType.BOULDER, _roll(1, 2)),
		}, 7));

		packetList.Add(new DropTablePacket(new List<ItemStack>()
		{
			new ItemStack(ItemType.STONE, _roll(1, 3)),
			new ItemStack(ItemType.BOULDER, _roll(2, 3)),
			new ItemStack(ItemType.METAL_SCRAP, _roll(0, 1)),
		}, 3));

		packetList.Add(new DropTablePacket(new List<ItemStack>()
		{
			new ItemStack(ItemType.STONE, _roll(3, 5)),
			new ItemStack(ItemType.BOULDER, _roll(2, 5)),
			new ItemStack(ItemType.METAL_SCRAP, _roll(1, 2)),
		}, 1));

		return new DropTable(packetList);
	}

	private DropTable _generateDriftwoodDropTable()
	{
		var packetList = new List<DropTablePacket>();

		packetList.Add(new DropTablePacket(new List<ItemStack>()
		{
			new ItemStack(ItemType.STICK, _roll(1, 3)),
		}, 7));

		packetList.Add(new DropTablePacket(new List<ItemStack>()
		{
			new ItemStack(ItemType.STICK, _roll(2, 4)),
			new ItemStack(ItemType.WOOD, _roll(0, 1)),
		}, 2));

		packetList.Add(new DropTablePacket(new List<ItemStack>()
		{
			new ItemStack(ItemType.STICK, _roll(3, 5)),
			new ItemStack(ItemType.FIBER, _roll(0, 1)),
			new ItemStack(ItemType.WOOD, 1),
		}, 1));

		return new DropTable(packetList);
	}

	private DropTable _generateTreeDropTable()
	{
		var packetList = new List<DropTablePacket>();

		packetList.Add(new DropTablePacket(new List<ItemStack>()
		{
			new ItemStack(ItemType.STICK, _roll(0, 2)),
			new ItemStack(ItemType.WOOD, _roll(1, 3)),
		}, 6));

		packetList.Add(new DropTablePacket(new List<ItemStack>()
		{
			new ItemStack(ItemType.STICK, _roll(1, 3)),
			new ItemStack(ItemType.WOOD, _roll(2, 3)),
			new ItemStack(ItemType.FIBER, _roll(0, 1)),
		}, 3));

		packetList.Add(new DropTablePacket(new List<ItemStack>()
		{
			new ItemStack(ItemType.STICK, _roll(2, 5)),
			new ItemStack(ItemType.WOOD, _roll(3, 5)),
			new ItemStack(ItemType.FIBER, _roll(0, 2)),
		}, 1));

		return new DropTable(packetList);
	}

	private DropTable _generateFiberDropTable()
	{
		var packetList = new List<DropTablePacket>();

		packetList.Add(new DropTablePacket(new List<ItemStack>()
		{
			new ItemStack(ItemType.FIBER, _roll(1, 2)),
		}, 4));

		packetList.Add(new DropTablePacket(new List<ItemStack>()
		{
			new ItemStack(ItemType.FIBER, _roll(1, 3)),
			new ItemStack(_roll(0, 1) == 0 ? ItemType.WOOD : ItemType.STONE, _roll(0, 1)),
		}, 3));

		packetList.Add(new DropTablePacket(new List<ItemStack>()
		{
			new ItemStack(ItemType.FIBER, _roll(2, 4)),
			new ItemStack(_roll(0, 1) == 0 ? ItemType.WOOD : ItemType.STONE, _roll(1, 2)),
		}, 1));

		return new DropTable(packetList);
	}

	private int _roll(int min, int max) => Random.Range(min, ++max);
}