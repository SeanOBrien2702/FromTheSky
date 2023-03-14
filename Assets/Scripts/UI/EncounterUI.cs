using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncounterUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image image;
    Button button;

    public TextMeshProUGUI NameText { get => nameText; set => nameText = value; }
    public Image Image { get => image; set => image = value; }
    public Button Button { get => button; set => button = value; }

    private void Awake()
    {
        button = GetComponent<Button>();
    }
}
