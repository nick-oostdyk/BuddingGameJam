using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleUI : MonoBehaviour
{
	[Header("Toggle Key")]
	[SerializeField] private InputAction _action;
	[SerializeField] public GameObject UIObject;
	[SerializeField] private bool _startEnabled;

	private bool _enabled;

	// close all the toggleable UIs on game start
	void Start()
	{
		UIObject.SetActive(_startEnabled);
		_enabled = _startEnabled;
	}

	void Update()
	{
		if (_action.WasPerformedThisFrame()) 
			Toggle();
	}

	// toggle if the object is active in the hierarchy
	public void Toggle()
	{
		_enabled = !_enabled;
		UIObject.SetActive(_enabled);
	}

	private void OnEnable() => _action.Enable();
	private void OnDisable() => _action.Disable();
}
