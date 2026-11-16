using System;
using UnityEngine;

public class PlanetTextScaler : MonoBehaviour
{
    [SerializeField]
    private float worldSize;
    
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 parentScale = rectTransform.parent.lossyScale;

        rectTransform.localScale =
            new Vector3(worldSize / parentScale.x, worldSize / parentScale.y, worldSize / parentScale.z);
    }
}
