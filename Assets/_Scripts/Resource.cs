using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
	STONE,
	BOULDER,
	METAL,

	DRIFTWOOD,
	LOGS,

	FIBER,
	ROPE,

	NUM_RESOURCES
}

public class Resource : MonoBehaviour, IInteractable
{
	[SerializeField] private ResourceType _ResourceType;

	public void Interact()
	{
		print($"Interaction with {name}");
	}
}