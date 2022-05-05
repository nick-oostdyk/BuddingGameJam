using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resource : MonoBehaviour, IHarvestable
{
	public abstract ItemType ItemDrop { get; }
	public abstract void Harvest(Player p);
}