using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainContextMenu : ContextMenu
{
    [SerializeField]
    private GameObject nodePrefab;

    public void CreateNewNode()
    {
        Close();

        CharacterArc currentArc = CharacterArcManager.Instance.CurrentArc;
        if (!currentArc) return;
        
        float y = Random.Range(-3f, 3f);
        Vector3 position = new Vector3(0, y, 0);
        
        GameObject node = Instantiate(nodePrefab, position, quaternion.identity, currentArc.transform);
        Node nodeComponent = node.GetComponent<Node>();
        
        currentArc.AddNode(nodeComponent);
    }
}
