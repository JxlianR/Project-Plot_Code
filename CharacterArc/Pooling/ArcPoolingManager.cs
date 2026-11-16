using System;
using System.Collections.Generic;
using UnityEngine;

public class ArcPoolingManager : MonoBehaviour
{
    [System.Serializable]
    public class ArcPoolEntry
    {
        public ArcType type;
        public CharacterArc prefab;
    }

    [SerializeField]
    private List<ArcPoolEntry> arcPools;
    private Dictionary<ArcType, ArcPool> pools;

    private void Awake()
    {
        pools = new Dictionary<ArcType, ArcPool>();

        foreach (ArcPoolEntry entry in arcPools)
        {
            ArcPool pool = new ArcPool(entry.prefab, transform);
            pools.Add(entry.type, pool);
        }
    }

    public CharacterArc Get(ArcType type)
    {
        return pools[type].Get();
    }

    public void Release(CharacterArc arc)
    {
        pools[arc.Data.type].Release(arc);
    }
}
