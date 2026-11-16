using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterArcManager : MonoBehaviour
{
    public static CharacterArcManager Instance { get; private set; }

    public List<ArcData> CharacterArcs { get; private set; } = new List<ArcData>();

    public CharacterArc CurrentArc { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void SwitchArcs(CharacterArc arc)
    {
        if (arc == CurrentArc) return;
        
        if (CurrentArc) CurrentArc.OnDeselected();
        if (arc) arc.OnSelected();
        
        CurrentArc = arc;
    }

    public void AddArc(ArcData arc) => CharacterArcs.Add(arc);

    public void RemoveArc(ArcData arc) => CharacterArcs.Remove(arc);
}
