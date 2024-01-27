using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    private PickableItem _pickedItem;

    public PickableItem PickedItem => _pickedItem;

    private PickableItem _itemToPick;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_pickedItem != null)
        {
            if (other.TryGetComponent(out ItemStorage storage))
            {
                storage.StartStoring(_pickedItem);
                storage.OnStoredItem += () => _pickedItem = null;
            }
            return;
        }
        if (other.TryGetComponent(out PickableItem pickable))
        {
            _itemToPick = pickable;
            _itemToPick.StartPicking();
            _itemToPick.OnPickItem += item =>
            {
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
            }
        }
    }
}
