using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class HoverClickSoundComponent : MonoBehaviour
{
    [SerializeField] [Title("Sound Settings")]
    private UISoundFlag events = UISoundFlag.Hover | UISoundFlag.Click;

    [SerializeField]
    private NavigationManager.ViewLevel requiredLevel; 

    private void OnMouseEnter()
    {
        if ((events & UISoundFlag.Hover) != 0 && NavigationManager.instance.currentLevel == requiredLevel)
            AudioManager.Instance.Play("ui_snd_mouseover");
    }

    private void OnMouseDown()
    {
        if ((events & UISoundFlag.Click) != 0 && NavigationManager.instance.currentLevel == requiredLevel)
            AudioManager.Instance.Play("ui_snd_select");
    }
}
