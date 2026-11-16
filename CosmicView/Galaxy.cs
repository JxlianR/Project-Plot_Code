
public class Galaxy : Interactable
{
    protected override void OnClick()
    {
        CreationHandler.instance.selectedGalaxy = gameObject.transform.root.gameObject;
    }
}
