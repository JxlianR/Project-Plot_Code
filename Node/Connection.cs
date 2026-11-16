using System;
using UnityEngine;

public class Connection : MonoBehaviour
{ 
    public Node FirstNode { get; private set; }
    public Node SecondNode{ get; private set; }

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        PositionUpdate();
    }

    public void Initialize(Node nodeA, Node nodeB, Material material)
    {
        FirstNode = nodeA;
        SecondNode = nodeB;
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = material;
        lineRenderer.positionCount = 2;
        
        PositionUpdate();
        //SetLength();
    }

    private void PositionUpdate()
    {
        if (!FirstNode || !SecondNode) return;
        
        lineRenderer.SetPosition(0, FirstNode.transform.position);
        lineRenderer.SetPosition(1, SecondNode.transform.position);
    }

    private void SetLength()
    {
        float length = (lineRenderer.GetPosition(0) - lineRenderer.GetPosition(1)).magnitude;
        float quarterLength = length / 4f;
        float end = length - quarterLength;
        
        /*Material material = lineRenderer.material;
        material.SetFloat("_EdgeCutoffStart", end);
        material.SetFloat("_EdgeCutOffEnd", quarterLength);*/
    }

    public void Fade(float value)
    {
        lineRenderer.material.SetFloat("_FadeDistance", value);
    }
}
