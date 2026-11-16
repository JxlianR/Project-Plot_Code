using UnityEngine;

public class SolarSystem : SolarSystemInteractable
{
    [SerializeField]
    private string solarSystemName;
    [SerializeField]
    private Color solarSystemColor;

    private PlanetSpawn planetSpawn;
    
    protected override void Start()
    {
        base.Start();

        planetSpawn = GetComponentInParent<PlanetSpawn>();
        ObjectName = solarSystemName;
    }

    protected override void OnClick()
    {
        if (planetSpawn == null)
            return;
        
        UIManager.instance.ChangeSolarSystemName(solarSystemName);
        navigationManager.UpdateSolarSystemButton(solarSystemColor);
        
        CreationHandler.instance.spawnPlanetButton.onClick.RemoveAllListeners();
        CreationHandler.instance.spawnPlanetButton.onClick.AddListener(() => planetSpawn.SpawnSinglePlanet());
    }
}
