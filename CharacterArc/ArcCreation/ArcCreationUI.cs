using System;
using UnityEngine;

public class ArcCreationUI : MonoBehaviour
{
    [Header("UI")] //[SerializeField]
    //private GameObject plusButton;
    [SerializeField]
    private GameObject arcTemplates;
    [SerializeField]
    private GameObject subArcTemplates;
    [SerializeField]
    private CharacterInfoPanel characterInfoPanel;
    [SerializeField]
    private AddArcButton addArcButton;
    [SerializeField]
    private GameObject arcPlusButton;
    [SerializeField]
    private NodeContextMenu nodeContextMenu;

    public void OnStateChanged(ArcCreationState state)
    {
        switch (state)
        {
            case ArcCreationState.Inactive:
                HideAll();
                break;
            case ArcCreationState.EnterCharacterInfo:
                characterInfoPanel.Open();
                break;
            case ArcCreationState.ChooseTemplate:
                ShowTemplates();
                break;
            case ArcCreationState.EditNodes:
                OpenNodeContextMenu();
                break;
        }
    }

    private void ShowTemplates()
    {
        //plusButton.SetActive(false);
        arcTemplates.SetActive(true); 
    }

    private void HideTemplates()
    {
        //plusButton.SetActive(true);
        arcTemplates.SetActive(false); 
    }

    private void HideAll()
    {
        HideTemplates();
        subArcTemplates.SetActive(false);
        characterInfoPanel.Close();
    }

    public void SpawnArcButton(string characterName)
    {
        HideTemplates();
        addArcButton.SpawnButton(characterName);
    }

    private void OpenNodeContextMenu()
    {
        Node currentNode = CharacterArcManager.Instance.CurrentArc.Nodes[0];
        nodeContextMenu.Open(currentNode);
    }

    public bool ClickedOnUI(ArcCreationState state, Vector3 position)
    {
        switch (state)
        {
            case ArcCreationState.Inactive:
                return false;
            case ArcCreationState.EnterCharacterInfo:
                return PositionOnUI(characterInfoPanel.Rect, position) ||
                       PositionOnUI(arcPlusButton.GetComponent<RectTransform>(), position);
            case ArcCreationState.ChooseTemplate:
                return PositionOnUI(arcTemplates.GetComponent<RectTransform>(), position);
        }
        return false;
    }

    private bool PositionOnUI(RectTransform rect, Vector3 position)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rect, position);
    }
}
