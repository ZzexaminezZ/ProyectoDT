using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiItem : MonoBehaviour
{
    private Image _image;

    public void SetItem(PickableItem item)
    {
        if(_image == null)
        {
            _image = GetComponent<Image>();
        }
        _image.sprite = item.Sprite;

        _image.color = Color.black;
    }

    public void SetItemStored()
    {
        _image.color = Color.white;
    }
}
