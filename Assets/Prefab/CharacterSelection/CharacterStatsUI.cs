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
    [SerializeField] TextMeshProUGUI attackRangeText;
    [SerializeField] TextMeshProUGUI supportRangeText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI(Player player)
    {
        nameText.text = player.name;
        healthText.text = "Health: " + player.GetStat(Stat.Health).ToString();
        movementText.text = "Movement: " + player.GetStat(Stat.Movement).ToString();
        attackRangeText.text = "Attack Range: " + player.GetStat(Stat.AttackRange).ToString();
        supportRangeText.text = "Support Range: " + player.GetStat(Stat.SupportRange).ToString();
    }
}
