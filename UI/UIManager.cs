using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    [SerializeField]
    private LocationDisplay galaxyName;
    [SerializeField]
    private LocationDisplay solarSystemName;
    [SerializeField]
    private LocationDisplay planetName;
    [SerializeField]
    private TMP_Text hoverFeedback;
    [SerializeField]
    private TMP_InputField planetNameInput;

    [SerializeField]
    private GameObject returnButtons;
    
    private GameObject currentView;
    
    [Header("Display Canvas")]
    [SerializeField] private GameObject galaxyViewCanvas;
    [SerializeField] private GameObject solarSystemViewCanvas;
    [SerializeField] private GameObject nodeViewCanvas;
    [SerializeField] private GameObject mainMenuCanvas;
    
    /// <summary>
    /// Initializes the singleton instance of the UIManager.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        Reset();
        UpdateUI();
    }

    /// <summary>
    /// Updates the active UI view based on the current navigation level.
    /// </summary>
    public void UpdateUI()
    {
        switch (NavigationManager.instance.currentLevel)
        {
            case NavigationManager.ViewLevel.Cosmic:
                SetActiveView(mainMenuCanvas);
                galaxyName.gameObject.SetActive(false);
                solarSystemName.gameObject.SetActive(false);
                planetName.gameObject.SetActive(false);
                returnButtons.SetActive(false);
                break;
            
            case NavigationManager.ViewLevel.Galaxy:
                SetActiveView(galaxyViewCanvas);
                galaxyName.gameObject.SetActive(true);
                solarSystemName.gameObject.SetActive(false);
                planetName.gameObject.SetActive(false);
                returnButtons.SetActive(true);
                break;

            case NavigationManager.ViewLevel.SolarSystem:
                SetActiveView(solarSystemViewCanvas);
                galaxyName.gameObject.SetActive(true);
                solarSystemName.gameObject.SetActive(true);
                planetName.gameObject.SetActive(false);
                returnButtons.SetActive(true);
                break;
            
            /*case NavigationManager.ViewLevel.Planet:
                SetActiveView(solarSystemViewCanvas);
                solarSystemName.gameObject.SetActive(true);
                planetName.gameObject.SetActive(false);
                break;*/
                
            case NavigationManager.ViewLevel.NodeSequence:
                SetActiveView(nodeViewCanvas);
                galaxyName.gameObject.SetActive(true);
                solarSystemName.gameObject.SetActive(true);
                planetName.gameObject.SetActive(true);
                returnButtons.SetActive(true);
                break;

            default:
                SetActiveView(mainMenuCanvas);
                galaxyName.gameObject.SetActive(false);
                solarSystemName.gameObject.SetActive(false);
                planetName.gameObject.SetActive(false);
                returnButtons.SetActive(false);
                break;
        }
        
        UpdateTextVisibility(NavigationManager.instance.currentLevel);
    }
    
    
    
    /// <summary>
    /// Updates the visibility of the text elements based on the current view level.
    /// </summary>
    /// <param name="level">The current view level.</param>
    private void UpdateTextVisibility(NavigationManager.ViewLevel level)
    {
        /*bool showGalaxy = (level == NavigationManager.ViewLevel.Cosmic || level == NavigationManager.ViewLevel.Galaxy);
        SetTextListVisibility(galaxyName, showGalaxy);

        bool showSolarSystem = (level == NavigationManager.ViewLevel.Galaxy);
        SetTextListVisibility(solarSystemName, showSolarSystem);

        bool showPlanet = (level == NavigationManager.ViewLevel.SolarSystem || level == NavigationManager.ViewLevel.Planet);
        SetTextListVisibility(planetName, showPlanet);

        if (planetNameForSolarSystem != null && planetNameForSolarSystem.gameObject.activeSelf != showPlanet)
        {
            planetNameForSolarSystem.gameObject.SetActive(showPlanet);
        }*/
    }
    
    /// <summary>
    /// Sets the visibility of a list of text components.
    /// </summary>
    /// <typeparam name="T">The type of the text component.</typeparam>
    /// <param name="textList">The list of text components.</param>
    /// <param name="isVisible">Whether the text components should be visible.</param>
    private void SetTextListVisibility<T>(List<T> textList, bool isVisible) where T : Component
    {
        for (int i = 0; i < textList.Count; i++)
        {
            if (textList[i] != null)
            {
                if (textList[i].gameObject.activeSelf != isVisible)
                {
                    textList[i].gameObject.SetActive(isVisible);
                }
            }
        }
    }

    /// <summary>
    /// Sets the active UI canvas, deactivating all others.
    /// </summary>
    /// <param name="activeCanvas">The canvas to be set as active.</param>
    private void SetActiveView(GameObject activeCanvas)
    {
        if (currentView == activeCanvas) return;
        
        /*galaxyViewCanvas.SetActive(false);
        solarSystemViewCanvas.SetActive(false);
        nodeViewCanvas.SetActive(false);
        mainMenuCanvas.SetActive(false);*/
        
        currentView.SetActive(false);

        if (activeCanvas != null)
        {
            activeCanvas.SetActive(true);
            currentView = activeCanvas;
        }
    }

    private void Reset()
    {
        galaxyViewCanvas.SetActive(false);
        solarSystemViewCanvas.SetActive(false);
        nodeViewCanvas.SetActive(false);
        
        currentView = mainMenuCanvas;
        
        galaxyName.gameObject.SetActive(false);
        solarSystemName.gameObject.SetActive(false);
        planetName.gameObject.SetActive(false);
    }

    /// <summary>
    /// Changes the text of all galaxy name labels.
    /// </summary>
    /// <param name="galaxy">The new name for the galaxy.</param>
    public void ChangeGalaxyName(string galaxy) => galaxyName.UpdateLocationText(galaxy);

    /// <summary>
    /// Changes the text of all solar system name labels.
    /// </summary>
    /// <param name="solarSystem">The new name for the solar system.</param>
    public void ChangeSolarSystemName(string solarSystem) => solarSystemName.UpdateLocationText(solarSystem);

    /// <summary>
    /// Changes the text of all planet name labels.
    /// </summary>
    /// <param name="planet">The new name for the planet.</param>
    public void ChangePlanetName(string planet) => planetName.UpdateLocationText(planet);

    public void UpdateHoverFeedback(string hoverInfo) => hoverFeedback.text = hoverInfo; 

    public TMP_InputField GetPlanetNameInput() => planetNameInput;
}