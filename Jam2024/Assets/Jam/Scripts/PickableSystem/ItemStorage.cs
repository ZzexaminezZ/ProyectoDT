using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ItemStorage : MonoBehaviour
{
    public List<PickableItem> storedItems;
    
    public Action<float> OnStoringProgress;
    public Action OnStoredItem;

    private bool _isStoring = false;
    private float _storingProgress = 0;
    private PickableItem _itemToStore;
    
    [SerializeField] private float storingSpeed = 0.1f;

    private void Update()
    {
        if (_isStoring && _storingProgress < 1)
        {
            _storingProgress += storingSpeed * Time.deltaTime;
            _storingProgress = Mathf.Clamp01(_storingProgress);
            
            OnStoring(_storingProgress);

            if (_storingProgress == 1)
            {
                EndStoring();
                OnStored();
                OnStoredItem = null;
                _itemToStore = null;
            }
        }
    }

    public void StartStoring(PickableItem item)
    {
        if (_isStoring)
        {
            return;
        }
        _isStoring = true;
        _itemToStore = item;
    }

    public void OnStoring(float progress)
    {
        OnStoringProgress?.Invoke(progress);
    }

    public void EndStoring()
    {
        if (!_isStoring)
        {
            return;
        }
        _isStoring = false;
        _storingProgress = 0;
        OnStoringProgress = null;
    }

    public void OnStored()
    {
        storedItems.Add(_itemToStore);
        OnStoredItem?.Invoke();
    }

    public void RemoveTempItem()
    {
        _itemToStore = null;
    }
}
