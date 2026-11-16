using System;
using Sirenix.OdinInspector;

[Serializable]
public class NodeTutorialEntry
{
    [MultiLineProperty(8)]
    public string summary;
    [MultiLineProperty(4)] [OnValueChanged("ClampArraySize")]
    public string[] keyElements = new string[6];

    private const int Limit = 6;

    private void ClampArraySize()
    {
        if (keyElements != null & keyElements.Length != Limit)
            Array.Resize(ref keyElements, Limit);
    }
}
