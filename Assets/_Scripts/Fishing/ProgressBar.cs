using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
	private GameObject _bar;
	private void Start() => _bar = transform.GetChild(0).gameObject;

	public float Progress { get; private set; }
	public void AddProgress(float deltaProgress) => SetProgress(Progress + deltaProgress);
	public void SetProgress(float progress)
	{
		Progress = Mathf.Clamp01(progress);
		_bar.transform.localScale = Vector3.up + Vector3.right * Progress;
	}
}