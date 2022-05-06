using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITimerBehaviour : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _timerText;

	private void FixedUpdate() => _timerText.text = TimeManager.TimeRemainingInt.ToString();
}
