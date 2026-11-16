using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ToggleColorApplier : ColorApplier
{
    private Toggle toggle;

    protected override void Awake()
    {
        base.Awake();
        toggle = GetComponent<Toggle>();
    }

    protected override void ChangeColor(Color newColor)
    {
        if (currentColor == newColor) return;
        
        currentColor = newColor;
        newColor.a = toggle.colors.normalColor.a;
        
        ColorBlock colorBlock = toggle.colors;
        colorBlock.normalColor = newColor;
        colorBlock.pressedColor = newColor;
        toggle.colors = colorBlock;
    }
}
