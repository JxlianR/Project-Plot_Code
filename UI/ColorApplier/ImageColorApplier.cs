using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorApplier : ColorApplier
{
    private Image image;
    
    protected override void Awake()
    {
        base.Awake();
        image = GetComponent<Image>();
    }

    protected override void ChangeColor(Color newColor)
    {
        if (currentColor == newColor) return;
        currentColor = newColor;
        
        newColor.a = image.color.a;
        image.color = newColor;
    }
}
