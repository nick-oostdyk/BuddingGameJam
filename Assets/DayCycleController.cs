using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class DayCycleController : MonoBehaviour
{
	[SerializeField] private Volume _vignetteVolume;

	[SerializeField] private bool _disable;

	private float _dayLength = 333f;
	private float _dayStartTime;

	private bool _dayStarting = false;

	private void Start()
	{
		StartDay();
	}

	private void FixedUpdate()
	{
		if (_dayStarting) return;
		if (_disable) return;

		float progressIntoDay = TimeManager.TimeSinceStart - _dayStartTime;
		var progressAsPercent = progressIntoDay / _dayLength;
		_vignetteVolume.weight = progressAsPercent;

		if (progressAsPercent > 1f)
			StartDay();
	}

	public async void StartDay()
	{
		_dayStarting = true;

		for (float i = _vignetteVolume.weight; i > 0; i -= 0.01f)
		{
			_vignetteVolume.weight = i;
			await System.Threading.Tasks.Task.Delay(15);
		}

		_dayStartTime = TimeManager.TimeSinceStart;
		_dayStarting = false;
	}

}
