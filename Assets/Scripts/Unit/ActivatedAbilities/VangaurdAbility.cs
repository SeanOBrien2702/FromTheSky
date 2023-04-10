using FTS.Cards;
using FTS.Turns;
using UnityEngine;

namespace FTS.Characters
{
    public class VangaurdAbility : MonoBehaviour, IAbility
    {
        CardController cardController;
        Player player;
        bool isUsed = false;

        [SerializeField] int cost;
        [SerializeField] Sprite abiltyImage;
        [SerializeField] int energyGained = 1;

        public int Cost { get => cost; set => cost = value; }
        public Sprite AbiltyImage { get => abiltyImage; set => abiltyImage = value; }
        public bool IsUsed { get => isUsed; set => isUsed = value; }

        private void Start()
        {
            cardController = FindObjectOfType<CardController>().GetComponent<CardController>();
            player = GetComponent<Player>();
            TurnController.OnPlayerTurn += TurnController_OnPlayerTurn;
        }

        private void OnDestroy()
        {
            TurnController.OnPlayerTurn -= TurnController_OnPlayerTurn;
        }

        private void TurnController_OnPlayerTurn()
        {
            isUsed = false;
        }

        public void ActivateEffect()
        {
            if (player.Health - cost > 0)
            {
                player.Health -= cost;
                cardController.DrawCard();
                player.Energy += energyGained;
                isUsed = true;
            }
        }

        public string GetDescription()
        {
            return "Draw 1 card and gain 1 energy" +
                "\nCost " + cost + " health";
        }
    }
}
