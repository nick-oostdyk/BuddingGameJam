using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

public class ResourceManager : MonoBehaviour
{
	private int _minSpawnTimeMS = 1 * 1000;
	private int _maxSpawnTimeMS = 3 * 1000;

	private DropTable _resourceDropTable;

	public void Start()
	{
		_generateDropTable();

		var resourceArr = GetComponentsInChildren<Resource>();

		// iterate over all the children and subscribe to harvest event
		foreach (var resource in resourceArr)
			resource.onResourceHarvest += _resourceInteractHandler;
	}

	private async void _resourceInteractHandler(Resource resource)
	{
		_resourceDropTable.RollResource(resource.Type);
		resource.gameObject.SetActive(false);

		await Task.Delay(Random.Range(_minSpawnTimeMS, _maxSpawnTimeMS));

		if (resource == null) return;
		resource.gameObject.SetActive(true);
	}

	private void _generateDropTable()
	{
		int roll(int min, int max) => Random.Range(min, ++max);

		var dropTable = new DropTable(
			new Dictionary<ResourceType, List<(List<(ItemType, int)>, int)>>() {

			// STONE
			{
				ResourceType.STONE, new List<(List<(ItemType, int)>, int)>()
				{
					(
						new List<(ItemType, int)>()
						{
							(ItemType.STONE, roll(1, 3)),
						}, 5
					),
					(
						new List<(ItemType, int)>()
						{
							(ItemType.STONE, roll(2, 5)),
						}, 3
					),
					(
						new List<(ItemType, int)>()
						{
							(ItemType.STONE, roll(3, 7)),
							(ItemType.BOULDER, roll(0, 1)),
						}, 1
					),
					(
						new List<(ItemType, int)>()
						{
							(ItemType.STONE, roll(3, 7)),
							(ItemType.METAL_SCRAP, roll(0, 1)),
						}, 1
					),
				}
			},

			// BOULDER
			{
				ResourceType.BOULDER, new List<(List<(ItemType, int)>, int)>()
				{
					(
						new List<(ItemType, int)>()
						{
							(ItemType.STONE, roll(0, 2)),
							(ItemType.BOULDER, roll(1, 2)),
						}, 7
					),
					(
						new List<(ItemType, int)>()
						{
							(ItemType.STONE, roll(1, 3)),
							(ItemType.BOULDER, roll(2, 3)),
							(ItemType.METAL_SCRAP, roll(0, 1)),
						}, 3
					),
					(
						new List<(ItemType, int)>()
						{
							(ItemType.STONE, roll(3, 5)),
							(ItemType.BOULDER, roll(2, 5)),
							(ItemType.METAL_SCRAP, roll(1, 2)),
						}, 1
					),
				}
			},

			// DRIFTWOOD
			{
				ResourceType.DRIFTWOOD, new List<(List<(ItemType, int)>, int)>()
				{
					(
						new List<(ItemType, int)>()
						{
							(ItemType.STICK, roll(1, 3)),
						}, 7
					),
					(
						new List<(ItemType, int)>()
						{
							(ItemType.STICK, roll(2, 4)),
							(ItemType.WOOD, roll(0, 1)),
						}, 2
					),
					(
						new List<(ItemType, int)>()
						{
							(ItemType.STICK, roll(3, 5)),
							(ItemType.WOOD, 1),
							(ItemType.FIBER, roll(0, 1)),
						}, 1
					),
				}
			},

			// TREE
			{
				ResourceType.TREE, new List<(List<(ItemType, int)>, int)>()
				{
					(
						new List<(ItemType, int)>()
						{
							(ItemType.STICK, roll(0, 2)),
							(ItemType.WOOD, roll(1, 3)),
						}, 6
					),
					(
						new List<(ItemType, int)>()
						{
							(ItemType.STICK, roll(1, 3)),
							(ItemType.WOOD, roll(2, 3)),
							(ItemType.FIBER, roll(0, 1)),
						}, 3
					),
					(
						new List<(ItemType, int)>()
						{
							(ItemType.STICK, roll(2, 5)),
							(ItemType.WOOD, roll(3, 5)),
							(ItemType.FIBER, roll(0, 2)),
						}, 1
					),
				}
			},

			// FIBER
			{
				ResourceType.FIBER, new List<(List<(ItemType, int)>, int)>()
				{
					(
						new List<(ItemType, int)>()
						{
							(ItemType.FIBER, roll(1, 2)),
						}, 4
					),
					(
						new List<(ItemType, int)>()
						{
							(ItemType.FIBER, roll(1, 3)),
							(roll(0, 1) == 0 ? ItemType.WOOD : ItemType.STONE, roll(0, 1)),
						}, 3
					),
					(
						new List<(ItemType, int)>()
						{
							(ItemType.FIBER, roll(2, 4)),
							(roll(0, 1) == 0 ? ItemType.WOOD : ItemType.STONE, roll(1, 2)),
						}, 1
					),
				}
			},
		});

		_resourceDropTable = dropTable;
	}
}