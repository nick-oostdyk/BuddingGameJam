using UnityEngine;

public class ResourceTree : Resource
{
	public override ResourceType Type => ResourceType.TREE;

	private void Start()
	{
		// start at random point in animations length
		var anim = GetComponent<Animator>();
		var clipLen = anim.GetCurrentAnimatorStateInfo(0).length;
		var randomStart = Random.Range(0, clipLen);
		anim.Play("Tree_Idle", 0, randomStart / clipLen);
	}
}