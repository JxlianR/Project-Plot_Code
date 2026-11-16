using System.Collections.Generic;
using UnityEngine;

public class ArcData
{
    public ArcType type;
    public CharacterData characterData;
    public List<NodeData> nodes = new List<NodeData>();
    public Vector3 initialPos;
    public ArcColorThemeSet.ArcColorTheme colorTheme;
}
