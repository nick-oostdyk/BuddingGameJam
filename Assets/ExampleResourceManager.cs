using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleResourceManager : MonoBehaviour
{
	public void Start()
	{
		// iterate over all the children and subscribe to harvest event
		foreach (var r in GetComponentsInChildren<Resource>())
		{
			r.onResourceHarvest += _resourceInteractHandler;
		}
	}

	private void _resourceInteractHandler(Resource r)
	{
		// do somethign with the resource after the player has interacted with it
	}
}
