using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkyMapCollider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private TMP_Text characterText;

    public void Deactivate()
    {
        characterText.enabled = false;
        gameObject.SetActive(false);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        characterText.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        characterText.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        NodeViewManager.Instance.DeactivateSkyMap();
        NodeViewManager.Instance.ActivateNodeViewFromSkyMap();
    }
}
