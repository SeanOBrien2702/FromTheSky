using FTS.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusTooltip : MonoBehaviour
{
    [SerializeField] Image statusIamge;
    [SerializeField] TextMeshProUGUI statusNameText;
    [SerializeField] TextMeshProUGUI statusText;

    public void SetToolTip(Status status)
    {
        statusIamge.sprite = status.StatusImage;
        statusNameText.text = status.StatusName;
        statusText.text = status.StatusDescription;
    }
}
