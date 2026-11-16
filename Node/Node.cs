using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private NodeData data;
    public NodeTutorialEntry TutorialEntry { get; private set; }

    public Connection NodeConnection { get; private set; }
    private CharacterArc arc;

    private SpriteRenderer spriteRenderer;
    private Material normalMaterial;
    private Material defaultMaterial;
    
    private bool filteredOut;
    [SerializeField] [Range(0, 1)]
    private float filteredOpacity;

    private float baseBrightness;
    [SerializeField]
    private float highlightedBrightness;

    private bool completeEdit;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        NodeConnection = GetComponentInChildren<Connection>();
    }

    public void Initialize(CharacterArc characterArc, Material material, NodeTutorialEntry tutorial = null)
    {
        TutorialEntry = tutorial;
        defaultMaterial = spriteRenderer.material;
        arc = characterArc;
        normalMaterial = material;

        baseBrightness = spriteRenderer.material.GetFloat("_brightness");
    }

    public void ApplyData(NodeData newData)
    {
        data = newData;
        OnCompleteEdit();
    }

    public NodeData Data() => data;

    public void SaveData(string eventName, string description, string associatedCharacters, string characterDevelopment, string ultimateGoal)
    {
        if (!ChangesMade(eventName, description, associatedCharacters, characterDevelopment, ultimateGoal, out List<string> characters))
            return;
        
        data.eventName = eventName;
        data.description = description;
        data.associatedCharacters = characters;

        arc.GetCharacterData().development = characterDevelopment;
        arc.GetCharacterData().goal = ultimateGoal;
        
        OnCompleteEdit();
        
        if (CharacterFilterManager.instance != null)
        {
            CharacterFilterManager.instance.OnNodesChanged();
        }
    }

    private bool ChangesMade(string eventName, string description, string associatedCharacters, string characterDevelopment, string ultimateGoal, out List<string> characters)
    {
        characters = associatedCharacters.Split(',').Select(n => n.Trim())
            .Where(n => !string.IsNullOrWhiteSpace(n)).ToList();

        if (eventName == data.eventName && description == data.description &&
            characters.SequenceEqual(data.associatedCharacters) &&
            characterDevelopment == arc.GetCharacterData().development &&
            ultimateGoal == arc.GetCharacterData().goal)
            return false;

        return true;
    }

    private void OnCompleteEdit()
    {
        if (!string.IsNullOrWhiteSpace(data.eventName) || !string.IsNullOrWhiteSpace(data.description))
        {
            if (completeEdit) return;
            spriteRenderer.material = normalMaterial;
        }
        else
        {
            if (!completeEdit) return;
            spriteRenderer.material = defaultMaterial;
        }

        completeEdit = !completeEdit;
        baseBrightness = spriteRenderer.material.GetFloat("_brightness");
    }
    
    public bool CharacterRelated(string character)
    {
        foreach (string associatedCharacter in data.associatedCharacters)
        {
            if (associatedCharacter == character)
                return true;
        }
        
        return false;
    }

    public void ApplyFilter(bool active)
    {
        if (!active)
        {
            if (filteredOut) return;
            
            Color color = spriteRenderer.color;
            color.a = filteredOpacity;
            spriteRenderer.color = color;
            
            filteredOut = true;
        }
        else
        {
            if (!filteredOut) return;
            
            Color color = spriteRenderer.color;
            color.a = 1;
            spriteRenderer.color = color;
            
            filteredOut = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CharacterArcManager.Instance.CurrentArc != GetArc())
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            InputManager.Instance.NodeClick(this);
            AudioManager.Instance.Play("ui_snd_select");
        }
    }

    public void SetHighlighted(bool highlighted)
    {
        Material material = spriteRenderer.material;
        
        float value = highlighted ? highlightedBrightness : baseBrightness;
        material.SetFloat("_brightness", value);
    }

    public void FadeOut(float value)
    {
        Material material = spriteRenderer.material;
        material.SetFloat("_FadeDistance", value);
        
        NodeConnection.Fade(value);
    }

    public void ResetVisuals()
    {
        spriteRenderer.material = defaultMaterial;
    }
    
    public CharacterArc GetArc() => arc;

    //Showcase Code
    public void SetData(NodeData nodeData)
    {
        data.eventName = nodeData.eventName;
        data.description = nodeData.description;
        data.associatedCharacters = nodeData.characters.Split(',').Select(n => n.Trim())
            .Where(n => !string.IsNullOrWhiteSpace(n)).ToList();
        
        OnCompleteEdit();
    }
}
