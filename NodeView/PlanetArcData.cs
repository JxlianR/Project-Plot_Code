using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlanetArcData")]
public class PlanetArcData : ScriptableObject
{
    public List<ArcSerializeData> data;
}

[Serializable]
public class ArcSerializeData
{
    public ArcType arcType;
    public CharacterData characterData;
    public List<NodeData> nodeData;
}

public enum ArcType
{
    PositiveChange,
    Flat,
    Corruption,
    Disillusionment,
    Fall
}
