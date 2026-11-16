using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISoundComponent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] [Title("Sound Settings")]
    private UISoundFlag events = UISoundFlag.Hover | UISoundFlag.Click;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if ((events & UISoundFlag.Hover) != 0)
            AudioManager.Instance.Play("ui_snd_mouseover");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((events & UISoundFlag.Click) != 0)
            AudioManager.Instance.Play("ui_snd_select");
    }
}
