using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DayCycleController : MonoBehaviour
{
	[SerializeField] private Volume _vignetteVolume;
	[SerializeField] private bool _disable;

	private bool _dayStarting = false;
	private float currWeight => _vignetteVolume.weight;

	public void Awake()
	{
		TimeManager.Clock.OnDaytimeEvent += _handleDaytimeEvent;
	}

	// when the time changes tween to the brightness of the new time
	private void _handleDaytimeEvent(TimeManager.Clock.DaytimeEvent evnt)
	{
		switch (evnt)
		{
			case TimeManager.Clock.DaytimeEvent.MORNING:
				TweenToValue(.3f, 1f);
				break;
			case TimeManager.Clock.DaytimeEvent.NOON:
				TweenToValue(.0f);
				break;
			case TimeManager.Clock.DaytimeEvent.EVENING:
				TweenToValue(.3f);
				break;
			case TimeManager.Clock.DaytimeEvent.NIGHT:
				TweenToValue(.5f);
				break;
			case TimeManager.Clock.DaytimeEvent.SLEEP:
				TweenToValue(.9f, 1f);
				break;
		}
	}

	public async void TweenToValue(float value, float durationSeconds = 5f)
	{
		print($"tweening to value {value} from value {currWeight}");

		var frameTimeMS = 4;
		var dtSeconds = frameTimeMS * 0.001f;
		var iterations = durationSeconds / dtSeconds;

		var startingValue = currWeight;

		var delta = (1f / iterations) * (value - startingValue);

		for (int i = 0; i < iterations; ++i)
		{
			_vignetteVolume.weight = startingValue + i * delta;
			await System.Threading.Tasks.Task.Delay(frameTimeMS);
		}
		_vignetteVolume.weight = value;
	}

}
