using UnityEngine;

public class ResourceTree : Resource
{
	public override ResourceType Type => ResourceType.TREE;

	private void Start()
	{
		var sr = GetComponent<SpriteRenderer>();
		sr.flipX = Random.Range(0, 2) == 1;
	}
}