using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FTS.Characters;
using TMPro;

public class CharacterStatsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI movementText;
    [SerializeField] TextMeshProUGUI descriptionText;

    public void UpdateUI(Player player)
    {
        nameText.text = player.name;
        healthText.text = "Health: " + player.GetStat(Stat.Health).ToString();
        movementText.text = "Movement: " + player.GetStat(Stat.Movement).ToString();
        descriptionText.text = player.Description;
    }
}
