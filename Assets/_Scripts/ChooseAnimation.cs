using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// picks a random animation from an array set in the inspector
public class ChooseAnimation : MonoBehaviour
{
	private Animator _anim;

	[SerializeField] private string[] _animations;

	private void Start()
	{
		_anim = GetComponent<Animator>();

		var animNum = Random.Range(0, _animations.Length);
		var animName = _animations[animNum];

		_anim.Play(animName);
	}
}
