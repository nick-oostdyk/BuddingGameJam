using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
	private void Start()
	{
		GetComponent<FishingSpot>().OnInteract += _onFish;
	}

	private void _onFish(object sender, EventArgs args)
	{
		
	}
}
