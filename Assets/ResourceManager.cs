using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ResourceManager : MonoBehaviour
{
	public void Start()
	{
		// iterate over all the children and subscribe to harvest event
		foreach (var r in GetComponentsInChildren<Resource>())
		{
			r.onResourceHarvest += _resourceInteractHandler;
		}
	}

	private async void _resourceInteractHandler(Resource r)
	{
		r.gameObject.SetActive(false);
		await Task.Delay(Random.Range(10000, 50000));
		if (r == null) return;
		r.gameObject.SetActive(true);
	}

}
