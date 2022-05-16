using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerOrderManager : MonoBehaviour
{
	private Player _player;
	private List<Transform> _layerOrderObjects = new List<Transform>();

	private Vector3 _feetPos => _player.transform.Find("Feet").position;
	private SpriteRenderer _playerRenderer;

	void Start()
	{
		_player = FindObjectOfType<Player>();
		_playerRenderer = _player.transform.GetChild(0).GetComponent<SpriteRenderer>();
		_playerRenderer.sortingOrder = 3;

		RebuildLayeredObjectList();
	}

	void FixedUpdate()
	{
		System.Action<Transform> setSROrder = (obj) => {

			if (obj.transform.position.y > _feetPos.y)
				obj.parent.GetComponent<SpriteRenderer>().sortingOrder = 2;
			else
				obj.parent.GetComponent<SpriteRenderer>().sortingOrder = 4;
		};

		// find closest LayerOrder
		foreach (var obj in _layerOrderObjects) setSROrder(obj);
	}

	public void RebuildLayeredObjectList()
	{
		// empty the list
		_layerOrderObjects.Clear();

		// grab all the layerorder objects in the scene
		var objects = GameObject.FindGameObjectsWithTag("LayerOrder");

		// add them to the list & set their layer order
		foreach (var obj in objects)
		{
			_layerOrderObjects.Add(obj.transform);
			obj.GetComponentInParent<SpriteRenderer>().sortingLayerName = "Midground";
		}
	}
}
