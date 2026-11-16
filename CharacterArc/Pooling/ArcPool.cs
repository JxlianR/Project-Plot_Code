using System.Collections.Generic;
using UnityEngine;

public class ArcPool
{
    private readonly CharacterArc prefab;
    private readonly Transform parent;
    private readonly Stack<CharacterArc> freeArcs = new Stack<CharacterArc>();

    public ArcPool(CharacterArc arcPrefab, Transform parentObject)
    {
        prefab = arcPrefab;
        parent = parentObject;
    }

    public CharacterArc Get()
    {
        if (freeArcs.Count > 0)
        {
            CharacterArc arc = freeArcs.Pop();
            arc.ResetArc();
            arc.gameObject.SetActive(true);
            
            return arc;
        }

        CharacterArc newArc = GameObject.Instantiate(prefab, parent);
        return newArc;
    }

    public void Release(CharacterArc arc)
    {
        arc.gameObject.SetActive(false);
        freeArcs.Push(arc);
    }
}
