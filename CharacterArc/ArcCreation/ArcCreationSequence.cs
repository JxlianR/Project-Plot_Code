using UnityEngine;

public class ArcCreationSequence : MonoBehaviour, IArcCreationSequence
{
    private CharacterData characterData;

    [Header("UI Manager for Arc Creation")][SerializeField]
    private ArcCreationUI ui;

    private ArcCreationState creationState;

    private bool changedState;

    public void StartSequence()
    {
        if (creationState != ArcCreationState.Inactive)
            return;
            
        InputManager.Instance.OnClick += CheckClick;
        ChangeState(ArcCreationState.EnterCharacterInfo);
    }

    public void SubmitCharacterData(CharacterData data)
    {
        if (creationState != ArcCreationState.EnterCharacterInfo)
            return;
        
        characterData = data;
        ChangeState(ArcCreationState.ChooseTemplate);
    }

    public void SpawnCharacterArc(ArcType type)
    {
        if (creationState != ArcCreationState.ChooseTemplate)
            return;
        
        ui.SpawnArcButton(characterData.name);
        CharacterArcSpawner.Instance.SpawnArc(type, characterData);
        ChangeState(ArcCreationState.EditNodes);
        Abort();
    }

    private void CheckClick(Vector3 position)
    {
        if (creationState == ArcCreationState.Inactive)
            return;

        if (changedState)
        {
            changedState = false;
            return;
        }

        if (!ui.ClickedOnUI(creationState, position))
        {
            Abort();
            ChangeState(ArcCreationState.Inactive);
        }
    }

    private void Abort()
    {
        InputManager.Instance.OnClick -= CheckClick;
        characterData = null;
        ChangeState(ArcCreationState.Inactive);
        changedState = false;
    }

    private void ChangeState(ArcCreationState newState)
    {
        creationState = newState;
        ui.OnStateChanged(creationState);
        changedState = true;
    }

    public void OnCloseClick()
    {
        Abort();
    }
    
    //Only for the showcase!
    public void SerializeArcs(ArcType type, CharacterData character, ArcSerializeData data)
    {
        ui.SpawnArcButton(character.name);
        CharacterArcSpawner.Instance.SerializeArc(type, character, data);
    }
}
