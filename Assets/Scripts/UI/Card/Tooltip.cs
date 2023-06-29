using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] GameObject toolTipBorder;
    [SerializeField] Image toolTipIamge;
    [SerializeField] TextMeshProUGUI keywordNameText;
    [SerializeField] TextMeshProUGUI keywordText;
    
    public void SetToolTip(Sprite image, string name, string text)
    {
        if (image != null)
        {
            toolTipIamge.sprite = image;
        }
        else
        {
            toolTipBorder.SetActive(false);
        }
        keywordNameText.text = name; 
        keywordText.text = text;
    }
}
