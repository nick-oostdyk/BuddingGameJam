using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerOrderManager : MonoBehaviour
{
	private Player _player;
	private List<Transform> _layerOrderObjects = new List<Transform>();

	private Vector3 _feetPos => _player.transform.Find("Feet").position;
	private Vector3 _pPos => _player.transform.position;
	private SpriteRenderer _playerRenderer;

	void Start()
	{
		_player = FindObjectOfType<Player>();
		_playerRenderer = _player.transform.GetChild(0).GetComponent<SpriteRenderer>();

		RebuildLayeredObjectList();
	}

	void FixedUpdate()
	{
		// find closest LayerOrder
		float minDist = float.MaxValue;
		Vector3 minPos = Vector3.zero;
		foreach (var obj in _layerOrderObjects)
		{
			var dist = Vector2.SqrMagnitude(obj.transform.parent.position - _pPos);
			if (dist < minDist)
			{
				minDist = dist;
				minPos = obj.transform.position;
			}
		}

		// check player y position against closest LayerOrder position
		_playerRenderer.sortingOrder = _feetPos.y <= minPos.y ? 4 : 2;
	}

	public void RebuildLayeredObjectList()
	{
		_layerOrderObjects.Clear();

		var objects = GameObject.FindGameObjectsWithTag("LayerOrder");

		foreach (var obj in objects)
		{
			_layerOrderObjects.Add(obj.transform);
			obj.GetComponentInParent<SpriteRenderer>().sortingOrder = 3;
		}
	}
}
