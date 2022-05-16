using UnityEngine;

public class ResourceStone : Resource
{
	public override ResourceType Type => ResourceType.STONE;

	private void Start()
	{
		Sprite[] sprites = {
			Resources.Load<Sprite>("Sprites/Island/Harvestables/cassStone1"),
			Resources.Load<Sprite>("Sprites/Island/Harvestables/cassStone2")
		};

		_sr.sprite = sprites[Random.Range(0, 2)];
	}
}
