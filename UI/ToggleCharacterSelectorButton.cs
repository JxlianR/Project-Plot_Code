using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToggleCharacterSelectorButton : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private TMP_Text text; 
    
    private string characterId;
    private CharacterArc characterArc;
    
    private void Awake()
    {
        //toggle = GetComponent<Toggle>();
        //text = GetComponentInChildren<TMP_Text>();
    }

    private void OnEnable()
    {
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
    }
    
    public void OnButtonInstantiated(string label, ToggleGroup group)
    {
        characterId = label;
        text.text = characterId;
        CharacterArcSpawner.Instance.SelectCharacter(characterId, this);
        Debug.Log("CharSelection: " + characterId);

        toggle.group = group;
        toggle.isOn = true;
    }

    private void OnToggleValueChanged(bool isOn)
    {
        if (isOn && characterArc != null)
            CharacterArcManager.Instance.SwitchArcs(characterArc);
    }

    public void SetCharacterArc(CharacterArc arc) => characterArc = arc;
}
