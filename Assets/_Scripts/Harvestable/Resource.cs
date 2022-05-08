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
	public event EventHandler OnInteract;
	public virtual void Interact() => OnInteract?.Invoke(this, EventArgs.Empty);

	public abstract ResourceType Type { get; }
}