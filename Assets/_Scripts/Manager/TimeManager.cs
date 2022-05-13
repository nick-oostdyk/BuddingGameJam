using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	private static float _timeScale = 1f;
	private static float _timeStart = 0f;

	private static bool _started;
	public static void StartTimer()
	{
		if (_started) return;
		_started = true;
		_timeStart = Time.time;
		Clock.Begin();
	}

	public static float TimeSinceStart => Time.time - _timeStart;
	public static int TimeSinceStartInt => Mathf.FloorToInt(Time.time - _timeStart);
	public static float TimeRemaining => 1000f - TimeSinceStart;
	public static int TimeRemainingInt => 1000 - Mathf.FloorToInt(TimeSinceStart);

	private void FixedUpdate() => Clock.TickTock();

	public static class Clock
	{
		private const float _MORNING_START_TIME = 0f;
		private const float _NOON_START_TIME = 80f;
		private const float _EVENING_START_TIME = 180f;
		private const float _NIGHT_START_TIME = 270f;
		private const float _KNOCKOUT_TIME = 333f;

		public enum DaytimeEvent
		{
			MORNING,
			NOON,
			EVENING,
			NIGHT,
			SLEEP,
		}

		public static float DayLength { get => 333f / _timeScale; }
		public static float TimeIntoDay => TimeSinceStart - _dayStart;
		public static System.Action<DaytimeEvent> OnDaytimeEvent;

		private static DaytimeEvent _time;
		private static float _dayStart;

		public static void Begin()
		{
			OnDaytimeEvent += _onMorningHandler;
			OnDaytimeEvent += _onSleepHandler;
			_setTime(DaytimeEvent.MORNING);
		}

		public static void TickTock()
		{
			switch (_time)
			{
				case DaytimeEvent.MORNING:
					if (TimeIntoDay > _NOON_START_TIME / _timeScale)
						_setTime(DaytimeEvent.NOON);
					break;

				case DaytimeEvent.NOON:
					if (TimeIntoDay > _EVENING_START_TIME / _timeScale)
						_setTime(DaytimeEvent.EVENING);
					break;

				case DaytimeEvent.EVENING:
					if (TimeIntoDay > _NIGHT_START_TIME / _timeScale)
						_setTime(DaytimeEvent.NIGHT);
					break;

				case DaytimeEvent.NIGHT:
					if (TimeIntoDay > _KNOCKOUT_TIME / _timeScale)
						_setTime(DaytimeEvent.SLEEP);
					break;
			}
		}

		private static void _onMorningHandler(DaytimeEvent evnt)
		{
			if (evnt != DaytimeEvent.MORNING) return;
			_dayStart = TimeSinceStart;
		}

		private static async void _onSleepHandler(DaytimeEvent evnt)
		{
			if (evnt != DaytimeEvent.SLEEP) return;

			GameManager.Instance.SetState(GameState.SLEEP);
			await Util.DelayRunAction(5000, () => _setTime(DaytimeEvent.MORNING));
			GameManager.Instance.SetState(GameState.PLAY);
		}

		private static void _setTime(DaytimeEvent daytimeEvent)
		{
			_time = daytimeEvent;
			OnDaytimeEvent?.Invoke(daytimeEvent);
			print(daytimeEvent);
		}
	}
}
