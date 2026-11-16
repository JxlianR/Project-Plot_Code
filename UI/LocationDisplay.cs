using System;
using TMPro;
using UnityEngine;

public class LocationDisplay : MonoBehaviour
{
    private TMP_Text locationText;

    private void Awake()
    {
        locationText = GetComponentInChildren<TMP_Text>();
    }

    public void UpdateLocationText(string newText) => locationText.text = newText;
}
