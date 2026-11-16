using System;
using System.Collections.Generic;
using UnityEngine;

public class Planet : SolarSystemInteractable
{
    [SerializeField]
    private PlanetArcData arcData;
    public List<ArcData> Arcs { get; private set; }

    private void Awake()
    {
        Arcs = new List<ArcData>();
    }

    protected override void HandleMouseEnter()
    {
        //if (navigationManager.currentLevel == NavigationManager.ViewLevel.Planet)
            //UIManager.instance.planetNameForSolarSystem.text = gameObject.name;
    }

    protected override void OnClick()
    {
        NodeViewManager.Instance.OnPlanetClick(this);
        UIManager.instance.ChangePlanetName(gameObject.name);
    }

    protected override bool HandleSpecialCamera(Camera cam)
    {
        if (cam.gameObject.layer != 24)
            return false;
        
        navigationManager.activeCamera.gameObject.SetActive(false);
        cam.gameObject.SetActive(true);
        navigationManager.activeCamera = cam;
        navigationManager.cosmicCamera.gameObject.SetActive(false);

        return true;
    }

    public void AddArc(CharacterArc arc) => Arcs.Add(arc.Data);

    public PlanetArcData GetPlanetArcData() => arcData;
}
