using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI
{
    public class EncounterUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI objectiveText;
        [SerializeField] Image image;
        Button button;

        public TextMeshProUGUI NameText { get => nameText; set => nameText = value; }
        public Image Image { get => image; set => image = value; }
        public Button Button { get => button; set => button = value; }
        public TextMeshProUGUI ObjectiveText { get => objectiveText; set => objectiveText = value; }

        private void Awake()
        {
            button = GetComponent<Button>();
        }
    }
}