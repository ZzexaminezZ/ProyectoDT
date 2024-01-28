using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private PickableItem[] _pickableItems;
    
    [SerializeField] private ItemPicker _itemPicker;

    [Header("UI")]
    [SerializeField] private UiItem _uiItemPrefab;
    [SerializeField] private Transform _uiItemsParent;
    private Dictionary<PickableItem, UiItem> _uiItems = new Dictionary<PickableItem, UiItem>();

    private int _itemsStored = 0;

    public Action OnWin;

    private void Awake()
    {
        CreateUIItems();
        _itemPicker.OnItemStored += ItemStored;
    }

    private void CreateUIItems()
    {
        foreach (var item in _pickableItems)
        {
            var uiItem = Instantiate(_uiItemPrefab, _uiItemsParent);
            uiItem.SetItem(item);
            _uiItems.Add(item, uiItem);
        }
    }

    private void ItemStored(PickableItem item)
    {
        if (item == null) return;

        _uiItems[item].SetItemStored();
        _itemsStored++;

        if (_itemsStored == _pickableItems.Length)
        {
            OnWin?.Invoke();
        }
    }
}
