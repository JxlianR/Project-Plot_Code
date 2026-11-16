using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject ui;
    public RectTransform Rect { get; private set; }
    
    [SerializeField]
    private ArcCreationSequence arcCreationSequence;
    private IArcCreationSequence sequence;

    [SerializeField]
    private Image background;
    
    [SerializeField]
    private TMP_Text errorText;
    
    [Header("Input Fields")][SerializeField]
    private TMP_InputField characterName;
    [SerializeField]
    private TMP_InputField goal;

    private void Awake()
    {
        sequence = arcCreationSequence as IArcCreationSequence;
        
        if (sequence == null)
            Debug.LogError("arcCreationSequence reference in CharacterInfoPanel is null");

        Rect = ui.GetComponent<RectTransform>();
    }

    private void Start()
    {
        ClearData();
        ui.SetActive(false);
    }

    public void Open()
    {
        ui.SetActive(true);
    }

    public void Close()
    {
        ClearData();
        ui.SetActive(false);
    }

    private void ClearData()
    {
        errorText.text = "";
        
        TMP_InputField[] fields = { characterName, goal };
        foreach (TMP_InputField field in fields)
            field.text = "";
    }

    public void OnNextClick()
    {
        if (!AllFieldsFilled())
        {
            ShowErrorMessage();
            return;
        }
        
        CharacterData data = CreateCharacterData();
        sequence.SubmitCharacterData(data);
        
        Close();
    }

    public void OnClose()
    {
        sequence.OnCloseClick();
    }

    private bool AllFieldsFilled()
    {
        TMP_InputField[] fields = { characterName, goal };
        if (fields.Any(field => string.IsNullOrWhiteSpace(field.text)))
            return false;

        return true;
    }

    private CharacterData CreateCharacterData()
    {
        CharacterData data = new CharacterData
        {
            name = characterName.text,
            goal = goal.text
        };
        
        return data;
    }

    private void ShowErrorMessage()
    {
        errorText.text = "Fill out all fields";
    }
}
