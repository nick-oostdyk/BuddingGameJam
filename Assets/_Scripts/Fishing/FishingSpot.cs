using System;
using UnityEngine;

public class FishingSpot : MonoBehaviour, IInteractable
{
	public event EventHandler OnInteract;
	public void Interact()
	{
		if (!GameManager.Instance.GameFlags.HasFlag(GameFlag.FISH_ROD))
		{
			DialogueBoxManager.Instance.PushText("", "Looks like a good place to fish. If only I had a rod!");
			return;
		}
		OnInteract?.Invoke(this, EventArgs.Empty);
	}
}