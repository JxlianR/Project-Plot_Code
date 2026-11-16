using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class ColorApplier : MonoBehaviour
{
    private NodeContextMenu nodeContextMenu;
    
    protected Color currentColor;

    protected virtual void Awake()
    {
        nodeContextMenu = GetComponentInParent<NodeContextMenu>();
        nodeContextMenu.applyColor.AddListener(ChangeColor);
    }

    private void OnDestroy()
    {
        nodeContextMenu.applyColor.RemoveListener(ChangeColor);
    }

    protected abstract void ChangeColor(Color newColor);
}
