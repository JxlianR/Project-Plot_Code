using System;
using UnityEngine;
using UnityEngine.UI;

public class ArcTemplateButton : MonoBehaviour
{
    private Button button;
    
    [SerializeField]
    private ArcCreationSequence arcCreationSequence;

    [SerializeField]
    private ArcType type;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        arcCreationSequence.SpawnCharacterArc(type);
    }
}
