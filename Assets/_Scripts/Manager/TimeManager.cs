using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	private static float _timeStart = 0f;

	public static void StartTimer() => _timeStart = Time.time;
	public static float TimeSinceStart => Time.time - _timeStart;
	public static int TimeSinceStartInt => Mathf.FloorToInt(Time.time - _timeStart);
	public static float TimeRemaining => 1000f - TimeSinceStart;
	public static int TimeRemainingInt => 1000 - Mathf.FloorToInt(TimeSinceStart);
}
