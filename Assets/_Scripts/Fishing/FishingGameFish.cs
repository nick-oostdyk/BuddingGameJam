using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// fish entity from fishing phase 2
public class FishingGameFish : FishingGameEntity
{
	[HideInInspector] public FishItem FishItem;
	private Collider2D _zoneCollider;
	private Collider2D _collider;

	public void Init(FishItem fish)
	{
		FishItem = fish;
		transform.localPosition = Vector3.zero;

		_sr.sprite = FishItem.Sprite;
		_maxSpeed = FishItem.SwimSpeed * .75f + .25f;
		_acceleration = FishItem.SwimSpeed * 1.5f + 1.5f;

		_collider = GetComponent<Collider2D>();
		_zoneCollider = GameObject.FindGameObjectWithTag("InZone").GetComponent<Collider2D>();
		_zoneCollider.transform.localScale = new Vector3(fish.ZoneSize, 1f, 1f);
	}

	public bool GetIsInZone() => _collider.IsTouching(_zoneCollider);
}
