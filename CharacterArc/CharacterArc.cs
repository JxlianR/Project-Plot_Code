using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterArc : MonoBehaviour
{
    private int id;

    public ArcData Data { get; private set; }

    public List<Node> Nodes { get; private set; }
    private Node lastNode;
    
    [SerializeField] [Range(0, 1)]
    private float fadeStep;

    [SerializeField]
    private ArcTutorialInfo tutorialInfo;
    
    // Change later
    public GameObject skyMapCollider;
    
    public void InitializeNewArc(ArcType arcType, CharacterData character, ArcColorThemeSet.ArcColorTheme colorThemeSet)
    {
        Data = new ArcData
        {
            type =  arcType,
            characterData = character,
            initialPos = transform.localPosition,
            colorTheme = colorThemeSet
        };

        Nodes = new List<Node>();
        
        Node[] nodesInArc = GetComponentsInChildren<Node>();
        Nodes = nodesInArc.ToList();

        SetUpNewArc();
        
        //id = CharacterArcManager.Instance.CharacterArcs.Count;
        // Following line is Showcase Code
        id = NodeViewManager.Instance.CurrentPlanet.Arcs.Count;
        CharacterArcManager.Instance.SwitchArcs(this);
    }

    // Called when an arc is reused from pool and needs to be set up
    public void ReuseArc(ArcData newData, int arcCount)
    {
        Data = newData;
        transform.localPosition = Data.initialPos;
        id = arcCount;
        
        SetUpLoadedArc();
    }

    private void SetUpNewArc()
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            Node node = Nodes[i];
            NodeTutorialEntry entry = tutorialInfo.tutorialEntries.Length > i ? tutorialInfo.tutorialEntries[i] : null;
            
            node.Initialize(this, Data.colorTheme.nodeMaterial, entry);
            node.ApplyData(new NodeData());
            Data.nodes.Add(node.Data());
            
            if (i + 1 < Nodes.Count)
                node.NodeConnection.Initialize(node, Nodes[i + 1], Data.colorTheme.connectionMaterial);
        }

        lastNode = Nodes.Last();
    }

    private void SetUpLoadedArc()
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            Node node = Nodes[i];
            NodeData nodeData = Data.nodes[i];
            
            // Can this be deleted? Pool for every type = same tutorial entry
            NodeTutorialEntry entry = tutorialInfo.tutorialEntries.Length > i ? tutorialInfo.tutorialEntries[i] : null; 
            node.Initialize(this, Data.colorTheme.nodeMaterial, entry);
            node.ApplyData(nodeData);
            
            if (i + 1 < Nodes.Count)
                node.NodeConnection.Initialize(node, Nodes[i + 1], Data.colorTheme.connectionMaterial);
        }

        lastNode = Nodes.Last();
    }

    public void AddNode(Node node)
    {
        Nodes.Add(node);
        node.Initialize(this, Data.colorTheme.nodeMaterial);
        Data.nodes.Add(node.Data());
        
        Vector3 nodePos = node.transform.position;
        nodePos.x = lastNode.transform.position.x + 1f;
        node.transform.position = nodePos;
        
        lastNode.NodeConnection.Initialize(lastNode, node, Data.colorTheme.connectionMaterial);
        
        lastNode = node;
    }

    private void FadeArc()
    {
        // id + 1 bc id starts at 0
        float value = 1f - Mathf.Pow(1f - fadeStep, id + 1);
        value = 1f - Mathf.Clamp(value, 0f, 1f);
        
        foreach (Node node in Nodes)
            node.FadeOut(value);
    }

    public void CheckFilter(List<string> filters)
    {
        if (filters.Count == 0)
        {
            foreach (Node node in Nodes)
                node.ApplyFilter(false);

            return;
        }
        
        foreach (Node node in Nodes)
        {
            bool matchesFilter = filters.Any(filterCharacter =>
                filterCharacter == Data.characterData.name || node.CharacterRelated(filterCharacter));
            
            node.ApplyFilter(matchesFilter);
        }
    }

    public void OnSelected()
    {
        transform.localPosition = Vector3.zero;
        
        foreach (Node node in Nodes)
            node.FadeOut(1f);
    }

    public void OnDeselected()
    {
        transform.localPosition = Data.initialPos;
        FadeArc();
    }
    
    public Node GetNextNode(Node node)
    {
        int index = Nodes.IndexOf(node);
        int nextIndex = index + 1;

        return nextIndex >= Nodes.Count ? null : Nodes[nextIndex];
    }

    public void ResetArc()
    {
        Data = new ArcData();
        id = 0;
        lastNode = null;

        foreach (Node node in Nodes)
            node.ResetVisuals();
    }

    public Color GetColorThemeButton() => Data.colorTheme.buttonColor;
    
    //Showcase Code
    /*public void SerializeArc(ArcSerializeData data)
    {
        for (int i = 0; i < Data.nodes.Count; i++)
        {
            Data.nodes[i].SetData(data.nodeData[i]);
        }
    }*/

    public string GetCharacterName() => Data.characterData.name;

    public CharacterData GetCharacterData() => Data.characterData;
}
