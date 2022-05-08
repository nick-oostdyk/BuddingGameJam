using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "new Fish", menuName = "ScriptableObjects/Fish")]
public class FishItem : ScriptableObject
{
	[SerializeField] public FishType Type;
	[SerializeField] public Sprite Sprite;
	[SerializeField] public float SwimSpeed;
	[SerializeField] public int NumBeats;
}
