using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
	STONE,
	BOULDER,
	METAL,

	STICK,
	WOOD,

	FIBER,
	ROPE,

	NUM_RESOURCES
}

public abstract class Resource : MonoBehaviour, IInteractable
{
	public abstract ResourceType Type { get; }

	public abstract void Interact(Player p);
}
