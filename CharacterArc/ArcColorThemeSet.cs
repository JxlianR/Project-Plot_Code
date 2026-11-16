using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Arc Color Theme Set")]
public class ArcColorThemeSet : ScriptableObject
{
    [Serializable]
    public struct ArcColorTheme
    {
        public Material nodeMaterial;
        public Material connectionMaterial;
        public Color buttonColor;
    }

    public List<ArcColorTheme> themes;
}
