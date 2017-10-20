using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MapObject
{
    [SerializeField]
    private bool _isBreakable;

    private Item _item = null;

	void Awake ()
    {
    }
	
	void Update ()
    {
		
	}

    public void SetItem(Item item)
    {
        _item = item;
        _item.gameObject.SetActive(false);
        _item.transform.position = transform.position;
    }

    public void OnDestroy()
    {
        if (_item)
        {
            _item.gameObject.SetActive(true);
        }
    }
}
