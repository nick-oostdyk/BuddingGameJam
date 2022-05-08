using System;
public interface IInteractable
{
	public event EventHandler OnInteract;
	public void Interact();
}