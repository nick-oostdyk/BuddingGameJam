using System;
using UnityEngine;

public class FishingSpot : MonoBehaviour, IInteractable
{
	public event EventHandler OnInteract;
	public void Interact() => OnInteract?.Invoke(this, EventArgs.Empty);
}
