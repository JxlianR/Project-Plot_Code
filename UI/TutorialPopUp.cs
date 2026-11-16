using TMPro;
using UnityEngine;

public class TutorialPopUp : MonoBehaviour
{
    [SerializeField]
    private TMP_Text beatInfo;
    [SerializeField]
    private TMP_Text[] keyElements;

    public void SetUpTutorialInfo(NodeTutorialEntry tutorialEntry)
    {
        beatInfo.text = tutorialEntry.summary;

        for (int i = 0; i < keyElements.Length; i++)
            keyElements[i].text = tutorialEntry.keyElements[i];
    }
}
