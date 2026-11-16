using System;
using UnityEngine;
using UnityEngine.UI;

public class NodeContextTabButton : MonoBehaviour
{
    [SerializeField]
    private NodeContextMenu nodeContextMenu;
    
    [SerializeField]
    private GameObject page;

    private Toggle toggle;
    private Image image;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);

        image = GetComponent<Image>();
    }

    private void Start()
    {
        image.color = Color.white;
    }

    private void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
            nodeContextMenu.OpenPage(page);
    }
}
