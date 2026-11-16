using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddArcButton : MonoBehaviour
{
    [SerializeField]
    private GameObject arcButtonPrefab;
    public List<GameObject> ArcButtons { get; private set; } = new List<GameObject>();

    private ToggleGroup toggleGroup;
    
    [SerializeField]
    private NodeUIColorManager colorManager;
    
    // Awake is not being called bc parent object is disabled (at least colorManager is null, when called in DeleteButtons()). Commented out for Showcase.
    // Find better solution for whole serializing system later
    private void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        //colorManager = GetComponent<NodeUIColorManager>();
    }

    public void SpawnButton(string characterName)
    {
        GameObject arcButton = Instantiate(arcButtonPrefab, transform);
        ArcButtons.Add(arcButton);
        
        arcButton.transform.SetSiblingIndex(transform.GetSiblingIndex());
        arcButton.GetComponent<ToggleCharacterSelectorButton>().OnButtonInstantiated(characterName, toggleGroup);
        colorManager.ApplyButtonColors();

        //colorManager.SelectButtonProgrammatically(button);
    }

    public void DeleteButtons()
    {
        for (int i = ArcButtons.Count - 1; i >= 0; i--)
        {
            GameObject button = ArcButtons[i];
            ArcButtons.Remove(button);
            Destroy(button);
        }
        
        colorManager.ResetColorIndex();
    }
}
