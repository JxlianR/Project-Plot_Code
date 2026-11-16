using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;
using SimpleFileBrowser;
using Sirenix.OdinInspector.Editor.Internal.UIToolkitIntegration;

public class NodeContextMenu : ContextMenu
{
    private Node currentNode;
    private CharacterArc currentArc;

    [Header("UI")] [SerializeField]
    private TMP_InputField eventName;
    [SerializeField]
    private TMP_InputField description;
    [SerializeField]
    private TMP_InputField relatedCharacters;
    [SerializeField]
    private TMP_InputField characterDevelopment;
    [SerializeField]
    private TMP_InputField ultimateGoal;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private Toggle defaultTab;
    [SerializeField]
    private TutorialPopUp tutorialPopUp;
    [SerializeField]
    private TMP_Text characterName;
    
    // Variable for evidence buttons -> later in own class
    [SerializeField]
    private Sprite evidenceButton;
    private float opacity;

    [SerializeField] private Image[] images;
    private int currentIndex = -1;

    private TMP_Text buttonText;

    private GameObject currentPage;
    
    [HideInInspector]
    public UnityEvent<Color> applyColor;

    public bool FileBrowserOpen { get; private set; }

    private void Awake()
    {
        buttonText = nextButton.GetComponentInChildren<TMP_Text>();
        opacity = images[0].color.a;
    }
    
    public override void Start()
    {
        base.Start();
        
        // Code from co-worker
        for (int i = 0; i < images.Length; i++)
        {
            int index = i;
            images[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                OpenFileBrowser(index);
            });
        }
    }

    public void Open(Node node)
    {
        currentNode = node;
        currentArc = node.GetArc();
        
        node.SetHighlighted(true);
        
        ChangeColor();
        ResetPage();
        
        IsOpen = true;
        menuUI.SetActive(true);
        rect.anchoredPosition = Vector2.zero;
        
        UpdateText();
        tutorialPopUp.SetUpTutorialInfo(node.TutorialEntry);
    }

    public override void Close()
    {
        base.Close();

        if (!currentNode) return;
        
        currentNode.SetHighlighted(false);
        currentNode.SaveData(eventName.text, description.text, relatedCharacters.text, characterDevelopment.text, ultimateGoal.text);
    }
    
    private void UpdateText()
    {
        NodeData data = currentNode.Data();
        eventName.text = data.eventName;
        description.text = data.description;
        relatedCharacters.text = string.Join(", ", data.associatedCharacters);

        CharacterData characterData = currentArc.GetCharacterData();
        characterDevelopment.text = characterData.development;
        ultimateGoal.text = characterData.goal;

        bool hasNextNode = currentArc.GetNextNode(currentNode) != null;
        buttonText.text = !hasNextNode ? "Close" : "Next";

        characterName.text = currentArc.GetCharacterName();

        for (int i = 0; i < images.Length; i++)
        {
            string path = "";
            
            if (currentNode.Data().evidencePaths.Count > i)
                path = currentNode.Data().evidencePaths[i];
            
            if (string.IsNullOrEmpty(path))
            {
                // Check if text is already active -> no picture here that needs to be changed
                GameObject text = images[i].transform.GetChild(0).gameObject;
                if (text.activeSelf) continue;

                // Activate text children
                images[i].transform.GetChild(0).gameObject.SetActive(true);
                
                images[i].sprite = evidenceButton;
                Color color = currentArc.GetColorThemeButton();
                color.a = opacity;
                images[i].color = color;

                VideoPlayer videoPlayer = images[i].GetComponentInChildren<VideoPlayer>();
                if (videoPlayer) Destroy(videoPlayer.gameObject);
                
                continue;
            }

            currentIndex = i;
            LoadMedia(path);
        }
    }

    public bool IsOpenFor(Node node)
    {
        if (currentNode == node) return true;
        return false;
    }

    public void OnNextClick()
    {
        Close();
        
        Node nextNode = currentArc.GetNextNode(currentNode);
        if (nextNode == null)
            return;
        
        Open(nextNode);
    }

    public void OpenPage(GameObject page)
    {
        if (currentPage == page)
            return;
        
        if (currentPage != null)
            currentPage.SetActive(false);

        page.SetActive(true);
        currentPage = page;
        ChangeColor();
    }
    
    private void ResetPage()
    {
        if (!defaultTab.isOn)
            defaultTab.isOn = true;
    }

    private void ChangeColor()
    {
        if (currentArc == null) return; 
        
        Color color = currentArc.GetColorThemeButton();
        applyColor.Invoke(color);
    }
    
    // From here on, code from co-workers
    void OpenFileBrowser(int index)
    {
        currentIndex = index;
        FileBrowserOpen = true;

        FileBrowser.SetFilters(true, 
            new FileBrowser.Filter("Media Files", ".png", ".jpg", ".jpeg", ".gif", ".mp4", ".mov", ".avi"));

        FileBrowser.SetDefaultFilter(".png");

        StartCoroutine(ShowLoadDialog());
    }

    IEnumerator ShowLoadDialog()
    {
        yield return FileBrowser.WaitForLoadDialog(
            FileBrowser.PickMode.Files,
            false,
            null,
            null,
            "Select Media",
            "Select"
        );

        if (FileBrowser.Success)
        {
            string path = FileBrowser.Result[0];
            LoadMedia(path);
            currentNode.Data().evidencePaths.Add(path);
        }
        else FileBrowserOpen = false;
    }
    void LoadMedia(string path)
    {
        string extension = Path.GetExtension(path).ToLower();

        if (extension == ".mp4" || extension == ".mov" || extension == ".avi")
        {
            ReplaceWithVideo(path);
        }
        else
        {
            ReplaceWithImage(path);
        }
        
        AudioManager.Instance.Play("ui_snd_enter");
        FileBrowserOpen = false;
    }

    void ReplaceWithImage(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);

        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);

        Sprite sprite = Sprite.Create(texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));

        Image targetImage = images[currentIndex];

        // Deactivate text children
        targetImage.transform.GetChild(0).gameObject.SetActive(false);

        targetImage.color = Color.white;

        targetImage.sprite = sprite;
        targetImage.preserveAspect = true;
    }

    void ReplaceWithVideo(string path)
    {
        Image targetImage = images[currentIndex];

        // Deactivate text children
        targetImage.transform.GetChild(0).gameObject.SetActive(false);

        targetImage.sprite = null;
        targetImage.color = Color.white;

        GameObject videoObj = new GameObject("VideoPlayer");
        videoObj.transform.SetParent(targetImage.transform, false);

        VideoPlayer vp = videoObj.AddComponent<VideoPlayer>();
        RawImage rawImage = videoObj.AddComponent<RawImage>();

        RectTransform rt = videoObj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        RenderTexture renderTexture = new RenderTexture(1920, 1080, 0);
        vp.targetTexture = renderTexture;

        rawImage.texture = renderTexture;

        vp.url = path;
        vp.isLooping = true;
        vp.Play();
    }
}
