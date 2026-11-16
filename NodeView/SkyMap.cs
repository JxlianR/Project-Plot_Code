using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkyMap : MonoBehaviour
{
    private Vector2 disillusionmentQuarter = new Vector2(0.25f, 0.75f);
    private Vector2 fallCorruptionQuarter = new Vector2(0.75f, 0.75f);
    private Vector2 positiveChangeQuarter = new Vector2(0.75f, 0.25f);
    private Vector2 flatQuarter = new Vector2(0.25f, 0.25f);

    [SerializeField] [Title("Camera Settings")]
    private Camera skyMapCam;
    [SerializeField]
    private float camDistanceToPlanet;

    [Title("Arc Positioning")] [SerializeField]
    private float arcDistanceToPlanet;
    [Tooltip("Offset in Viewport Units")] [SerializeField] [Range(0f, 0.2f)]
    private float xViewportOffset;
    [Tooltip("Offset in Viewport Units")] [SerializeField] [Range(0f, 0.2f)]
    private float yViewportOffset;

    private void Start()
    {
        Deactivate();
    }

    public void Activate(Vector3 planetPosition, List<CharacterArc> arcs)
    {
        PlaceArcs(arcs);

        Vector3 newCamPos = new Vector3(planetPosition.x, planetPosition.y, planetPosition.z + camDistanceToPlanet);
        skyMapCam.transform.localPosition = newCamPos;
        //skyMapCam.transform.LookAt(planetPosition);
        skyMapCam.enabled = true;
    }
    
    private void PlaceArcs(List<CharacterArc> arcs)
    {
        foreach (CharacterArc arc in arcs)
        {
            Vector2 quarterPos = GetQuarterByArcType(arc.Data.type);
            Vector3 newPos = GetRandomPositionInQuarter(quarterPos);
            arc.transform.position = newPos;
        }
    }

    private Vector2 GetQuarterByArcType(ArcType type)
    {
        switch (type)
        {
            case ArcType.Disillusionment:
                return disillusionmentQuarter;
            case ArcType.Fall:
            case ArcType.Corruption:
                return fallCorruptionQuarter;
            case ArcType.Flat:
                return flatQuarter;
            case ArcType.PositiveChange:
                return positiveChangeQuarter;
        }

        return positiveChangeQuarter;
    }

    private Vector3 GetRandomPositionInQuarter(Vector2 quarterCenter)
    {
        float xOffset = Random.Range(-xViewportOffset, xViewportOffset);
        float yOffset = Random.Range(-yViewportOffset, yViewportOffset);

        Vector3 viewportPosition = new Vector3(quarterCenter.x + xOffset, quarterCenter.y + yOffset, arcDistanceToPlanet + camDistanceToPlanet);
        return skyMapCam.ViewportToWorldPoint(viewportPosition);
    }

    public void Deactivate()
    {
        skyMapCam.enabled = false;
    }
}
