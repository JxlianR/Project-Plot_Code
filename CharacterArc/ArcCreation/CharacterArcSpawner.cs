using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterArcSpawner : MonoBehaviour
{
    public static CharacterArcSpawner Instance { get; private set; }
    
    [HideInInspector] public bool characterSelected = false;
    private string selectedCharacter = null;
    
    private ToggleCharacterSelectorButton currentCharacterButton;
    
    //private float rearZ = 0;
    [SerializeField]
    private float zDifference;
    
    private GameObject lastArc;

    [SerializeField]
    private ArcColorThemeSet colorThemeSet;
    private int currentThemeIndex;

    [SerializeField]
    private ArcPoolingManager arcPoolingManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Debug.Log("Spawner Awake: " + gameObject.name, gameObject);

        if (Instance != null && Instance != this)
        {
            Debug.LogError("Spawner MULTIPLE CharacterArcSpawners detected!", this);
        }

        Instance = this;
    }

    public void SpawnArc(ArcType type, CharacterData characterData)
    {
        Planet currentPlanet = NodeViewManager.Instance.CurrentPlanet;

        CharacterArc arc = arcPoolingManager.Get(type);
        lastArc = arc.gameObject;
        
        float rearZ = (currentPlanet.Arcs.Count + 1) * zDifference;
        float randomX = Random.Range(-4f, 4f);
        float randomY = Random.Range(-3f, 3f);
        
        Vector3 offset = new Vector3(randomX, randomY, rearZ);
        lastArc.transform.localPosition = offset;

        ArcColorThemeSet.ArcColorTheme theme = GetNextTheme();
        arc.InitializeNewArc(type, characterData, theme);
        CharacterArcManager.Instance.AddArc(arc.Data);
        currentPlanet.AddArc(arc);
        NodeViewManager.Instance.AddArc(arc);
        
        currentCharacterButton.SetCharacterArc(arc);
        currentCharacterButton = null;
    }

    private ArcColorThemeSet.ArcColorTheme GetNextTheme()
    {
        ArcColorThemeSet.ArcColorTheme theme = colorThemeSet.themes[currentThemeIndex];
        currentThemeIndex = (currentThemeIndex + 1) % colorThemeSet.themes.Count;
        
        return theme;
    }
    
    public void SelectCharacter(string id, ToggleCharacterSelectorButton button)
    {
        selectedCharacter = id;
        characterSelected = true;
        currentCharacterButton = button;
    }
    
    public void ClearSelection()
    {
        selectedCharacter = null;
        characterSelected = false;
    }
    
    //Showcase Code
    public void SerializeArc(ArcType type, CharacterData characterData, ArcSerializeData data)
    {
        SpawnArc(type, characterData);
        CharacterArc arc = lastArc.GetComponent<CharacterArc>();
        //arc.SerializeArc(data);
    }

    public void SetThemeIndex(int index) => currentThemeIndex = index;
}
