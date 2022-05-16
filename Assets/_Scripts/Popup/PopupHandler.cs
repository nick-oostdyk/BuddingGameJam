using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupHandler : MonoBehaviour
{
    // attached to Player right now to test functionality
    [SerializeField] private Transform _popup;
    [SerializeField] private Sprite _sprite;
    private int t = 0;

    void Start()
    {
        Transform p = Instantiate(_popup, Vector3.zero, Quaternion.identity);
        p.GetComponent<PopupText>().Setup("big banana bitches", _sprite);
    }

    private void FixedUpdate()
    {
        // test making the popup go away
        t++;
        if (t >= 60)
            FindObjectOfType<PopupText>().Clear();
    }
}
