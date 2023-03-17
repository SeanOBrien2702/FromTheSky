using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI keywordNameText;
    [SerializeField] TextMeshProUGUI keywordText;
    
    public void SetToolTip(string name, string text)
    {
        keywordNameText.text = name; 
        keywordText.text = text;
    }
}
