using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Entity
{
	public FishItem FishItem;
	private SpriteRenderer _sr;
	private Collider2D _zoneCollider;
	private Collider2D _collider;


	private void Start() => _sr = GetComponent<SpriteRenderer>();
	public void Init(FishItem fish)
	{
		FishItem = fish;
		transform.localPosition = Vector3.zero;

		_sr.sprite = FishItem.Sprite;
		_maxSpeed = FishItem.SwimSpeed * .75f + .25f;
		_acceleration = FishItem.SwimSpeed * 1.5f + 1.5f;

		_collider = GetComponent<Collider2D>();
		_zoneCollider = GameObject.FindGameObjectWithTag("InZone").GetComponent<Collider2D>();
	}

	public bool GetIsInZone() => _collider.IsTouching(_zoneCollider);
	public void ClampPosition()
	{
		var localPos = transform.localPosition;
		localPos.x = Mathf.Clamp(localPos.x, -4.5f, 4.5f);
		transform.localPosition = localPos;
	}
}
