using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LayerOrderManager : MonoBehaviour
{
    private List<GameObject> _objectsInScene = new List<GameObject>();
    private Player _player;

    void Start()
    {
        _player = FindObjectOfType<Player>();
        RebuildLayeredObjectList();
    }

    void Update()
    {
        foreach (var o in _objectsInScene)
        {
            if (o.tag == "Player")
                continue;
            else if (o.transform.position.y - 0.5f >= _player.transform.position.y)
                _player.GetComponentInChildren<SpriteRenderer>().sortingOrder = 4;
            else 
                _player.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;

        }
    }

    public void RebuildLayeredObjectList()
    {
        _objectsInScene.Clear();

        foreach (var o in FindObjectsOfType<GameObject>())
        {
            if (o.transform.Find("LayerOrder"))
            {
                _objectsInScene.Add(o);
                if(o.tag != "Player")
                    o.GetComponent<SpriteRenderer>().sortingOrder = 3;
            }
        }

        _objectsInScene = _objectsInScene.OrderBy(i => i.transform.position.y).ToList();
    }
}
