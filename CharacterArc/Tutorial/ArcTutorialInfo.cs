using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ArcTutorialInfo")]
public class ArcTutorialInfo : ScriptableObject
{
    [TableList]
    public NodeTutorialEntry[] tutorialEntries;
}
