using FTS.Cards;
using FTS.Turns;
using UnityEngine;

namespace FTS.Characters
{
    public class SummonerAbility : MonoBehaviour, IAbility
    {
        [SerializeField] int cost;
        CardController cardController;
        [SerializeField] Sprite abiltyImage;
        [SerializeField] Card card;
        Player player;
        bool isUsed = false;

        public int Cost { get => cost; set => cost = value; }
        public Sprite AbiltyImage { get => abiltyImage; set => abiltyImage = value; }
        public bool IsUsed { get => isUsed; set => isUsed = value; }

        private void Start()
        {
            cardController = FindObjectOfType<CardController>().GetComponent<CardController>();
            TurnController.OnPlayerTurn += TurnController_OnPlayerTurn;
            player = GetComponent<Player>();
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
                cardController.AddCard(card, false, CardLocation.Hand, true);
            }
        }

        public string GetDescription()
        {
            return "Create a free drone card" +
                "\nCost " + cost + " health";
        }
    }
}
