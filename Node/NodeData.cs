using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class NodeData
{
    [HideInInspector]
    public int id;
    public string eventName;
    public string description;
    [HideInInspector]
    public List<string> associatedCharacters = new List<string>();
    public string characters;
    [HideInInspector]
    public int chronologicalMarker;
    [HideInInspector]
    public EventType eventTypes;
    public List<string> evidencePaths = new List<string>();
    
    [Flags]
    public enum EventType
    {
        None = 0,
        MajorPlot = 1 << 0,
        CharacterSpecific = 1 << 1
    };
}
