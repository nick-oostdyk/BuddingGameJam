using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleUI : MonoBehaviour
{
	[Header("Toggle Key")]
	[SerializeField] private InputAction _action;
	[SerializeField] private GameObject _UIObject;

	private bool _enabled;

	// close all the toggleable UIs on game start
	void Start()
	{
		_UIObject.SetActive(false);
		_enabled = false;
	}

	void Update()
	{
		if (_action.WasPerformedThisFrame()) 
			_toggle();
	}

	// toggle if the object is active in the hierarchy
	private void _toggle()
	{
		_enabled = !_enabled;
		_UIObject.SetActive(_enabled);
	}

	private void OnEnable() => _action.Enable();
	private void OnDisable() => _action.Disable();
}
