using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType : short
{
	NONE,

	STONE,
	BOULDER,

	DRIFTWOOD,
	TREE,

	FIBER,
}

public abstract class Resource : MonoBehaviour, IInteractable
{
	protected SpriteRenderer _sr;

	public event System.EventHandler OnInteract;
	public virtual void Interact() => OnInteract?.Invoke(this, EventArgs.Empty);

	public abstract ResourceType Type { get; }

	public void Awake()
	{
		_sr = GetComponent<SpriteRenderer>();
		_sr.flipX = UnityEngine.Random.Range(0, 2) == 0;
	}
}