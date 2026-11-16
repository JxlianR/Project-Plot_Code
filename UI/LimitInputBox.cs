using TMPro;
using UnityEngine;

public class LimitInputBox : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text inputText;
    
    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputText = inputField.textComponent;
        
        inputField.onValueChanged.AddListener(CheckFullField);
    }

    private void CheckFullField(string text)
    {
        if (inputText.isTextOverflowing)
            inputField.text = text.Substring(0, text.Length - 1);
    }
}
