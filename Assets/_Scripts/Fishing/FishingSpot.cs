using System;
using UnityEngine;

public class FishingSpot : MonoBehaviour, IInteractable
{
	public event EventHandler OnInteract;
	public void Interact()
	{
		var gameFlags = GameManager.Instance.GameFlags;
		var dialogue = DialogueBoxManager.Instance;

		if (!gameFlags.HasFlag(GameFlag.FISH_ROD))
			dialogue.PushText("Looks like a good place to fish. If only I had a rod!");
		
		else OnInteract?.Invoke(this, EventArgs.Empty);
	}
}