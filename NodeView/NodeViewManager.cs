using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeViewManager : MonoBehaviour
{
    public static NodeViewManager Instance { get; private set; }
    
    [SerializeField]
    private GameObject nodeView;
    [SerializeField]
    private GameObject nodeViewCanvas;
    [SerializeField]
    private AddArcButton addArcButton;
    [SerializeField]
    private ArcPoolingManager arcPoolingManager;

    [SerializeField]
    private SkyMap skyMap;

    public Planet CurrentPlanet { get; private set; }
    private List<CharacterArc> currentArcs = new List<CharacterArc>();

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        DeactivateNodeView();
    }

    public void OnPlanetClick(Planet planet)
    {
        ActivateNodeView(planet);
        
        /*if (planet.Arcs.Count != 0) ActivateSkyMap(planet);
        else ActivateNodeView(planet);*/
    }

    private void ActivateSkyMap(Planet planet)
    {
        if (CurrentPlanet != planet)
        {
            CurrentPlanet = planet;
            LoadArcsSkyMap();
        }
        
        skyMap.Activate(CurrentPlanet.transform.TransformPoint(Vector3.zero), currentArcs);
        
        foreach (CharacterArc arc in currentArcs)
        {
            arc.skyMapCollider.SetActive(true);
        }
    }

    public void DeactivateSkyMap()
    {
        skyMap.Deactivate();
    }

    public void ActivateNodeViewFromSkyMap()
    {
        foreach (CharacterArc arc in currentArcs)
        {
            SkyMapCollider skyMapCollider = arc.GetComponentInChildren<SkyMapCollider>();
            skyMapCollider.Deactivate();
        }
        
        ActivateNodeView(CurrentPlanet);
    }

    private void ActivateNodeView(Planet planet)
    {
        CurrentPlanet = planet;
        nodeView.SetActive(true);
        nodeViewCanvas.SetActive(true);
        
        // Only as longs Sky Map is deactivated
        foreach (CharacterArc arc in currentArcs)
            arcPoolingManager.Release(arc);
        
        currentArcs.Clear();
        addArcButton.DeleteButtons();
        
        foreach (ArcData data in CurrentPlanet.Arcs)
        {
            CharacterArc arc = arcPoolingManager.Get(data.type);
            arc.ReuseArc(data, CurrentPlanet.Arcs.Count + 1);
            currentArcs.Add(arc);
            arc.gameObject.SetActive(true);
            
            addArcButton.SpawnButton(arc.GetCharacterName());
            int lastButton = addArcButton.ArcButtons.Count - 1;
            addArcButton.ArcButtons[lastButton].GetComponent<ToggleCharacterSelectorButton>().SetCharacterArc(arc);
        }
        
        CharacterArcSpawner.Instance.SetThemeIndex(CurrentPlanet.Arcs.Count);
        
        // Switch to the arcs that got clicked on
        if (currentArcs.Count > 0)
        {
            CharacterArcManager.Instance.SwitchArcs(currentArcs[0]);
            addArcButton.ArcButtons[0].GetComponent<Toggle>().isOn = true;
        }
        else
        {
            CharacterArcManager.Instance.SwitchArcs(null);
        }
        
        // Everything above only as long as Sky Map is deactivated
        
        /*if (CurrentPlanet.Arcs.Count != 0)
            LoadArcsNodeView();*/
    }

    public void DeactivateNodeView()
    {
        nodeView.SetActive(false);
        nodeViewCanvas.SetActive(false);
    }

    private void LoadArcsSkyMap()
    {
        //Check if planet has serializeArcData and spawn it
        
        foreach (CharacterArc arc in currentArcs)
            arcPoolingManager.Release(arc);
        
        currentArcs.Clear();

        foreach (ArcData data in CurrentPlanet.Arcs)
        {
            CharacterArc arc = arcPoolingManager.Get(data.type);
            arc.ReuseArc(data, CurrentPlanet.Arcs.Count + 1);
            currentArcs.Add(arc);
            arc.gameObject.SetActive(true);
        }
    }

    private void LoadArcsNodeView()
    {
        addArcButton.DeleteButtons();
        
        foreach (CharacterArc arc in currentArcs)
        {
            arc.gameObject.transform.localPosition = arc.Data.initialPos;
            
            addArcButton.SpawnButton(arc.GetCharacterName());
            int lastButton = addArcButton.ArcButtons.Count - 1;
            addArcButton.ArcButtons[lastButton].GetComponent<ToggleCharacterSelectorButton>().SetCharacterArc(arc);
        }
        CharacterArcSpawner.Instance.SetThemeIndex(CurrentPlanet.Arcs.Count);
        
        // Switch to the arcs that got clicked on
        if (currentArcs.Count > 0)
        {
            CharacterArcManager.Instance.SwitchArcs(currentArcs[0]);
            addArcButton.ArcButtons[0].GetComponent<Toggle>().isOn = true;
        }
        else
        {
            CharacterArcManager.Instance.SwitchArcs(null);
        }
    }

    public void AddArc(CharacterArc arc) => currentArcs.Add(arc);

    public void SetCurrentPlanet(Planet planet) => CurrentPlanet = planet;
}
