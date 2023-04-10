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
    [SerializeField] TextMeshProUGUI abiltyText;
    [SerializeField] Image abiltyImage;

    public void UpdateUI(Player player)
    {
        nameText.text = player.name;
        healthText.text = "Health: " + player.GetStat(Stat.Health).ToString();
        movementText.text = "Movement: " + player.GetStat(Stat.Movement).ToString();
        descriptionText.text = player.Description;
        IAbility abilty = player.GetComponent<IAbility>();
        abiltyText.text = abilty.GetDescription();
        abiltyImage.sprite = abilty.AbiltyImage;
    }
}
