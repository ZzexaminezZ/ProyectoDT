using System;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    private PickableItem _pickedItem;

    public PickableItem PickedItem => _pickedItem;


    private PickableItem _itemToPick;
    [SerializeField] private PickerProgressBar uiProgressBar;

    public Action<bool> OnPickItem;
    public Action<PickableItem> OnItemStored;

    [Header("Item viewer")]
    [SerializeField] private SpriteRenderer _itemViewer;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_pickedItem != null)
        {
            if (other.TryGetComponent(out ItemStorage storage))
            {
                storage.StartStoring(_pickedItem);
                storage.OnStoredItem += () =>
                {
                    OnPickItem?.Invoke(false);
                    OnItemStored?.Invoke(_pickedItem);
                    _itemViewer.gameObject.SetActive(false);
                    _pickedItem = null;
                };
                
                if (uiProgressBar != null)
                {
                    storage.OnStoringProgress += uiProgressBar.UpdateProgressBar;
                    uiProgressBar.gameObject.SetActive(true);
                }
            }
            return;
        }
        
        if (other.TryGetComponent(out PickableItem pickable))
        {
            _itemToPick = pickable;
            if (uiProgressBar != null)
            {
                _itemToPick.OnPickingProgress += uiProgressBar.UpdateProgressBar;
                uiProgressBar.gameObject.SetActive(true);
            }
            _itemToPick.StartPicking();
            _itemToPick.OnPickItem += item =>
            {
                OnPickItem?.Invoke(true);
                _itemViewer.gameObject.SetActive(true);
                _itemViewer.sprite = item.Sprite;
                _pickedItem = item;
                _itemToPick = null;
            };
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_pickedItem != null)
        {
            if (other.TryGetComponent(out ItemStorage storage))
            {
                storage.EndStoring();
                storage.RemoveTempItem();
                
                if (uiProgressBar != null)
                {
                    uiProgressBar.RestartProgressBar();
                }
            }
            return;
        }
        
        if (other.TryGetComponent(out PickableItem pickable))
        {
            if (_itemToPick == pickable)
            {
                _itemToPick.EndPicking();
                _itemToPick.OnPickItem = null;
                _itemToPick = null;

                if (uiProgressBar != null)
                {
                    uiProgressBar.RestartProgressBar();
                }
            }
        }
    }
}
