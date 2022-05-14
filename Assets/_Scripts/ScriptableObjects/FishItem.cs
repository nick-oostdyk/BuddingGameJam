using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "new Fish", menuName = "ScriptableObjects/Fish")]
public class FishItem : ScriptableObject
{
	[Header("Fish")]
	[SerializeField] public FishType Type;
	[SerializeField] public Sprite Sprite;

	[Header("Minigame 1")]
	[SerializeField] public float SwimSpeed;
	[SerializeField] public float PlayerFillTime;
	[SerializeField] public float FishFillTime;
	[Range(0.75f, 1.5f)]
	[SerializeField] public float PerlinStep;
	[Range(1f, 8f)]
	[SerializeField] public float ZoneSize;
	
	[Header("Minigame 2")]
	[SerializeField] public int NumBeats;
	[Range(.4f, 1f)]
	[SerializeField] public int NoteTime;
}
