using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContextMenu : MonoBehaviour
{
    [SerializeField]
    protected GameObject menuUI;

    [SerializeField]
    protected RectTransform rect;
    [SerializeField]
    private Canvas canvas;
    
    public bool IsOpen { get; protected set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        Close();
    }

    public void Open(Vector2 position)
    {
        IsOpen = true;
        menuUI.SetActive(true);
        ClampToScreen(position);
    }

    public virtual void Close()
    {
        IsOpen = false;
        menuUI.SetActive(false);
    }

    private void ClampToScreen(Vector2 mousePosition)
    {
        float screenWidth = Screen.width;

        Vector2 menuSize = rect.rect.size * canvas.scaleFactor;

        Vector2 pivot = new Vector2(0, 1);

        if (mousePosition.x + menuSize.x > screenWidth)
            pivot.x = 1;

        if (mousePosition.y - menuSize.y < 0)
            pivot.y = 0;

        rect.pivot = pivot;
        menuUI.transform.position = mousePosition;
    }

    public bool ClickedOnMenu(Vector2 mousePosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult hit in results)
        {
            if (hit.gameObject.transform.IsChildOf(transform))
                return true;

            if (hit.gameObject == gameObject)
                return true;
        }

        return false;
    }
}
