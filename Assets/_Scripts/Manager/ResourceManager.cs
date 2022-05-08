using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ResourceManager : MonoBehaviour
{
	private int _minSpawnTimeMS = 1 * 1000;
	private int _maxSpawnTimeMS = 3 * 1000;

	private DropTableManager _resourceDropTable;

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

		_resourceDropTable.RollAndSet(resource.Type);
		resource.gameObject.SetActive(false);

		await Task.Delay(Random.Range(_minSpawnTimeMS, _maxSpawnTimeMS));

		if (resource == null) return;
		resource.gameObject.SetActive(true);
	}

	private void _generateDropTable()
	{
		_resourceDropTable = new DropTableManager();
		_resourceDropTable.AddTable(ResourceType.STONE, _generateStoneDropTable());
		_resourceDropTable.AddTable(ResourceType.BOULDER, _generateBoulderDropTable());
		_resourceDropTable.AddTable(ResourceType.DRIFTWOOD, _generateDriftwoodDropTable());
		_resourceDropTable.AddTable(ResourceType.TREE, _generateTreeDropTable());
		_resourceDropTable.AddTable(ResourceType.FIBER, _generateFiberDropTable());
	}

	private DropTable _generateStoneDropTable()
	{
		var stoneTable = new DropTable();

		stoneTable.AddTableIntPair(
		new Table(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.STONE, _roll(1, 3) },
			}), 5
		);

		stoneTable.AddTableIntPair(
		new Table(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.STONE, _roll(3, 7) },
				{ ItemType.BOULDER, _roll(0, 1)},
			}), 5
		);

		stoneTable.AddTableIntPair(
		new Table(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.STONE, _roll(3, 7) },
				{ ItemType.BOULDER, _roll(0, 1) },
				{ ItemType.METAL_SCRAP, _roll(0, 1) },
			}), 5
		);

		return stoneTable;
	}

	private DropTable _generateBoulderDropTable()
	{
		var boulderTable = new DropTable();

		boulderTable.AddTableIntPair(
		new Table(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.STONE, _roll(0, 2) },
				{ ItemType.BOULDER, _roll(1, 2) },
			}), 7
		);

		boulderTable.AddTableIntPair(
		new Table(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.STONE, _roll(1, 3) },
				{ ItemType.BOULDER, _roll(2, 3) },
				{ ItemType.METAL_SCRAP, _roll(0, 1) },
			}), 3
		);

		boulderTable.AddTableIntPair(
		new Table(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.STONE, _roll(3, 5) },
				{ ItemType.BOULDER, _roll(2, 5) },
				{ ItemType.METAL_SCRAP, _roll(1, 2) },
			}), 1
		);

		return boulderTable;
	}
	private DropTable _generateDriftwoodDropTable()
	{
		var driftwoodTable = new DropTable();

		driftwoodTable.AddTableIntPair(
		new Table(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.STICK, _roll(1, 3) },
			}), 7
		);

		driftwoodTable.AddTableIntPair(
		new Table(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.STICK, _roll(2, 4) },
				{ ItemType.WOOD, _roll(0, 1) },
			}), 2
		);

		driftwoodTable.AddTableIntPair(
		new Table(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.STICK, _roll(3, 5) },
				{ ItemType.FIBER, _roll(0, 1) },
				{ ItemType.WOOD, 1 },
			}), 1
		);


		return driftwoodTable;
	}

	private DropTable _generateTreeDropTable()
	{
		var treeTable = new DropTable();

		treeTable.AddTableIntPair(
		new Table(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.STICK, _roll(0, 2) },
				{ ItemType.WOOD, _roll(1, 3) },
			}), 6
		);

		treeTable.AddTableIntPair(
		new Table(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.STICK, _roll(1, 3) },
				{ ItemType.WOOD, _roll(2, 3) },
				{ ItemType.FIBER, _roll(0, 1) },
			}), 3
		);

		treeTable.AddTableIntPair(
		new Table(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.STICK, _roll(2, 5) },
				{ ItemType.WOOD, _roll(3, 5) },
				{ ItemType.FIBER, _roll(0, 2) },
			}), 1
		);

		return treeTable;
	}

	private DropTable _generateFiberDropTable()
	{
		var fiberTable = new DropTable();

		fiberTable.AddTableIntPair(
		new Table(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.FIBER, _roll(1, 2) },
			}), 4
		);

		fiberTable.AddTableIntPair(
		new Table(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.FIBER, _roll(1, 3) },
				{ _roll(0, 1) == 0 ? ItemType.WOOD : ItemType.STONE, _roll(0, 1) },
			}), 3
		);

		fiberTable.AddTableIntPair(
		new Table(
			new Dictionary<ItemType, int>()
			{
				{ ItemType.FIBER, _roll(2, 4) },
				{ _roll(0, 1) == 0 ? ItemType.WOOD : ItemType.STONE, _roll(1, 2) },
			}), 1
		);

		return fiberTable;
	}

	private int _roll(int min, int max) => Random.Range(min, ++max);
}