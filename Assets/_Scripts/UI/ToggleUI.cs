using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ToggleUI : MonoBehaviour
{
	[Header("Toggle Key")]
	[SerializeField] private InputAction _action;
	[SerializeField] private Button _assocButton;
	[SerializeField] public GameObject UIObject;

	private bool _enabled;

	// close all the toggleable UIs on game start
	void Start()
	{
		UIObject.SetActive(false);
		_enabled = false;

		if (_assocButton is not null)
			_assocButton.onClick.AddListener(Toggle);
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
