using UnityEngine;

public abstract class PickableEvent : MonoBehaviour
{
    [SerializeField] private PickableItem trigger;

    private void Awake()
    {
        trigger.OnPickItem += OnPickItem;
    }

    protected abstract void OnPickItem(PickableItem obj);
}
