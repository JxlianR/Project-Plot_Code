using System;
using TMPro;
using UnityEngine;

public class SolarSystemInteractable : Interactable
{
    [SerializeField]
    private GameObject nameCanvas;
    private TMP_Text nameCanvasText;

    protected override void Start()
    {
        base.Start();

        nameCanvasText = nameCanvas.GetComponentInChildren<TMP_Text>();
    }
    
    private void LateUpdate()
    {
        if (nameCanvas == null)
            return;

        if (nameCanvas.gameObject.activeSelf)
            nameCanvas.transform.rotation = navigationManager.activeCamera.transform.rotation;
    }

    private void OnMouseEnter()
    {
        HandleMouseEnter();
    }

    protected virtual void HandleMouseEnter()
    {
        if (nameCanvas == null) return;

        nameCanvasText.text = gameObject.name;
        nameCanvas.gameObject.SetActive(true);
        
        UIManager.instance.UpdateHoverFeedback(ObjectName);
    }

    private void OnMouseExit()
    {
        if (nameCanvas == null)
            return;
        
        nameCanvas.gameObject.SetActive(false);
        UIManager.instance.UpdateHoverFeedback("");
    }
}
