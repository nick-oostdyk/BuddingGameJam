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
	
	[Header("Minigame 2")]
	[SerializeField] public int NumBeats;
}
