using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ClickableView<T> : MonoBehaviour where T : ClickableView<T>
{
    [SerializeField]
    protected Collider2D _collider2D;

    public event Action<T> OnClicked = delegate { };

    public virtual void OnMouseDown()
    {
        if (IsPointerOverUI())
            return;

        Interact();
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    protected virtual void Interact()
    {
        OnClicked.Invoke(this as T);
    }
}