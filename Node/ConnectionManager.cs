using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager Instance { get; private set; }

    private Node firstNode;
    private List<Connection> connections = new List<Connection>();

    [SerializeField]
    private GameObject connectionPrefab;
    
    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    /*public void OnNodeClicked(Node node)
    {
        if (firstNode == null)
        {
            firstNode = node;
            firstNode.Highlight();
        }
        else if (firstNode != node)
        {
            // if (firstNode.GetArc() != null && node.GetArc() != null)
            //     return;
            // if (firstNode.GetArc() == null && node.GetArc() == null)
            //     return;

            if (firstNode.GetArc() == null && node.GetArc() == null)
            {
                CreateConnection(firstNode, node);
                return;
            }
            
            if (firstNode.GetArc() != null)
            {
                node.SetArc(firstNode.GetArc());
                firstNode.GetArc().AddNode(node);
            }
            else
            {
                firstNode.SetArc(node.GetArc());
                node.GetArc().AddNode(firstNode);
            }
            
            CreateConnection(firstNode, node);
        }
    }*/

    public void Reset()
    {
        if (firstNode == null) return;

        //firstNode.ResetColor();
        firstNode = null;
    }

    private void CreateConnection(Node nodeA, Node nodeB)
    {
        if (ConnectionExists(nodeA, nodeB))
        {
            Reset();
            return;
        }
        
        GameObject line = Instantiate(connectionPrefab, nodeA.gameObject.transform);
        Connection connection = line.GetComponent<Connection>();
        //connection.Initialize(nodeA, nodeB);
        connections.Add(connection);
        Reset();
    }

    private bool ConnectionExists(Node nodeA, Node nodeB)
    {
        foreach (Connection connection in connections)
        {
            if ((connection.FirstNode == nodeA && connection.SecondNode == nodeB) ||
                (connection.FirstNode == nodeB && connection.SecondNode == nodeA))
                return true;
        }
        
        return false;
    }
}
