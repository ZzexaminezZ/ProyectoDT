using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PickableItem : MonoBehaviour
{
    public Action<float> OnPickingProgress;
    public Action<PickableItem> OnPickItem;

    private bool _siBeingPick = false;
    private float _pickingProgress = 0;

    [SerializeField] private float _pickingSpeed = 0.1f;

    void Update()
    {
        if (_siBeingPick && _pickingProgress < 1)
        {
            _pickingProgress += _pickingSpeed * Time.deltaTime;
            _pickingProgress = Mathf.Clamp01(_pickingProgress);
            
            OnPicking(_pickingProgress);

            if (_pickingProgress == 1)
            {
                EndPicking();
                OnPick();
                OnPickItem = null;
            }
        }
    }

    public void StartPicking()
    {
        if (_siBeingPick)
        {
            return;
        }
        _siBeingPick = true;
    }

    public void OnPicking(float progress)
    {
        OnPickingProgress?.Invoke(progress);
    }

    public void EndPicking()
    {
        if (!_siBeingPick)
        {
            return;
        }
        _siBeingPick = false;
        _pickingProgress = 0;
        OnPickingProgress = null;
    }

    public void OnPick()
    {
        OnPickItem?.Invoke(this);
        gameObject.SetActive(false);
    }
}
